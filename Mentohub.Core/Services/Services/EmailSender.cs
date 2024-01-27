using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Mentohub.Core.Services.Interfaces;
using Newtonsoft.Json;
using System.Net.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using System.Net;

namespace Mentohub.Core.Services.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_configuration["Email:SmtpServer"], _configuration["Email:FromAddress"] ));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlMessage
            };

            message.Body = bodyBuilder.ToMessageBody();

            try
            {
                using (var client = new SmtpClient())
                {
                    var smtpServer = _configuration["Email:SmtpServer"];
                    var smtpPort = _configuration.GetValue<int>("Email:Port");
                    var smtpUsername = _configuration["Email:SmtpUsername"];
                    var smtpPassword = _configuration["Email:SmtpPassword"];
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    await client.ConnectAsync(smtpServer, smtpPort, true);
                    client.Authenticate(new NetworkCredential()
                    {
                        Password = smtpPassword,
                        UserName = smtpUsername
                    });
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
        } 
        
    }
}
