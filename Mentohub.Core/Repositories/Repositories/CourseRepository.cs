using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mentohub.Core.Repositories.Repositories
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        private ProjectContext _context;

        public CourseRepository(ProjectContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }

        /// <summary>
        /// Method returns all course of authod by his id
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public IQueryable<Course> GetAllAuthorsCourses(Guid uid)
        {
            return GetAll().Where(c => c.AuthorId == uid);
        }

        /// <summary>
        /// Method return course by id
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public Course GetCourse(int courseId)
        {
            return GetAll().Where(course => course.Id == courseId).FirstOrDefault();
        }

        /// <summary>
        /// Method execute stored procedure from db to get a list of course elements
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public string GetCourseElementsList(string courseId)
        {
            string query = "EXEC GetCourseElementsList @courseId = " + courseId;
            var result = _context.Database.SqlQueryRaw<string>(query).AsEnumerable().FirstOrDefault();
            return result;
        }
    }
}
