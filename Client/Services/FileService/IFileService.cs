using RafaStore.Shared.Model;

namespace RafaStore.Client.Services.FileService
{
    public interface IFileService
    {
        event Action NotesChanged;
        List<NoteFileModel> Notes { get; set; }
        string Message { get; set; }
        int CurrentPage { get; set; } 
        int PageCount { get; set; }
        Task GetAllNotesPaginated(int customerId, int page);
        Task SearchNotes(int customerId, DateTime? startDate, DateTime? endDate, int page);
        Task<byte[]> DownloadPdf(int? noteId);
        Task<bool> DeletePdf(int? noteId);
    }
}
