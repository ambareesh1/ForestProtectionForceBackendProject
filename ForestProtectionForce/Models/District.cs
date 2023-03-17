namespace ForestProtectionForce.Models
{
    public partial class District
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int CircleId { get; set; }
        public bool IsActive { get; set; } = true;
        public Circle? Circle { get; set; }
    }
}
