using Mentohub.Core.Services.Interfaces;
using Mentohub.Core.Services.Services;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Mentohub.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [SwaggerTag("AdminController")]
    public class AdminController : Controller
    {
 
        private readonly UserService _userService;
        private readonly ILogger<AdminController> _logger;
        private readonly EmailSender _emailSender;
        //private readonly IHubContext<SignalRHub> _signalRHub;
        public AdminController(UserService userService,
            ILogger<AdminController> logger, EmailSender emailSender
           /* IHubContext<SignalRHub> signalRHub*/)
        {
            _userService = userService;
            _logger = logger;
            _emailSender = emailSender;
            //_signalRHub = signalRHub;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Get list of users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUserList")]
        public IActionResult GetUserList() => Json(_userService.GetAllUsers());

        [HttpGet]
        [Route("GetUser")]
        [SwaggerOperation(Summary = "Get information about user")]
        public async Task<IActionResult> GetUser(string userName)
        {
            try
            {
                var user =await _userService.GetUser(userName);
                if (user != null)
                {
                    return new JsonResult(user)
                    {
                        StatusCode = 200
                    };
                }
            }
            catch(Exception ex) 
            {
                var errorResponse = new
                {
                    message = "User not found",
                    error = ex.Message // інформація про помилку
                };
                return new JsonResult(errorResponse)
                {
                    StatusCode = 500 // код статусу, що вказує на помилку
                };
            }

            return new JsonResult("Unknown error");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">user's Id</param>
        /// <returns></returns>
        [HttpPost]
        [Route("editUserRoles")]
        [SwaggerOperation(Summary ="Edit the list of user's roles")]
        public async Task<IActionResult> EditUserRoles([FromForm]string userId)
        {
            var model = await _userService.GetChangeRoleDTO(userId);
            if(model == null) { return NoContent(); }
               return new JsonResult(model)
               {
                   StatusCode = 200
               };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ListOfUsersByRoleName")]
        public async Task<IActionResult> GetUsersByRoleName(string roleName)
        {
            try
            {
                var result = await _userService.GetAllUsersByRoleName(roleName);
                return new JsonResult(result)
                {
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    message = "Not found users by this roleName",
                    error = ex.Message // інформація про помилку
                };

                return new JsonResult(errorResponse)
                {
                    StatusCode = 500 // код статусу, що вказує на помилку
                };
            }
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
