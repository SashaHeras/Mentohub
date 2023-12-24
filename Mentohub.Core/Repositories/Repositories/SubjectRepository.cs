using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Core.Repositories.Interfaces;
using Mentohub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Repositories.Repositories
{
    public class SubjectRepository : Repository<CourseSubject>, ISubjectRepository
    {
        public SubjectRepository(ProjectContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
