namespace ForestProtectionForce.Models
{
    public class ForestOffender
    {
        public int Id { get; set; }
        public int Sno { get; set; }
        public string? NameOfForestOffender { get; set; }
        public string? AreaOfOperations { get; set; }
        public int CasesRegistered { get; set; }
        public string? CasesStatus { get; set; }
        public string? ModusOperandi { get; set; }
        public string? ActiveDormant { get; set; }
        public int DistrictId { get; set; }
        public int ProvinceId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime DateOfInsertion { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
