using Mentohub.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Mentohub.Controllers
{
    public class EmailController : Controller
    {
        private readonly IHubContext<SignalRHub> _hubContext;
        public EmailController(IHubContext<SignalRHub> hubContext)
        {
            _hubContext = hubContext;
        }
    }
}
