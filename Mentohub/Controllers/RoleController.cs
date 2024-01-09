using Mentohub.Core.Repositories.Interfaces;
using Mentohub.Core.Repositories.Repositories;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Core.Services.Services;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Mentohub.Controllers
{
    [EnableCors("MentoPolicy")]
    [Route("api/role")]
    [ApiController]
    [SwaggerTag("RoleController")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<CurrentUser> _usermanager;
        private readonly IUserService _userService;
        private ICRUD_UserRepository _cRUD;
        public RoleController(ICRUD_UserRepository cRUD,RoleManager<IdentityRole> roleManager, IUserService userService, UserManager<CurrentUser> usermanager)
        {
            _roleManager = roleManager;
            _usermanager = usermanager;
            _userService = userService;
            _cRUD= cRUD;
        }
        [Route("create")]
        public IActionResult Create() => View();

        /// <summary>
        /// Method for role creation
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(string name)
        {
            try
            {
                var result = await _userService.CreateRole(name);
                if (result)
                {
                    Json(result).StatusCode = 204;
                    return Json($"Role {name} was created");
                }
                else
                {
                    var res = Json($"Error during the creating role {name}");
                    res.StatusCode = 400;
                    return res;
                }
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    message = $"Role {name} was not created",
                    error = ex.Message // інформація про помилку
                };

                return new JsonResult(errorResponse)
                {
                    StatusCode = 500 // код статусу, що вказує на помилку
                };
            }
        }

        /// <summary>
        /// Get list of all roles
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("listRoles")]
        public async Task<IActionResult> ListOfRoles()
        {
            return new JsonResult(await _cRUD.GetAllRoles());
        }
           
        /// <summary>
        /// delete role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("deletRole")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _userService.DeleteRole(id);
                if (result)
                {
                    Json(result).StatusCode = 204;
                    return Json("Role is deleted");
                }
                    
                else
                {
                    Json(result).StatusCode = 400;
                    return Json("Role is not deleted");
                }
                    
            }
            catch(Exception ex)
            {
                var errorResponse = new
                {
                    message = "Not found role by this roleId",
                    error = ex.Message // інформація про помилку
                };
                return new JsonResult(errorResponse)
                {
                    StatusCode = 500 // код статусу, що вказує на помилку
                };
            }            
        }

        [HttpGet]
        [Route("edit")]
        public async Task<IActionResult> Edit(string userId)
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
        

    }
}
