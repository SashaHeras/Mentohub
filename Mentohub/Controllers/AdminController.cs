using Mentohub.Core.Services.Services;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Mentohub.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [SwaggerTag("AdminController")]
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<CurrentUser> _usermanager;
        private readonly UserService _userService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(RoleManager<IdentityRole> roleManager,
            UserManager<CurrentUser> usermanager, UserService userService,
            ILogger<AdminController> logger)
        {
            _roleManager = roleManager;
            _usermanager = usermanager;
            _userService = userService;
            _logger = logger;
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
        [Route("userlist")]
        public IActionResult UserList() => Json(_usermanager.Users.ToList());
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
        [SwaggerOperation(Summary ="Edit the list of user's roles")]
        public async Task<IActionResult> EditUserRoles([FromForm]string userId)
        {
            // получаем пользователя
            CurrentUser user = await _usermanager.FindByIdAsync(userId);
            if (user != null)
            {
                // получем список ролей пользователя
                var userRoles = await _usermanager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                ChangeRoleDTO model = new ChangeRoleDTO
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles,
                };
                return new JsonResult(model);
            }
            return NotFound();
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
    }
}
