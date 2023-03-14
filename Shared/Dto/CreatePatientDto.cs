namespace RafaStore.Shared.Dto
{
    public class CreatePatientDto
    {
        public string Name { get; set; }
        public int? Age { get; set; }
        public string MotherName { get; set; }
        public DateTime? BirthDate { get; set; }
        public int HospitalId { get; set; }
        public bool Editing { get; set; } = false;
        public bool IsNew { get; set; } = false;
    }
}
