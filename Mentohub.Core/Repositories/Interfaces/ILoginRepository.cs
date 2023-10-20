using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.Entities;
using Mentohub.Domain.Data.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Repositories.Interfaces
{
    public interface ILoginRepository
    {
        public Task<CurrentUser> LoginAsync(LoginDTO model);
    }
}
