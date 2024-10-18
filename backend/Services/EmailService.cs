using System.Net;
using System.Net.Mail;
using backend.Helpers;
using backend.Interfaces;

namespace backend.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public Task SendEmailAsync(SendEmailObject emailObject)
        {
            string MailServer = _config["EmailSettings:MailServer"];
            string FromEmail = _config["EmailSettings:FromEmail"];
            string Password = _config["EmailSettings:Password"];
            string SenderName = _config["EmailSettings:SenderName"];
            int Port = int.Parse(_config["EmailSettings:MailPort"]);

            var client = new SmtpClient(MailServer, Port)
            {
                Credentials = new NetworkCredential(FromEmail, Password),
                EnableSsl = true,
            };

            MailMessage mailMessage = new()
            {
                From = new MailAddress(FromEmail, SenderName),
                Subject = emailObject.Subject,
                Body = emailObject.Body,
                IsBodyHtml = emailObject.IsBodyHTML
            };
            mailMessage.To.Add(emailObject.ToEmail);

            return client.SendMailAsync(mailMessage);
        }
    }
}