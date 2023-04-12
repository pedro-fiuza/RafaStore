namespace RafaStore.Shared.Model
{
    public class NoteFileModel : EntityBase
    {
        public string Blob { get; set; } = "rafastore";
        public string FileName { get; set; } = string.Empty;
        public decimal? ValorParcela { get; set; }
        public decimal? ValorTotal { get; set; }
        public int? NumeroDeParcelas { get; set; }
        public int? CustomerModelId { get; set; }
        public CustomerModel? CustomerModel { get; set; }

    }
}
