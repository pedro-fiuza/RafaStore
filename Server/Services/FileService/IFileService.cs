using RafaStore.Shared;
using RafaStore.Shared.Model;
using RafaStore.Shared.ViewModel;

namespace RafaStore.Server.Services.FileService
{
    public interface IFileService
    {
        Task CreateFile(Stream file, CustomerModel customer);
        Task<byte[]> DownloadToStream(string blob, string fileNames);
        Task<ServiceResponse<NoteFileListViewModel>> GetAllNotesPaginated(int page);
        Task<ServiceResponse<NoteFileListViewModel>> SearchNotes(string startDate, string endDate, int page);
        Task<NoteFileModel?> GetNoteById(int noteId);
    }
}
