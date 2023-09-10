using Mentohub.Core.Infrastructure;
using Mentohub.Domain.Entities;

namespace Mentohub.Core.Repositories.Intefaces
{
    public interface ICourseRepository : ISingletoneService, IRepository<Course>
    {
        public string GetCourseElementsList(string courseId);

        public Course GetCourse(int courseId);

        public IQueryable<Course> GetAllAuthorsCourses(Guid uid);
    }
}
