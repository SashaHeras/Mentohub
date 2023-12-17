
using Mentohub.Core.AllExceptions;
using Mentohub.Core.Repositories.Interfaces;
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
        private readonly ICRUD_UserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly SignInManager<CurrentUser> _signInManager;
        private readonly IHubContext<SignalRHub> _hubContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        /// конструктор сервісу з параметром
        /// </summary>
        /// <param name="cRUD"></param>
        public UserService(ICRUD_UserRepository cRUD, UserManager<CurrentUser> userManager
            , RoleManager<IdentityRole> roleManager, AllException exciption,
            ILogger<UserService> logger, SignInManager<CurrentUser> signInManager,
            IHubContext<SignalRHub> hubContext, IWebHostEnvironment webHostEnvironment)
        {
            _userRepository = cRUD;
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
            var role = await _roleManager.FindByNameAsync("Customer");
            if (role == null)
            {
                role = new IdentityRole("Customer");
                await _roleManager.CreateAsync(role);
            }

            CurrentUser user = new CurrentUser() { 
                Email = model.Email, 
                UserName = model.NickName,
                DateOfBirth = DateTime.MinValue
            };

            // добавляем пользователя
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded && role != null)
            {
                _logger.LogInformation($"Role: {role.Id}, User: {user.Id}");
                await _userManager.AddToRoleAsync(user, role.Name);

                _logger.LogInformation("User created a new account with password.");

                //установка куки
                await _signInManager.SignInAsync(user, false);
                return user;
            }

            return _exciption.NotFoundObject("User was not created");            
        }

        /// <summary>
        /// сервіс видалення проіфілю користувача
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteUser(string id)
        {
            CurrentUser user = await _userRepository.FindCurrentUserById(id);
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<bool> DeleteUserByName(string userName)
        {
            CurrentUser user = await _userRepository.FindCurrentUserByName(userName);
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
            await _userRepository.UpdateAvatarUrl(userId, avatarUrl);

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
            CurrentUser currentUser = await _userRepository.FindCurrentUserById(id);
            dto.Id = currentUser.Id;  
            dto.Email = currentUser.Email;
            dto.Name = currentUser.UserName;
            dto.UserRoles = await _userRepository.GetUserRoles(currentUser);
            return dto;
        }

        /// <summary>
        /// вхід в аккаунт
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<CurrentUser> Login(LoginDTO model)
        {
            if (model.Email != null && model.Password != null)
            {
                var result = await _userRepository.Login(model);
                if (result)
                {
                    _logger.LogInformation("User logged in");
                    return await _userRepository.FindCurrentUserByEmail(model.Email);
                }
            }
            else
            {
                return _exciption.NotFoundObject("Wrong email or password");
            }
            return _exciption.NotFoundObject("User is not found");
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
            CurrentUser currentUser = await _userRepository.FindCurrentUserById(id);
            
            if (currentUser == null)
            {
                return false;
            }
            currentUser.FirstName = userDTO.FirstName;
            currentUser.LastName = userDTO.LastName;
            currentUser.AboutMe = userDTO.AboutMe;
            if (currentUser is IdentityUser identityUser)
            {
                // Перевірка, чи користувач успадковується від IdentityUser
                // Якщо так, то оновити дату народження
                currentUser.DateOfBirth = userDTO.DateOfBirth;
            }
           
            var result=await _userManager.UpdateAsync(currentUser);
            return result.Succeeded;
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<UserDTO> GetUser(string userName)
        {
            CurrentUser user = await _userRepository.FindCurrentUserByName(userName);
            if (user != null)
            {
                UserDTO userDTO = new UserDTO();
                userDTO.Id = user.Id;
                userDTO.Name = user.UserName;
                userDTO.Email = user.Email;
                userDTO.UserRoles = await _userRepository.GetUserRoles(user);
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
            return await _userRepository.FindCurrentUserById(id);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<bool> AddRoleToUserListRoles(string userId, string roleName)
        {
            var user =await _userRepository.FindCurrentUserById(userId);
            var identityRole = await _roleManager.FindByNameAsync(roleName);
            if (identityRole != null&& user!=null)
            {
                var userRoles=_userManager.GetRolesAsync(user);
                foreach (var role in await userRoles)
                {
                    if (role != identityRole.Name)
                    {
                        await _userManager.AddToRoleAsync(user, roleName);
                        return true;
                    }
                    else
                    {
                        _logger.LogInformation("Role already exists for this user");
                        return false;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// сервис завантаження аватарки користувача на сторінку користувача
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<string> GetAvatarUrl(string userId)
        {
            var user =await _userRepository.FindCurrentUserById(userId);
            if (user != null && !string.IsNullOrEmpty(user.Image))
            {
                
                 return user.Image; 
                             
            }
            
            // Повернути URL за замовчуванням, якщо користувач не має аватарки.
            return "/wwwroot/avatar/default-avatar.ipg";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<List<CurrentUser>> GetAllUsersByRoleName(string roleName)
        {
            if (!string.IsNullOrEmpty(roleName))
            {
                throw new Exception("No role name to search!");
            }

            return await _userRepository.GetAllUsers(roleName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteRole(string roleId)
        {
            IdentityRole role = await _userRepository.GetRoleById(roleId);
            if (role != null)
            {
                await _roleManager.DeleteAsync(role);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> CreateRole(string name)
        { 
            if (!string.IsNullOrEmpty(name)
                &&await _roleManager.FindByNameAsync(name)==null)
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return true;
                }              
            }
            return false;
        }

        
    }
}

