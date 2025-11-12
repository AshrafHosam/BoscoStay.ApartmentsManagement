namespace Application.Contracts.Services
{
    public interface IEmailService
    {
        Task<bool> SendVerificationEmail(string email, string password);
        Task<bool> SendEmail(string fromEmail, string toEmail, string subject, string message);
    }
}
