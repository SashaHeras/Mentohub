
using Mentohub.Core.Repositories.Repositories;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.Entities;
using Mentohub.Domain.Data.Entities.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Services.Services
{
    public class UserService : IUserService
    {
        private CRUD_UserRepository _cRUD;
        /// <summary>
        /// конструктор сервісу з параметром
        /// </summary>
        /// <param name="cRUD"></param>
        public UserService(CRUD_UserRepository cRUD)
        {
            _cRUD = cRUD;
        }
        /// <summary>
        /// сервіс реєстрації користувача
        /// </summary>
        /// <param name="form"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IItem> CreateUser( RegisterDTO model)
        {
          return  await _cRUD.Register( model);
        }
        /// <summary>
        /// сервіс видалення проіфілю користувача
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteUser(string id)
        {
           return await _cRUD.Delete(id);
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
        public async Task<CurrentUser> Login(LoginDTO model)
        {
           return await _cRUD.LoginAsync(model);
        }
        /// <summary>
        /// вихід з аккаунта
        /// </summary>
        /// <returns></returns>
        public async Task<bool> LogOut()
        {
            return await _cRUD.LogOut();
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
