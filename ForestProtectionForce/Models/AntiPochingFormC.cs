namespace ForestProtectionForce.Models
{
    public class AntiPochingFormC
    {
        public int Id { get; set; }
        public int Sno { get; set; }
        public string? FIRRegistered { get; set; }
        public string? NoDate { get; set; }
        public string? Details { get; set; }
        public int ProvinceId { get; set; }
        public int DistrictId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime DateOfInsertion { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
