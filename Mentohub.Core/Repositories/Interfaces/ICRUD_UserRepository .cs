using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.Entities;
using Mentohub.Domain.Data.Entities.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Repositories.Interfaces
{
    public interface ICRUD_UserRepository        
    {
        public Task<List<CurrentUser>> GetAllUsers(string roleName);

        public Task<CurrentUser> FindCurrentUserByName(string name);

        public Task<CurrentUser> FindCurrentUserById(string id); 
        
        public Task<CurrentUser> FindCurrentUserByEmail(string email);

        public Task<List<string>> GetUserRoles(CurrentUser user);

        public Task<IdentityRole> GetRoleById(string roleId);

        public Task<bool> Login(LoginDTO model);

        public Task UpdateAvatarUrl(string userId, string avatarUrl);
        public Task<List<IdentityRole>> GetAllRoles();
        public  Task<IdentityRole> GetRoleName(string roleId);
    }
}
