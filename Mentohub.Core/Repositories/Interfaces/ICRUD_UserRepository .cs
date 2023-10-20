using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Repositories.Interfaces
{
    public interface ICRUD_UserRepository:ICreateUserRepository,IDeleteUserRepository,
        IUpdateUserRepository,IReadUserRepository,ILoginRepository
    {
    }
}
