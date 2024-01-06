using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Core.Repositories.Interfaces.CourseInterfaces;
using Mentohub.Domain.Data.Entities.CourseEntities;
using Microsoft.EntityFrameworkCore;

namespace Mentohub.Core.Repositories.Repositories.CourseRepositories
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        #pragma warning disable 8603
        public TagRepository(ProjectContext repositoryContext) : base(repositoryContext)
        {

        }

        public Tag GetById(int id)
        {
            return GetAll().Where(x => x.ID == id)
                           .Include(x => x.User)
                           .Include(x => x.CourseTags)
                           .FirstOrDefault();
        }
    }
}
