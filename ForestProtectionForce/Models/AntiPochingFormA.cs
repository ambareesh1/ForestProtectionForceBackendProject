namespace ForestProtectionForce.Models
{
    public class AntiPochingFormA
    {
        public int Id { get; set; }
        public int Sno { get; set; }
        public string? Activity { get; set; }
        public int Unit { get; set; }
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
