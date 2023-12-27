using Mentohub.Core.AllExceptions;
using Mentohub.Core.Repositories.Interfaces;
using Mentohub.Core.Repositories.Repositories;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Core.Services.Services;
using Mentohub.Domain.Data.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace Mentohub.Controllers
{
    [Route("api/user")]
    [ApiController]
    [SwaggerTag("UserController")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly AllException _exception;
        private readonly IUserService _userService;
        private readonly ICRUD_UserRepository _cRUD;
        public UserController(ILogger<UserController> logger, IUserService userService,
            AllException exception, ICRUD_UserRepository cRUD_UserRepository)
        {
            _logger = logger;
            _userService = userService;
            _exception = exception;
            _cRUD = cRUD_UserRepository;
        }
        
        [HttpDelete]
        [Route("deleteUser")]
        [SwaggerOperation(Summary = "Delete a user by ID")]
        
        public async Task<IActionResult> DeleteUser([FromForm] string userId)
        {
            // Логіка видалення користувача
            try 
            { 
                var deletedUser =await _userService.DeleteUser(userId);
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
            }catch(Exception ex)
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
        /// <summary>
        /// отримання профіля користувача
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getUserProfile")]
        [SwaggerOperation(Summary ="Get user profile")]
        public async Task<IActionResult>GetUserProfile(string id)
        {
            try
            {
                var profile = await _userService.GetProfile(id);
                if (profile != null)
                    return new JsonResult(profile)
                    {
                        StatusCode = 200
                    };
                else
                {
                    return _exception.NotFoundObjectResult("Not Found");

                }

            }
            catch(Exception ex) 
            {
                var errorResponse = new
                {
                    message = "Error when getting a user",
                    error = ex.Message // інформація про помилку
                };
                return new JsonResult(errorResponse)
                {
                    StatusCode = 500 // код статусу, що вказує на помилку
                };
            }
            
        }

        /// <summary>
        /// оновлення інформації про користувача
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="avatarFile"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("updateUser")]
        [SwaggerOperation(Summary = "Update user information by ID")]
        [SwaggerResponse(200, "User updated successfully")]
        [SwaggerResponse(400, "Invalid input")]
        [SwaggerResponse(404, "User not found")]
        public async Task<IActionResult> UpdateUser([FromForm] UserDTO userDTO)
        {
            try
            {               
                // Логіка оновлення інформації про користувача
                var updatedUser = await _userService.UpdateUser(userDTO);
                if (updatedUser)
                {
                    return new JsonResult("User profile is updated")
                    {
                        StatusCode = 200 //профіль користувача успішно оновлено
                    };
                }
                else
                {
                    return new JsonResult("User not found")
                    {
                        StatusCode = 404 // Код статусу "Not Found", користувача не знайдено
                    };
                }
            }
            catch(Exception ex)
            {
                var errorResponse = new
                {
                    message = "Error when updating a user",
                    error = ex.Message // інформація про помилку
                };
                return new JsonResult(errorResponse)
                {
                    StatusCode = 500 // код статусу, що вказує на помилку
                };
            }
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="avatar"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("uploadAvatar")]
        [SwaggerOperation(Summary ="Download user's avatar")]
        public async Task<JsonResult> UploadAvatar(IFormFile avatar,string id)
        {
            try
            { 
                var avatarUrl =await _userService.UploadAvatar(avatar, id);
                if(avatarUrl != null)
                {
                      return new JsonResult(avatarUrl)
                      {
                           StatusCode = 200
                      };
                }
                return new JsonResult("Not file for download")
                {
                    StatusCode = 404
                };

            }
           catch (Exception ex)
            {
                var errorResponse = new
                {
                    message = "Error when was a download of avatar",
                    error = ex.Message // інформація про помилку
                };
                return new JsonResult(errorResponse)
                {
                    StatusCode = 500 // код статусу, що вказує на помилку
                };
            }
        }
       /// <summary>
       /// додавання ролі користувачу
       /// </summary>
       /// <param name="id"></param>
       /// <param name="roleName"></param>
       /// <returns></returns>
        [HttpPost]
        [Route("addRole")]
        [SwaggerOperation(Summary ="add role to user`s roles")]
        public async Task<JsonResult> AddUserRoles([FromForm]string userId,
            [FromForm]string roleId)
        {            
            var user=await _cRUD.FindCurrentUserById(userId);
            try
            {              
                if( await _userService.AddRoleToUserListRoles(userId, roleId))
                {
                    var UserDTO = await _userService.GetProfile(userId);
                    return new JsonResult(UserDTO)
                    {
                        StatusCode = 200
                    };
                }
                return new JsonResult("Add role is failed")
                {
                    StatusCode = 500
                };
            }
            catch(Exception ex)
            {
                var errorResponse = new
                {
                    message = "Error when add user`s role",
                    error = ex.Message // інформація про помилку
                };
                return new JsonResult(errorResponse)
                {
                    StatusCode = 500 // код статусу, що вказує на помилку
                };
            }
           
        }
        /// <summary>
        /// отримання аватарки
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getAvatar")]
        public async Task< IActionResult> GetAvatar(string id)
        {
                // Отримайте URL аватарки користувача з сервісу
                string avatarUrl =await _userService.GetAvatarUrl(id);

                if (!string.IsNullOrEmpty(avatarUrl))
                {
                    return new JsonResult(new { success = true, avatarUrl });
                }
 
            // Якщо аватарка не знайдена або виникла помилка
            return new JsonResult(new { success = false, message = "Error when getting an avatar" });
        }
    }
}


