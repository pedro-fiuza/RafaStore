using RafaStore.Shared;
using RafaStore.Shared.Model;
using RafaStore.Shared.ViewModel;

namespace RafaStore.Server.Services.FileService
{
    public interface IFileService
    {
        Task CreateFile(Stream file, CustomerModel customer, NoteFileModel note);
        Task<byte[]> DownloadToStream(string blob, string fileNames);
        Task<ServiceResponse<NoteFileListViewModel>> GetAllNotesPaginated(int customerId, int page);
        Task<ServiceResponse<NoteFileListViewModel>> SearchNotes(int customerId, string startDate, string endDate, int page);
        Task<NoteFileModel?> GetNoteById(int noteId);
        Task<bool> DeleteFile(int? noteId);
    }
}
