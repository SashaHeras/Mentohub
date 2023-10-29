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
        /// редагування профілю користувача
        /// </summary>
        /// <param name="avatarFile"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>

        public async Task<IItem> Edit(IFormFile avatarFile, EditUserDTO model)
        {
            CurrentUser user = await FindCurrentUserById(model.Id);
            if (user != null)
            {
                user.Id = model.Id;
                user.Email = model.Email;
                user.UserName = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.AboutMe = model.AboutMe;
                //await UploadAvatar(avatarFile, model.Id);
                await _userManager.UpdateAsync(user);
                return (IItem)user;
            }
            return (IItem)model;
        }
        //отримання переліку всіх користувачів
        //public Task<ICollection>? GetAllUsers()
        //{
        //    return _userManager.Users as ICollection as Task<ICollection>;
        //}

        /// <summary>
        /// отримання форми для редагування профілю користувача
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EditUserDTO> GetUserProfile(string id)
        {
            EditUserDTO model = new EditUserDTO();
            CurrentUser user = await FindCurrentUserById(id);
            model.Id = user.Id;
            model.Email = user.Email;
            model.LastName = user.LastName;
            model.FirstName = user.FirstName;
            model.AboutMe = user.AboutMe;
            model.Image = user.Image;
            return model;
        }
        
        public Task<ICollection> GetAllUsers()
        {
            throw new NotImplementedException();
        }
    }
}
