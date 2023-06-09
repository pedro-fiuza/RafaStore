﻿using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.EntityFrameworkCore;
using RafaStore.Shared;
using RafaStore.Shared.Model;
using RafaStore.Shared.ViewModel;

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

    public async Task CreateFile(Stream file, CustomerModel customer, NoteFileModel note)
    {
        var blob = new BlobServiceClient(_configuration["Azure"]);

        BlobContainerClient containerClient = blob.GetBlobContainerClient(note.Blob);

        var blobClient = containerClient.GetBlobClient(note.FileName);

        await blobClient.UploadAsync(file, new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders
            {
                ContentType = "application/pdf",
            }
        });

        context.Note.Add(note);

        await context.SaveChangesAsync();
    }

    public async Task<bool> DeleteFile(int? noteId)
    {
        var note = await context.Note.SingleOrDefaultAsync(x => x.Id == noteId);

        var blob = new BlobServiceClient(_configuration["Azure"]);
        BlobContainerClient containerClient = blob.GetBlobContainerClient(note.Blob);
        var result = await containerClient.DeleteBlobIfExistsAsync(note.FileName);

        if (!result)
            return false;

        context.Note.Remove(note);
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<byte[]> DownloadToStream(string blob, string fileName)
    {
        var blobClient = new BlobClient(_configuration["Azure"], blob, fileName);
        var content = await blobClient.DownloadContentAsync();
        return content.Value.Content.ToArray();
    }

    public async Task<ServiceResponse<NoteFileListViewModel>> SearchNotes(int customerId, string startDate, string endDate, int page)
    {
        var customersFound = await FindNotesByDate(customerId, Convert.ToDateTime(startDate).Date, Convert.ToDateTime(endDate).Date);

        return await PaginateNotes(customersFound, page);
    }

    private async Task<List<NoteFileModel>> FindNotesByDate(int customerId, DateTime startDate, DateTime endDate)
    {
        return await context.Note
            .Where(r => r.CustomerModelId.Equals(customerId) && r.CreatedAt.Date >= startDate && r.CreatedAt.Date <= endDate)
            .ToListAsync();
    }
    public async Task<NoteFileModel?> GetNoteById(int noteId)
    {
        return await context.Note
            .AsNoTracking()
            .Include(x => x.CustomerModel)
            .SingleOrDefaultAsync(x => x.Id == noteId);
    }


    public async Task<ServiceResponse<NoteFileListViewModel>> GetAllNotesPaginated(int customerId, int page)
    {
        var notes = await context.Note.
            Where(x => x.CustomerModelId.Equals(customerId)).
            ToListAsync();

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