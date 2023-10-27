using Mentohub.Core.Repositories.Repositories;
using Mentohub.Core.Services.Services;
using Mentohub.Domain.Data.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Mentohub.Controllers
{
    [Route("api/user")]
    [ApiController]
    [SwaggerTag("UserController")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        private readonly UserService _userService;
        public UserController(ILogger<UserController> logger, UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }
        [HttpDelete]
        [Route("deleteUser")]
        [SwaggerOperation(Summary = "Delete a user by ID")]
        [SwaggerResponse(204, "User deleted successfully")]
        [SwaggerResponse(404, "User not found")]
        public JsonResult DeleteUser([FromRoute] string userId)
        {
            // Логіка видалення користувача
            var deletedUser = _userService.DeleteUser(userId);

            if (deletedUser != null)
            {
                return new JsonResult(null)
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
        [HttpPut]
        [Route("updateUser")]
        [SwaggerOperation(Summary = "Update user information by ID")]
        [SwaggerResponse(200, "User updated successfully")]
        [SwaggerResponse(400, "Invalid input")]
        [SwaggerResponse(404, "User not found")]
        public JsonResult UpdateUser([FromRoute] string userId, IFormFile avatarFile, [FromBody] EditUserDTO user)
        {
            // Логіка оновлення інформації про користувача
            var updatedUser = _userService.UpdateUser(avatarFile, user);

            if (updatedUser != null)
            {
                return new JsonResult(updatedUser)
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
        [HttpGet]
        [Route("getUser")]
        [SwaggerOperation(Summary = "Get user information by ID")]
        [SwaggerResponse(200, "User information retrieved successfully")]
        [SwaggerResponse(404, "User not found")]
        public JsonResult GetUser([FromRoute] string userId)
        {
            // Логіка отримання інформації про користувача
            var user = _userService.GetProfile(userId);

            if (user != null)
            {
                return new JsonResult(user);
            }
            else
            {
                return new JsonResult("User not found")
                {
                    StatusCode = 404 // Код статусу "Not Found"
                };
            }
        }
    }
}
