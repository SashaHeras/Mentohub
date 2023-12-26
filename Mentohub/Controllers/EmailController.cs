using Azure.Core;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using Mentohub.Core.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MimeKit;
using System.Net.Mail;

namespace Mentohub.Controllers
{
    public class EmailController : Controller
    {
        private readonly IHubContext<SignalRHub> _signalRHub;
        private readonly IEmailSender _emailSender;

        public EmailController(IHubContext<SignalRHub> signalRHub, IEmailSender emailSender)
        {
            _signalRHub=signalRHub;
            _emailSender = emailSender;
        }

        [HttpPost]
        [Route("sendEmail")]
        public async Task<IActionResult> SendEmail([FromForm] string email, [FromForm] string subject, [FromForm] string htmlmessage)
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(htmlmessage))
            {
                await _emailSender.SendEmailAsync(email, subject, htmlmessage);
                //await _signalRHub.ReceiveEmail(email);
                return new JsonResult("Email is sent successfully")
                {
                    StatusCode = 200
                };
            }
            return new JsonResult("An error occurred while trying to send an email");
        }

        /// <summary>
        /// Test send email
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("testEmail")]
        public IActionResult TestEmail(string message)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Mentohub", "mentochub@ukr.net"));
            email.To.Add(new MailboxAddress("Mentohub", "mentochub@ukr.net"));
            email.Subject = "Test";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect("smtp.ukr.net", 465, MailKit.Security.SecureSocketOptions.Auto);
            try
            {
                smtp.Authenticate("mentochub@ukr.net", "fRJvT7s!B6aYEeM");
                smtp.Send(email);
            }
            catch(Exception ex)
            {
                throw ex;
            }

            smtp.Disconnect(true);

            return Ok();
        }
    }
}
