namespace RafaStore.Shared.Model
{
    public class CustomerModel : EntityBase
    {
        public string Name { get; set; } = string.Empty;    
        public string CpfOrCnpj { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public IList<NoteFileModel>? File { get; set; }
    }
}
