﻿using Mentohub.Core.Services.Interfaces;
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
        
        private readonly IUserService _userService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IUserService userService,
            ILogger<AdminController> logger)
        {
            _userService = userService;
            _logger = logger;
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
    }
}
