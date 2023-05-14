namespace ForestProtectionForce.Models
{
    public class Gamma_unit_form_b
    {
        public int Id { get; set; }
        public int SerialNo { get; set; }
        public int ProvinceId { get; set; }
        public int DistrictId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string? Gamma_Unit { get; set; }
        public int Nakas_Laid { get; set; }
        public int Patrollings_Performed { get; set; }
        public int Jungle_Gashts_Performed { get; set; }
        public int JOP_Reports_Received { get; set; }
        public int Complaints_Received { get; set; }
        public int Complaints_Verified { get; set; }
        public int Requisitions_Made { get; set; }
        public int Requisitions_Attended { get; set; }
        public int No_Of_Fire_Fighting_Operations { get; set; }
        public DateTime DateOfInsertion { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastUpdatedOn { get; set; }
    }
}
