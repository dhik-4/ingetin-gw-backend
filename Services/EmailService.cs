using IngetinGwAPI.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;

namespace IngetinGwAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config) 
        {
            _config = config;
        }

        public async Task SendAsync(string to, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("noreply@yourapp.com"));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart("plain") { Text = body };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, false);
            await smtp.AuthenticateAsync("your-email@gmail.com", "app-password");
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public async Task SendMailpitAsync(string to, string subject, string body)
        {
            try
            {
                var mailPit = _config.GetSection("Mailpit");
                var _mode = mailPit["Mode"];
                var _host = mailPit["Host"];
                var _Port = int.Parse(mailPit["Port"]);
                var _From = mailPit["From"];

                var message = new MimeMessage();
                message.From.Add(MailboxAddress.Parse(_From));
                message.To.Add(MailboxAddress.Parse(to));
                message.Subject = subject;

                message.Body = new TextPart("plain")
                {
                    Text = body
                };

                using var smtp = new SmtpClient();

                // Mailpit SMTP (no auth, no SSL)
                await smtp.ConnectAsync(_host, _Port, false);
                await smtp.SendAsync(message);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
