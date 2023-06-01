namespace ForestProtectionForce.Models
{
    public class Complaints_Registered
    {
            public int Id { get; set; }
            public int sno { get; set; }
            public string? ComplaintNo { get; set; }
            public DateTime DateTimeOfReceipt { get; set; }
            public string? SourceOfComplaint { get; set; }
            public string? BriefDescription { get; set; }
            public string? ComplaintArea { get; set; }
            public string? CognizanceUnderSection { get; set; }
            public string? ActionTaken { get; set; }
            public string? NameSignMunshiMoharir { get; set; }
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
