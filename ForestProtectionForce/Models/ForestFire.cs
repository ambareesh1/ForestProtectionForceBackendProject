namespace ForestProtectionForce.Models
{
    public class ForestFire
    {
        public int id { get; set; }
        public int sno { get; set; }
        public int province_id { get; set; }
        public int district_id { get; set; }
        public string? gamma_unit_name { get; set; }
        public int ob_total_cases { get; set; }
        public string? forest_division_name { get; set; }
        public string? fire_spot { get; set; }
        public decimal forest_damage_area { get; set; }
        public string? forest_crop_damaged { get; set; }
        public DateTime fire_datetime { get; set; }
        public string? fpf_personnel_name { get; set; }
        public int total_fire_cases { get; set; }
        public int month { get; set; }
        public int year { get; set; }
        public DateTime date_of_insertion { get; set; }
        public bool is_active { get; set; }
        public DateTime last_updated_on { get; set; }
    }
}
