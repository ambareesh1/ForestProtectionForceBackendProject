using System.Reflection.Metadata;

namespace ForestProtectionForce.Models
{
    public class Images
    {
        public int id { get; set; }
        public byte[]? data { get; set; }
        public DateTime updatedOn { get; set; }
        public bool isActive { get; set; }

    }
}
