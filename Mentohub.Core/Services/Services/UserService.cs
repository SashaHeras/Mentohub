
using Mentohub.Core.AllExceptions;
using Mentohub.Core.Repositories.Repositories;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.Entities;
using Mentohub.Domain.Data.Entities.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Services.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<CurrentUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AllException _exciption;
        private CRUD_UserRepository _cRUD;
        private readonly ILogger<UserService> _logger;
        private readonly SignInManager<CurrentUser> _signInManager;
        /// <summary>
        /// конструктор сервісу з параметром
        /// </summary>
        /// <param name="cRUD"></param>
        public UserService(CRUD_UserRepository cRUD, UserManager<CurrentUser> userManager
            , RoleManager<IdentityRole> roleManager, AllException exciption,
            ILogger<UserService> logger, SignInManager<CurrentUser> signInManager)
        {
            _cRUD = cRUD;
            _userManager = userManager;
            _roleManager = roleManager;
            _exciption = exciption;
            _logger = logger;
            _signInManager = signInManager;
        }
        /// <summary>
        /// сервіс реєстрації користувача
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<CurrentUser> CreateUser( RegisterDTO model)
        {
            if (await _roleManager.FindByNameAsync("Customer") == null)
            {
                var role = await _roleManager.CreateAsync(new IdentityRole("Customer"));
            }

            CurrentUser user = new CurrentUser { Email = model.Email, UserName = model.NickName };
            // добавляем пользователя
            var result = await _userManager.CreateAsync(user, model.Password);
            //получаем роль

            if (result.Succeeded == true)
            {
                await _userManager.AddToRoleAsync(user, "Customer");

                _logger.LogInformation("User created a new account with password.");

                //установка куки
                await _signInManager.SignInAsync(user, false);
                return user;
            }
            else
            {
                // Якщо створення користувача не вдалося, виводимо помилки в лог або консоль
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"Помилка при створенні користувача: {error.Description}");
                }
                return _exciption.NotFoundObject("User isn't created");
            }
        }
        /// <summary>
        /// сервіс видалення проіфілю користувача
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteUser(string id)
        {
            CurrentUser user = await _cRUD.FindCurrentUserById(id);
            if (user == null)
            {
                return _exciption.RankException("User does not exist");
            }
            await _signInManager.SignOutAsync();
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                // Успішно видалено
                return true;
            }
            else
            {
                // Помилка при видаленні користувача
                return false;
            }
        }

        public IAsyncEnumerable<CurrentUser> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// сервіс отримання переліку всіх користувачів
        /// </summary>
        /// <returns></returns>
        //public  IAsyncEnumerable<CurrentUser> GetAllUsers()
        //{
        //    return  _cRUD.GetAllUsers() as IAsyncEnumerable<CurrentUser>;
        //}
        /// <summary>
        /// сервіс отримання форми редагування профілю користувача
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EditUserDTO> GetProfile(string id)
        {
            return await _cRUD.GetUserProfile(id);
        }
        /// <summary>
        /// сервіс пошуку користувача по id)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CurrentUser> GetUserById(string id)
        {
           return await _cRUD.FindCurrentUserById(id);
        }
        /// <summary>
        /// сервіс пошуку користувача по Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<CurrentUser> GetUserByName(string name)
        {
            return await _cRUD.FindCurrentUserByName(name);
        }

        /// <summary>
        /// вхід в аккаунт
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<CurrentUser> Login( LoginDTO model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email,
                 model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in");

                return await _cRUD.FindCurrentUserByName(model.Email);
            }
            else
            {
                return _exciption.NotFoundObject("User is not found");
            }
        }
        /// <summary>
        /// вихід з аккаунта
        /// </summary>
        /// <returns></returns>
        public async Task<bool> LogOut()
        {
            await _signInManager.SignOutAsync();
            return true;
        }
        /// <summary>
        /// оновлення даних користувача
        /// </summary>
        /// <param name="avatarFile"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IItem> UpdateUser(IFormFile avatarFile, EditUserDTO model)
        {
            return await _cRUD.Edit(avatarFile,model);
        }
    }
}
