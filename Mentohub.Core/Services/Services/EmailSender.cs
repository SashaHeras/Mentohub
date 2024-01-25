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
        //private readonly IQueueService _queueService;

        public EmailSender(IConfiguration configuration/*, IQueueService queueService*/)
        {
            _configuration = configuration;
            //_queueService = queueService;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Mentohub", "mentochub@ukr.net"));
            message.To.Add(new MailboxAddress("Dear customer", email));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlMessage
            };

            message.Body = bodyBuilder.ToMessageBody();
            //await _queueService.SendMessageAsync(JsonConvert.SerializeObject(message));

            try
            {
                using (var client = new SmtpClient())
                {
                    var smtpServer = "smtp.ukr.net";
                    var smtpPort = 465;
                    var smtpUsername = "mentochub@ukr.net";
                    var smtpPassword = "N6740I3aWaYPfABF";
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    await client.ConnectAsync(smtpServer, smtpPort, true);
                    //client.CheckCertificateRevocation = false;
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
