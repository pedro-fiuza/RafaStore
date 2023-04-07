using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using RafaStore.Shared.ViewModel;
using RafaStore.Shared;
using RafaStore.Shared.Model;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace RafaStore.Server.Services.FileService;

public class FileService : IFileService
{
    private readonly IConfiguration _configuration;
    private readonly DataContext context;


    public FileService(IConfiguration configuration, DataContext context)
    {
        _configuration = configuration;
        this.context = context;
    }

    public async Task CreateFile(Stream file, CustomerModel customer)
    {
        var x = DateTime.ParseExact(DateTime.UtcNow.ToString(), "yyyy-MM-dd hh-mm-ss", new CultureInfo("pt-BR"));

        var fileName = $"{customer.Name}-{DateTime.Now.Date:dd-MM-yyyy}";

        var blob = new BlobServiceClient(_configuration["Azure:Connection"]);

        BlobContainerClient containerClient = blob.GetBlobContainerClient("rafastore");

        var blobClient = containerClient.GetBlobClient(fileName);

        await blobClient.UploadAsync(file, new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders
            {
                ContentType = "application/pdf",
            }
        });

        var note = new NoteFileModel
        {
            FileName = fileName,
            CustomerModelId = customer.Id
        };

        context.Note.Add(note);

        await context.SaveChangesAsync();
    }

    public async Task<bool> DeleteFile()
    {

        var blob = new BlobServiceClient(_configuration["Azure:Connection"]);

        BlobContainerClient containerClient = blob.GetBlobContainerClient("rafastore");
        containerClient.DeleteBlobIfExistsAsync("fileName");
    }

    public async Task<byte[]> DownloadToStream(string blob, string fileName)
    {
        var blobClient = new BlobClient(_configuration["Azure:Connection"], blob, fileName);
        var content = await blobClient.DownloadContentAsync();
        return content.Value.Content.ToArray();
    }

    public async Task<ServiceResponse<NoteFileListViewModel>> SearchNotes(string startDate, string endDate, int page)
    {
        var customersFound = await FindNotesByDate(Convert.ToDateTime(startDate).Date, Convert.ToDateTime(endDate).Date);

        return await PaginateNotes(customersFound, page);
    }

    private async Task<List<NoteFileModel>> FindNotesByDate(DateTime startDate, DateTime endDate)
    {
        return await context.Note
            .Where(r => r.CreatedAt.Date >= startDate && r.CreatedAt.Date <= endDate)
            .ToListAsync();
    }
    public async Task<NoteFileModel?> GetNoteById(int noteId)
    {
        return await context.Note
            .AsNoTracking()
            .Include(x => x.CustomerModel)
            .SingleOrDefaultAsync(x => x.Id == noteId);
    }


    public async Task<ServiceResponse<NoteFileListViewModel>> GetAllNotesPaginated(int page)
    {
        var notes = await context.Note.ToListAsync();

        return await PaginateNotes(notes, page);
    }

    private async Task<ServiceResponse<NoteFileListViewModel>> PaginateNotes(List<NoteFileModel> note, int page)
    {
        var pageResults = 5f;
        var pageCount = Math.Ceiling((note).Count / pageResults);

        var paginatedNotes = note.Skip((page - 1) * (int)pageResults)
            .Take((int)pageResults)
            .ToList();

        return new ServiceResponse<NoteFileListViewModel>
        {
            Data = new NoteFileListViewModel
            {
                Notes = paginatedNotes,
                CurrentPage = page,
                Pages = (int)pageCount,
            }
        };
    }
}