using Mentohub.Core.Infrastructure;
using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.Entities.CourseEntities;

namespace Mentohub.Core.Repositories.Interfaces.CourseInterfaces
{
    public interface ICourseRepository : ISingletoneService, IRepository<Course>
    {
        public Course GetCourse(int courseId);

        public IQueryable<Course> GetAllAuthorsCourses(string ID);
    }
}
