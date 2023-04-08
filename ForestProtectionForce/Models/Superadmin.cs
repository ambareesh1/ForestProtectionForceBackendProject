namespace ForestProtectionForce.Models
{
    public class Superadmin
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public string? AlternativeEmail { get; set; }
        public string? Division { get; set; }
        public string? Ipaddress { get; set; }
        public DateTime LastupdatedOn { get; set; }
        public bool Isactive { get; set; }
        public int Otp { get; set; }
    }

}
