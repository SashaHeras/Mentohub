
using Mentohub.Core.AllExceptions;
using Mentohub.Core.Repositories.Repositories;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.Entities;
using Mentohub.Domain.Data.Entities.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
        private readonly IHubContext<SignalRHub> _hubContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        /// <summary>
        /// конструктор сервісу з параметром
        /// </summary>
        /// <param name="cRUD"></param>
        public UserService(CRUD_UserRepository cRUD, UserManager<CurrentUser> userManager
            , RoleManager<IdentityRole> roleManager, AllException exciption,
            ILogger<UserService> logger, SignInManager<CurrentUser> signInManager,
            IHubContext<SignalRHub> hubContext, IWebHostEnvironment webHostEnvironment)
        {
            _cRUD = cRUD;
            _userManager = userManager;
            _roleManager = roleManager;
            _exciption = exciption;
            _logger = logger;
            _signInManager = signInManager;
            _hubContext = hubContext;
            _webHostEnvironment = webHostEnvironment;
        }
        /// <summary>
        /// сервіс реєстрації користувача
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<CurrentUser> CreateUser(RegisterDTO model)
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
        public async Task<bool> DeleteUserByName(string userName)
        {
            CurrentUser user = await _cRUD.FindCurrentUserByName(userName);
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
            return (IAsyncEnumerable<CurrentUser>)_userManager.Users.ToList();
            //throw new NotImplementedException();
        }
        /// <summary>
        /// download user's avatar
        /// </summary>
        /// <param name="avatar"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<string> UploadAvatar(IFormFile avatar, string userId)
        {
            if (avatar == null || avatar.Length == 0)
            {
                return _exciption.ArgumentNullException("No file"); // Помилка: відсутні дані файлу.
            }

            // унікальне ім'я для файлу аватарки, за допомогою Guid
            var uniqueFileName = Guid.NewGuid() + Path.GetExtension(avatar.FileName);

            // Повний шлях до файлу в папці wwwroot/avatar
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "avatar", uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await avatar.CopyToAsync(stream);
            }

            // Оновіть URL аватарки в базі даних
            var avatarUrl = "/avatar/" + uniqueFileName;
            // оновлення URL аватарки в базі даних 
            await _cRUD.UpdateAvatarUrl(userId, avatarUrl);

            _logger.LogInformation("avatar is successfully saved");
            // сповіщення про зміну аватарки користувачу за допомогою SignalR
            await _hubContext.Clients.User(userId).SendAsync("ReceiveAvatarUpdate", avatarUrl);

            return avatarUrl;
        }

        /// <summary>
        /// сервіс отримання форми редагування профілю користувача
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserDTO> GetProfile(string id)
        {
            UserDTO dto = new UserDTO();
            CurrentUser currentUser = await _cRUD.FindCurrentUserById(id);
            dto.Id = currentUser.Id;  
            dto.Email = currentUser.Email;
            dto.Name = currentUser.UserName;
            dto.UserRoles = await _cRUD.GetUserRoles(currentUser);
            return dto;
        }

        /// <summary>
        /// вхід в аккаунт
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<CurrentUser> Login(LoginDTO model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email,
                 model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in");

                return await _cRUD.FindCurrentUserByEmail(model.Email);
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
        public async Task<bool> UpdateUser( string id, EditUserDTO userDTO)
        {
            CurrentUser currentUser = await _cRUD.FindCurrentUserById(id);
            userDTO = new EditUserDTO();
            if (userDTO != null)
            {
                currentUser.FirstName = userDTO.FirstName;
                currentUser.LastName = userDTO.LastName;
                currentUser.AboutMe = userDTO.AboutMe;
                //currentUser.DateOfBirth = userDTO.DateOfBirth;
                await _userManager.UpdateAsync(currentUser);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<UserDTO> GetUser(string userName)
        {
            CurrentUser user = await _cRUD.FindCurrentUserByName(userName);
            if (user != null)
            {
                UserDTO userDTO = new UserDTO();
                userDTO.Id = user.Id;
                userDTO.Name = user.UserName;
                userDTO.Email = user.Email;
                userDTO.UserRoles = await _cRUD.GetUserRoles(user);
                return userDTO;
            }
            return _exciption.NullException(nameof(userName));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CurrentUser> GetCurrentUser(string id)
        {
            return await _cRUD.FindCurrentUserById(id);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<bool> AddRoleToUserListRoles(CurrentUser user, string roleName)
        {

            var allRoles = _roleManager.Roles.ToList();
            foreach (var role in allRoles)
            {
                if (role.Name == roleName)
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                    return true;
                }
            }
            return false;
        }
    }
}

