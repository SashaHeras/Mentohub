
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.Entities;
using Mentohub.Domain.Data.Entities.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Services.Interfaces
{
    public interface IUserService
    {
        public Task<CurrentUser> GetUserById(string id);
        public Task<CurrentUser> GetUserByName(string name);
        public IAsyncEnumerable<CurrentUser> GetAllUsers();
        public Task<IItem> CreateUser( RegisterDTO model);
        public Task<IItem> UpdateUser(IFormFile avatarFile, EditUserDTO model);
        public Task<bool> DeleteUser(string id);
        public Task<EditUserDTO> GetProfile(string id);
        public Task<CurrentUser> Login(LoginDTO model);
       
        public Task<bool> LogOut();
    }
}
