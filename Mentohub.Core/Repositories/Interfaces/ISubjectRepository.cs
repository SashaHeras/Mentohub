using Mentohub.Core.Infrastructure;
using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Repositories.Interfaces
{
    public interface ISubjectRepository: ISingletoneService, IRepository<CourseSubject>
    {

    }
}
