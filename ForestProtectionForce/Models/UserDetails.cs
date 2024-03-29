﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ForestProtectionForce.Models
{
    public partial class UserDetails
    {
        public int Id { get; set; }
        public int UserType_Id { get; set; }

        public int ProvinceId { get; set; }
        public int CircleId { get; set; }
        public int DistrictId { get; set; }
        [NotMapped]
        public string? DistrictName { get; set; }
        public string? UserType_Name { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? First_Name { get; set; }
        public string? Last_Name { get; set; }
        public string? Email { get; set; }
        public string? Alternate_Email { get; set; }
        public string? Mobile { get; set; }
        public int Otp {get;set;}
        public string? Address { get; set; }
        public DateTime Created_On { get; set; }
        public DateTime Updated_On { get; set; }
        public bool ? IsActive { get; set; }

    }
}
