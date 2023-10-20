using Mentohub.Domain.Data.Entities;
using Mentohub.Domain.Data.Entities.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Repositories.Interfaces
{
    public interface IReadUserRepository
    {
     
        public Task<ICollection> GetAllUsers();
        public Task<CurrentUser> FindCurrentUserByName(string name);
        public Task<CurrentUser> FindCurrentUserById(string id);
    }
}
