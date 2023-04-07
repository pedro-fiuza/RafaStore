using RafaStore.Shared.Model;
using RafaStore.Shared.ViewModel;

namespace RafaStore.Client.Services.FileService
{
    public class FileService : IFileService
    {
        private readonly HttpClient httpClient;
        public event Action NotesChanged;
        public List<NoteFileModel> Notes { get; set; } = new();
        public string Message { get; set; } = "Carregando notas...";
        public int CurrentPage { get; set; } = 1;
        public int PageCount { get; set; } = 0;

        public FileService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<byte[]> DownloadPdf(int? noteId)
        {
            var file = await httpClient.PostAsJsonAsync($"api/customer/download-pdf?noteId={noteId}", new object());
            return await file.Content.ReadAsByteArrayAsync();
        }

        public async Task SearchNotes(DateTime? startDate, DateTime? endDate, int page)
        {
            var teste = $"api/note/search?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}&page={page}";
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<NoteFileListViewModel>>($"api/note/search?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}&page={page}");

            if (result != null && result.Data != null)
            {
                Notes = result.Data.Notes;
                CurrentPage = result.Data.CurrentPage;
                PageCount = result.Data.Pages;
            }

            if (Notes.Count == 0) Message = "Nenhuma nota foi encontrada...";
            NotesChanged?.Invoke();
        }

        public async Task GetAllNotesPaginated(int page)
        {
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<NoteFileListViewModel>>($"api/note?page={page}");

            if (result != null && result.Data != null)
            {
                Notes = result.Data.Notes;
                CurrentPage = result.Data.CurrentPage;
                PageCount = result.Data.Pages;
            }

            if (Notes.Count == 0) Message = "Nenhuma nota foi encontrada...";

            NotesChanged?.Invoke();
        }
    }
}
