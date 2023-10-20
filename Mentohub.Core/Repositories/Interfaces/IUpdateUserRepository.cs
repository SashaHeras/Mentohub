using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.Entities.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Repositories.Interfaces
{
    public interface IUpdateUserRepository
    {
        public Task<EditUserDTO> GetUserProfile(string id);
        public Task<IItem> Edit(IFormFile avatarFile, EditUserDTO model);
    }
}
