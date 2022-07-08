using System;
using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;

namespace EventPlanning.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration configuration)
        {
            _config=configuration;
        }
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var emailMessage = new MimeMessage();

                emailMessage.From.Add(new MailboxAddress(_config["EmailServiceMessage"], _config["EmailServiceSender"]));
                emailMessage.To.Add(new MailboxAddress("", email));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };

                using var client = new SmtpClient();
                await client.ConnectAsync(_config["SmtpClientHost"], Convert.ToInt32(_config["SmtpClientPort"]),
                    Convert.ToBoolean(_config["SmtpClientUseSsl"]));

                await client.AuthenticateAsync(_config["SmtpClientUserName"], _config["SmtpClientPassword"]);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
            catch
            {
                throw new Exception("Account has been created, but email cannot be sent. Account can be confirmed from SuperUser panel.");
            }
        }
    }
}
