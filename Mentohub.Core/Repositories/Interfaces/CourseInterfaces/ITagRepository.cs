using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Domain.Data.Entities.CourseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Repositories.Interfaces.CourseInterfaces
{
    public interface ITagRepository : IRepository<Tag>
    {
        public Tag GetById(int id);
    }
}
