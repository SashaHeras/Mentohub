using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Core.Repositories.Interfaces;
using Mentohub.Domain.Data.Entities.CourseEntities;
using Microsoft.EntityFrameworkCore;

namespace Mentohub.Core.Repositories.Repositories
{
    public class CourseLanguageRepository : Repository<CourseLanguage>, ICourseLanguageRepository
    {
        #pragma warning disable 8603
        public CourseLanguageRepository(ProjectContext repositoryContext) : base(repositoryContext)
        {

        }

        public CourseLanguage FindById(int ID)
        {
            return GetAll().Where(x => x.Id == ID)
                   .Include(x => x.Courses)
                   .FirstOrDefault();
        }
    }
}
