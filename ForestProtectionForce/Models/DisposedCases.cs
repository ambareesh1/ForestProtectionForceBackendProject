namespace ForestProtectionForce.Models
{
    public class DisposedCases
    {
            public int Id { get; set; }
            public string? CaseId { get; set; }
            public DateTime DateOfFillingComplaints { get; set; }
            public string? NameOfCourt { get; set; }
            public string? ToolsAmplements { get; set; }
            public int NoOfAccused { get; set; }
            public int FinalDisposalOfCase { get; set; }
            public string? IfConvictedPunishment { get; set; }
            public string? Attachment { get; set; }
            public string? SectionsOfLaws { get; set; }
            public string? DetailsOfSeizuresRecory { get; set; }
            public string? NameOfAccused { get; set; }
            public decimal Amount { get; set; }
            public string? AppealFilledIfAny { get; set; }
            public int District { get; set; }
            public int Province { get; set; }
            public DateTime LastUpdatedDate { get; set; }
            public string? UpdatedBy { get; set; }
            public bool IsActive { get; set; }
        

    }
}
