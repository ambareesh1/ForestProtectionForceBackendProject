namespace ForestProtectionForce.Models
{
    public class Seizures_Form_A
    {
        public int id { get; set; }
        public string? serialNo { get; set; }
        public string? name { get; set; }
        public string? ob_independent { get; set; }
        public string? during_month_independent { get; set; }
        public string? total_independent { get; set; }
        public string? ob_joint { get; set; }
        public string? during_month_joint { get; set; }
        public string? total_joint { get; set; }
        public int provinceId { get; set; }
        public int districtId { get; set; }
        public int month { get; set; }
        public int? year { get; set; }
        public DateTime DateOfInsertion { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastUpdatedOn { get; set; }
    }
}
