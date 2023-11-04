using Mentohub.Core.Repositories.Interfaces;
using Mentohub.Domain.Data.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mentohub.Domain.Data.Entities;
using Mentohub.Core.AllExceptions;
using Microsoft.AspNetCore.Routing;
using Mentohub.Domain.Data.Entities.Interfaces;


namespace Mentohub.Core.Repositories.Repositories
{
    public class CRUD_UserRepository : ICRUD_UserRepository
    {
        private readonly UserManager<CurrentUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AllException _exciption;
        private readonly SignInManager<CurrentUser> _signInManager;
        private readonly ILogger<CRUD_UserRepository> _logger;
        
        public CRUD_UserRepository(UserManager<CurrentUser> userManager, AllException exciption,
            RoleManager<IdentityRole> roleManager, SignInManager<CurrentUser> signInManager,
            ILogger<CRUD_UserRepository> logger)
        {
            _userManager = userManager;
            _exciption = exciption;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _logger = logger;
            
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
                return _exciption.NotFoundObject("Customer is not found");
            }
            return user;
        }
        /// <summary>
        /// пошук користувача по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CurrentUser> FindCurrentUserById(string id)
        {
            CurrentUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return _exciption.NotFoundObject("Customer is not found");
            }
            return user;
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

        Task<ICollection> ICRUD_UserRepository.GetAllUsers(string roleName)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// повертає перелік ролей користувача
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<List<string>> GetUserRoles(CurrentUser user)
        {
            if(user!=null)
            {
                var roles=await _userManager.GetRolesAsync(user);
                return roles.ToList();
            }
           return new List<string>();
        }
    }
}
