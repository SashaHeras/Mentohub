﻿using Mentohub.Core.AllExceptions;
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
        private readonly AllException _exception;
        private readonly UserService _userService;
        private readonly CRUD_UserRepository _cRUD;
        public UserController(ILogger<UserController> logger, UserService userService,
            AllException exception, CRUD_UserRepository cRUD_UserRepository)
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
        /// 
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
        public async Task<IActionResult> UpdateUser([FromForm] string userId,
            [FromForm] EditUserDTO userDTO)
        {
            try
            {               
                // Логіка оновлення інформації про користувача
                var updatedUser =await _userService.UpdateUser(userId, userDTO);

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
        [Route("upload avatar")]
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
       /// 
       /// </summary>
       /// <param name="id"></param>
       /// <param name="roleName"></param>
       /// <returns></returns>
        [HttpPost]
        [Route("addRole")]
        [SwaggerOperation(Summary ="add role to user`s roles")]
        public async Task<JsonResult> AddUserRoles([FromForm]string id,
            [FromForm]string roleName)
        {
            try
            {
                var user =await _userService.GetCurrentUser(id);
                if(user != null && await _userService.AddRoleToUserListRoles(user,roleName))
                {

                    var UserDTO = await _userService.GetUser(user.UserName);
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
    }
}
