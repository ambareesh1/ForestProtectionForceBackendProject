namespace ForestProtectionForce.Models
{
    public class HistorySheet
    {
        public int Id { get; set; }
        public string? Offender { get; set; }
        public string? AadharCard { get; set; }
        public string? IdentifierOfficerName { get; set; }
        public string? UsualFieldOfOperation { get; set; }
        public string? ModusOperandi { get; set; }
        public string? PreviousHistory { get; set; }
        public string? PlaceOfHabitualResort { get; set; }
        public string? NameOfRelative { get; set; }
        public string? Relationship { get; set; }
        public string? Criminality { get; set; }
        public string? NameOfAssociate { get; set; }
        public string? Parentage { get; set; }
        public string? Address { get; set; }
        public int? District { get; set; }
        public int? Province { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
