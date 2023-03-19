namespace ForestProtectionForce.Models
{
    public class Baseline
    {
        public int Id { get; set; }
        public DateTime DateOfDetection { get; set; }
        public string? OfficerName { get; set; }
        public string? CrimeDetails { get; set; }
        public string? ToolsUsed { get; set; }
        public int CircleId { get; set; }
        public string? CircleName { get; set; }
        public string? ForestDivisionName { get; set; }
        public int ForestDivisionId { get; set; }
        public string? ForestRangeName { get; set; }
        public int ForestRangeId { get; set; }
        public int CompartmentId { get; set; }
        public string? CompartmentName { get; set; }
        public string? CaseNo { get; set; }
        public string? PoliceStation { get; set; }
        public string? FIRNo { get; set; }
        public DateTime CrimeDate { get; set; }
        public string? SectionOfLaw { get; set; }
        public int Quantity { get; set; }
        public float Weight { get; set; }
        public int NoOfAccused { get; set; }
        public string? NameOfAccused { get; set; }
        public string? SpeciesDetected { get; set; }
        public string? ItemDescription { get; set; }
        public string? Status { get; set; }
        public bool IsActive { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
