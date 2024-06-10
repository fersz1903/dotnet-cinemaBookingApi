using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using MimeKit;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace UserAuthApi.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            UserCredential credential;

            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { GmailService.Scope.GmailSend },
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true));
            }

            // Create Gmail API service.
            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "CinemaBookingApp",
            });

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_configuration["EmailSettings:SenderName"], _configuration["EmailSettings:SenderEmail"]));
            emailMessage.To.Add(new MailboxAddress(toEmail, "ReceiverName"));
            emailMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = message
            };

            emailMessage.Body = bodyBuilder.ToMessageBody();

            using (var memoryStream = new MemoryStream())
            {
                await emailMessage.WriteToAsync(memoryStream);
                var rawMessage = Convert.ToBase64String(memoryStream.ToArray())
                    .Replace('+', '-')
                    .Replace('/', '_')
                    .Replace("=", "");

                var gmailMessage = new Message
                {
                    Raw = rawMessage
                };

                await service.Users.Messages.Send(gmailMessage, "me").ExecuteAsync();
            }
        }



        public async Task SendConfirmationEmailAsync(string toEmail, string token)
        {
            UserCredential credential;

            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    new[] { GmailService.Scope.GmailSend },
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true));
            }

            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "GmailConfirmationExample",
            });

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_configuration["EmailSettings:SenderName"], _configuration["EmailSettings:SenderEmail"]));
            emailMessage.To.Add(new MailboxAddress("", toEmail));
            emailMessage.Subject = "Confirm your email";

            var confirmationUrl = _configuration["EmailSettings:ConfirmationUrl"] + token;
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $"<a href='{confirmationUrl}'>Click here to confirm your email</a>"
            };

            emailMessage.Body = bodyBuilder.ToMessageBody();

            using (var memoryStream = new MemoryStream())
            {
                await emailMessage.WriteToAsync(memoryStream);
                var rawMessage = Convert.ToBase64String(memoryStream.ToArray())
                    .Replace('+', '-')
                    .Replace('/', '_')
                    .Replace("=", "");

                var gmailMessage = new Message
                {
                    Raw = rawMessage
                };

                await service.Users.Messages.Send(gmailMessage, "me").ExecuteAsync();
            }
        }
    }
}
