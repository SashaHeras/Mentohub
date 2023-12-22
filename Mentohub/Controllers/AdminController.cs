using Mentohub.Core.Services;
using Mentohub.Core.Services.Services;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Swashbuckle.AspNetCore.Annotations;

namespace Mentohub.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [SwaggerTag("AdminController")]
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        
        private readonly UserService _userService;
        private readonly ILogger<AdminController> _logger;
        private readonly EmailSender _emailSender;
        //private readonly IHubContext<SignalRHub> _signalRHub;
        public AdminController(RoleManager<IdentityRole> roleManager,
             UserService userService,
            ILogger<AdminController> logger, EmailSender emailSender 
           /* IHubContext<SignalRHub> signalRHub*/)
        {
            _roleManager = roleManager;
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
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("userslist")]
        public IActionResult UserList() => Json(_userService.GetAllUsers());
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("deleteUserByName")]
        [SwaggerOperation(Summary = "Delete a user by Name")]
        public async Task<IActionResult> DeleteUser([FromForm] string userName)
        {
            // Логіка видалення користувача
            try
            {
                var deletedUser = await _userService.DeleteUserByName(userName);
                if (deletedUser)
                {
                    _logger.LogInformation("User deleted successfully.");
                    return new JsonResult("User deleted successfully ")
                    {
                        StatusCode = 204 // Код статусу "No Content"
                    };
                }
                else
                {
                    return new JsonResult("User not found")
                    {
                        StatusCode = 404 // Код статусу "Not Found"
                    };
                }
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    message = "Error when deleting a user",
                    error = ex.Message // інформація про помилку
                };
                return new JsonResult(errorResponse)
                {
                    StatusCode = 500 // код статусу, що вказує на помилку
                };
            }
        }
        [HttpGet]
        [Route("getUser")]
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
        [SwaggerOperation(Summary = "Edit the list of user's roles")]
        public async Task<JsonResult> EditUserRoles([FromForm] string userId)
        {
            // получаем пользователя
            CurrentUser user = await _userService.GetCurrentUser(userId);
            var model = _userService.EditUserRoles(user);
            if (model == null) { return Json("Not found"); }
            return Json(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("listOfUsersByRoleName")]
        public async Task<IActionResult> GetUsersByRoleName(string roleName)
        {
            try
            {
                if(!string.IsNullOrEmpty(roleName)) 
            {
                var result =await _userService.GetAllUsersByRoleName(roleName);
                return new JsonResult(result)
                {
                    StatusCode = 200
                };
            }

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
            return new JsonResult("Unknown error");

        }
        [HttpPost]
        [Route("sendEmail")]
        public async Task<IActionResult> SendEmail([FromForm]string email, [FromForm] string subject, [FromForm] string htmlmessage)
        {
            if(!string.IsNullOrEmpty(email)&&!string.IsNullOrEmpty(htmlmessage)) 
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
