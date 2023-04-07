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
        Task GetAllNotesPaginated(int page);
        Task SearchNotes(DateTime? startDate, DateTime? endDate, int page);
        Task<byte[]> DownloadPdf(int? noteId);
    }
}
