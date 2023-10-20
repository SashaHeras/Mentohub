using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Repositories.Interfaces
{
    public interface IDeleteUserRepository
    {
        public Task<bool> Delete(string id);
        public Task<bool> LogOut();
    }
}
