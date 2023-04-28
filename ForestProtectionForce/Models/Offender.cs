namespace ForestProtectionForce.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Offender
    {
       
        public int Id { get; set; }

       
        public string? CaseId { get; set; }

       
        public string? Name { get; set; }

        [Column("Surname_Alias")]
     
        public string? SurnameAlias { get; set; }

        [Column("Father_Husband_Name_Alias")]
       
        public string? FatherHusbandNameAlias { get; set; }

       
       
        public string? Caste { get; set; }

        [Column("Trade_Profession")]
        
        public string? TradeProfession { get; set; }

        [Column("Date_of_Photography")]
        public DateTime? DateOfPhotography { get; set; }

        public byte[]? Photo { get; set; }

        public string? Photo_Url { get; set; }

        [Column("Date_of_Birth")]
        public DateTime? DateOfBirth { get; set; }

        public int? Age { get; set; }

       
        public string? Sex { get; set; }

     
        public string? Citizenship { get; set; }

      
        public string? Email { get; set; }

        [Column("Passport_No")]
     
        public string? PassportNo { get; set; }

        [Column("Telephone_Mobile_No")]
  
        public string? TelephoneMobileNo { get; set; }

        [Column("Aadhaar_No")]
      
        public string? AadhaarNo { get; set; }

        [Column("Bank_Account_No")]
      
        public string? BankAccountNo { get; set; }

        [Column("House_No")]
    
        public string? HouseNo { get; set; }

      
        public string? Village { get; set; }

      
        public string? City { get; set; }

     
        public string? Street { get; set; }

        [Column("PinCode")]
      
        public string? PinCode { get; set; }

        [Column("policestation")]
      
        public string? PoliceStation { get; set; }

        [Column("districtId")]
        public int? DistrictId { get; set; }

        [Column("updatedBy")]
       
        public string? UpdatedBy { get; set; }

        [Column("updatedOn")]
        public DateTime UpdatedOn { get; set; } = DateTime.Now;

        [Column("isActive")]
        public bool IsActive { get; set; } = true;
    }

}
