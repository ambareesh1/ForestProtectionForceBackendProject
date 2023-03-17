namespace ForestProtectionForce.Models
{
    public partial class Division
    {

        public int Id { get; set; }
        public string? Name { get; set; }
        public int DistrictId { get; set; }
        public bool IsActive { get; set; } = true;
        public District? District { get; set; }
    }
}
