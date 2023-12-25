using Azure.Core;
using Mentohub.Core.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MimeKit;

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
        
    }
}
