using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Traditiona_trend_on_rent.Services
{
    public class EmailService
    {
        private readonly string _fromEmail;
        private readonly string _password;

        public EmailService(IConfiguration configuration)
        {
            _fromEmail = configuration["EmailSettings:FromEmail"];
            _password = configuration["EmailSettings:Password"];
        }

        public void SendEmail(string toEmail, string subject, string body)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(_fromEmail, _password),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_fromEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = false,
            };
            mailMessage.To.Add(toEmail);

            smtpClient.Send(mailMessage);
        }
    }
}
