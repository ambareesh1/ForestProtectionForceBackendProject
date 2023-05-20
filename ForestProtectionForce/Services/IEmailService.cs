namespace ForestProtectionForce.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string htmlContent);
        bool IsDefaultPassword();
    }
}
