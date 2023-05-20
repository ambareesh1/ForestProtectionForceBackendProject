namespace ForestProtectionForce.Models
{
    public class EmailConfiguration
    {
        public string? SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string? SmtpUsername { get; set; }
        public string? SmtpPassword { get; set; }
        public string? FromAddress { get; set; }
        public string? FromName { get; set; }
        public bool? EnableSsl { get; set; }
        public bool? UseDefaultCredentials { get; set; }
        public bool isOtpRequired { get; set; }

    }
}
