namespace ForestProtectionForce.Models
{
    public partial class Circle 
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int ProvinceId { get; set; }
        public bool IsActive { get; set; } = true;
        public Province? Province { get; set; }
    }
}
