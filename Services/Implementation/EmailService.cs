using Domin.Helper;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Repository.Interfaces;

namespace Repository.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailsettings;

        public EmailService(IOptions<MailSettings> mailsettings)
        {
            _mailsettings = mailsettings.Value;
        }

        public async Task SendEmail(string mailto, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_mailsettings.DisplayName, _mailsettings.Email));
            email.To.Add(MailboxAddress.Parse(mailto));
            email.Subject = subject;

            var builder = new BodyBuilder
            {
                HtmlBody = body
            };
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();

            // Accept all SSL certificates (use with caution in production)
            smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

            await smtp.ConnectAsync(_mailsettings.Host, 465, SecureSocketOptions.SslOnConnect);
            await smtp.AuthenticateAsync(_mailsettings.Email, _mailsettings.Password);
            await smtp.SendAsync(email);

            await smtp.DisconnectAsync(true);
        }
    }
}
