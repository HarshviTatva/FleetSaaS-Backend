using FleetSaaS.Application.Interfaces.IServices;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace FleetSaaS.Application.Services
{
    public class EmailService(IConfiguration _config):IEmailService
    {
        public async Task SendAsync(string to, string subject, string body)
        {
            var smtpSection = _config.GetSection("SmtpSettings");

            var host = smtpSection["Host"];
            var port = int.Parse(smtpSection["Port"]!);
            var password = smtpSection["Password"];
            var fromName = smtpSection["DisplayName"];
            var fromEmail = smtpSection["FromEmail"];

            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(fromName, fromEmail));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;

            message.Body = new TextPart("html")
            {
                Text = body
            };

            using var client = new SmtpClient();

            await client.ConnectAsync(host, port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(fromEmail, password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
