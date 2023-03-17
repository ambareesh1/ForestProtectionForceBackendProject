namespace ForestProtectionForce.Models
{
    public partial class Compartment
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int DivisionId { get; set; }
        public bool IsActive { get; set; } = true;
        public Division? Division { get; set; }
    }
}
