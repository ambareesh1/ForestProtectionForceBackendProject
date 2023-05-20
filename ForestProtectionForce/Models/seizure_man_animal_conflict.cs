namespace ForestProtectionForce.Models
{
    public class seizure_man_animal_conflict
    {
        public int Id { get; set; }
        public int SNo { get; set; }
        public string? NameOfGammaUnit { get; set; }
        public string? PlaceOfOccurrence { get; set; }
        public long NoOfFPFPersonnelDeployed { get; set; }
        public string? Remarks { get; set; }
        public int? ProvinceId { get; set; }
        public int ? DistrictId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public bool IsActive { get; set; }
    }
}
