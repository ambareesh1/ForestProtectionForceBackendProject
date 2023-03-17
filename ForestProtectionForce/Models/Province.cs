namespace ForestProtectionForce.Models
{
    public partial class Province
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
