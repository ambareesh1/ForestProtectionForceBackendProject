namespace ForestProtectionForce.Models
{
    public partial class Division
    {

        public int Id { get; set; }
        public string? Name { get; set; }
        public int DistrictId { get; set; }
        public int CircleId { get; set; }
        public int ProvinceId { get; set; }
        public bool IsActive { get; set; } = true;
        public District? District { get; set; }
    }
}
