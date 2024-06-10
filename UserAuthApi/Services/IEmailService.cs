using System.Threading.Tasks;

namespace UserAuthApi.Services
{
    public interface IEmailService
    {
        Task SendConfirmationEmailAsync(string toEmail, string token);
        Task SendEmailAsync(string toEmail, string subject, string message);
    }
}