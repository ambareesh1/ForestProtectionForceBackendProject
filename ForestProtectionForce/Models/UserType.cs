namespace ForestProtectionForce.Models
{
    public partial class UserType
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
