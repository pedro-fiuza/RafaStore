using RafaStore.Shared.Model;

namespace RafaStore.Shared.ViewModel
{
    public class NoteFileListViewModel
    {
        public List<NoteFileModel> Notes { get; set; } = new();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}
