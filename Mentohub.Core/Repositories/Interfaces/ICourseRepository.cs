using Mentohub.Core.Infrastructure;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Entities;

namespace Mentohub.Core.Repositories.Intefaces
{
    public interface ICourseRepository : ISingletoneService, IRepository<Course>
    {
        public Course GetCourse(int courseId);

        public IQueryable<Course> GetAllAuthorsCourses(string ID);
    }
}
