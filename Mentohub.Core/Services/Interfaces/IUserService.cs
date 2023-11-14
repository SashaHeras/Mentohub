
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
       
        public IAsyncEnumerable<CurrentUser> GetAllUsers();
        public Task<CurrentUser> CreateUser( RegisterDTO model);
        public Task<bool> UpdateUser( string id, EditUserDTO model);
        public Task<bool> DeleteUser(string id);
        public Task<UserDTO> GetProfile(string id);
        public Task<CurrentUser> Login(LoginDTO model);
        public Task<UserDTO>GetUser(string userName);
        public Task<bool> LogOut();
        public Task<CurrentUser> GetCurrentUser(string id);
        public Task<string> UploadAvatar(IFormFile avatar, string userId);
        public Task<bool> CreateRole(string name);
        public Task<bool> DeleteRole(string roleId);
    }
}
