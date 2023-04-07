namespace RafaStore.Shared.Model
{
    public class NoteFileModel : EntityBase
    {
        public string Blob { get; set; } = "rafastore";
        public string FileName { get; set; } = string.Empty;
        public int? CustomerModelId { get; set; }
        public CustomerModel? CustomerModel { get; set; }

    }
}
