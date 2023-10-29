using Mentohub.Core.Services.Services;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Mentohub.Controllers
{
    [Route("api/role")]
    [ApiController]
    [SwaggerTag("RoleController")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<CurrentUser> _usermanager;
        private readonly UserService _userService;
        public RoleController(RoleManager<IdentityRole> roleManager, UserService userService, UserManager<CurrentUser> usermanager)
        {
            _roleManager = roleManager;
            _usermanager = usermanager;
            _userService = userService;

        }
        [Route("create")]
        public IActionResult Create() => View();
        [HttpPost]

        public async Task<IActionResult> Create(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(name);
        }

        [Route("index")]
        public IActionResult Index() => View(_roleManager.Roles.ToList());
        [HttpGet]
        [Route("listRoles")]
        public IActionResult ListOfRoles() => Json(_roleManager.Roles.ToList());
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Route("userlist")]
        public IActionResult UserList() => Json(_usermanager.Users.ToList());

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
                return View(model);
            }

            return NotFound();
        }
        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> Edit(string userId, List<string> roles)
        {
            // получаем пользователя
            CurrentUser user = await _userService.GetUserById(userId);
            if (user != null)
            {
                // получем список ролей пользователя
                var userRoles = await _usermanager.GetRolesAsync(user);
                // получаем все роли
                var allRoles = _roleManager.Roles.ToList();
                // получаем список ролей, которые были добавлены
                var addedRoles = roles.Except(userRoles);
                // получаем роли, которые были удалены
                var removedRoles = userRoles.Except(roles);

                await _usermanager.AddToRolesAsync(user, addedRoles);

                await _usermanager.RemoveFromRolesAsync(user, removedRoles);

                return RedirectToAction("UserList");
            }

            return NotFound();
        }

    }
}
