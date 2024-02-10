﻿
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Services.Interfaces
{
    public interface IUserService : IService
    {
        public IList<CurrentUser> GetAllUsers();
        public Task<CurrentUser> CreateUser( RegisterDTO model);
        public Task<bool> UpdateUser(UserDTO model);
        public Task<bool> DeleteUser(string id);
        public Task<UserDTO> GetProfile(string id);
        public Task<CurrentUser> Login(LoginDTO model);
        public Task<UserDTO>GetUser(string userName);
        public Task<bool> LogOut();
        public Task<CurrentUser> GetCurrentUser(string id);
        public Task<string> UploadAvatar(IFormFile avatar, string userId);
        public Task<bool> CreateRole(string name);
        public Task<bool> DeleteRole(string roleId);
        public Task<bool> AddRoleToUserListRoles(string userId, string roleName);
        public Task<string> GetAvatarUrl(string userId);
        public Task<List<CurrentUser>> GetAllUsersByRoleName(string roleName);
        public Task<ChangeRoleDTO?> GetChangeRoleDTO(string userId);
        public Task<string> AddRoleAuthor(string userId);
        public Task<string> ForgotPassword(string email);
        string GenerateRandomPassword();
        public Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);

    }
}
