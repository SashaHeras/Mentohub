using Mentohub.Core.Repositories.Interfaces;
using Mentohub.Domain.Data.DTO;
using Microsoft.AspNetCore.Identity;
using Mentohub.Domain.Data.Entities;
using Mentohub.Core.AllExceptions;
using Microsoft.EntityFrameworkCore;

namespace Mentohub.Core.Repositories.Repositories
{
    public class CRUD_UserRepository : ICRUD_UserRepository
    {
        #pragma warning disable 8603
        private readonly UserManager<CurrentUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AllException _exception;
       

        public CRUD_UserRepository(UserManager<CurrentUser> userManager, AllException exception,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _exception = exception;
        }

        /// <summary>
        /// пошук користувача по name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<CurrentUser> FindCurrentUserByName(string name)
        {
            CurrentUser user = await _userManager.FindByNameAsync(name);
            if (user == null)
            {
                return _exception.NotFoundObject("Customer is not found");
            }

            return user;
        }

        public CurrentUser FindByID(string ID)
        {
            return _userManager.Users.Where(x => x.Id == ID)
                               .Include(x => x.Comments)
                               .Include(x => x.Courses)
                               .Include(x => x.CourseViews)
                               .Include(x => x.Tags)
                               .FirstOrDefault();
        }

        /// <summary>
        /// пошук користувача по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CurrentUser> FindCurrentUserById(string id)
        {
            return await _userManager.FindByIdAsync(id); 
        }
        /// <summary>
        /// оновлення інформації про аватарку користувача
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="avatarUrl"></param>
        /// <returns></returns>
        public async Task UpdateAvatarUrl(string userId, string avatarUrl)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                user.Image = avatarUrl;
                await _userManager.UpdateAsync(user);
            }
        }

        /// <summary>
        /// отримання переліку користувачів по певній ролі
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<List<CurrentUser>> GetAllUsers(string roleName)
        {
            return (List<CurrentUser>)await _userManager.GetUsersInRoleAsync(roleName);
        }
        /// <summary>
        /// пошук користувача по email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<CurrentUser> FindCurrentUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }


        /// <summary>
        /// повертає перелік ролей користувача
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<List<string>> GetUserRoles(CurrentUser user)
        {
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                return roles.ToList();
            }

            return new List<string>();
        }
        /// <summary>
        /// отримання ролі по roleId
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<IdentityRole> GetRoleById(string roleId)
        {
            return await _roleManager.FindByIdAsync(roleId);
        }
        /// <summary>
        /// отримання всіх ролей
        /// </summary>
        /// <returns></returns>
        public Task<List<IdentityRole>> GetAllRoles()
        {
            return Task.FromResult(_roleManager.Roles.ToList());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> Login(LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user!=null)
            return await _userManager.CheckPasswordAsync(user, model.Password);
            return false;
        }
    }
}