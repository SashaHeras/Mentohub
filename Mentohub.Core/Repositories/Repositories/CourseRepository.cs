using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Domain.Data.Entities.CourseEntities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Mentohub.Core.Repositories.Repositories
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        #pragma warning disable 8603
        private readonly ProjectContext _context;

        public CourseRepository(ProjectContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }

        /// <summary>
        /// Method returns all course of authod by his id
        /// </summary>
        /// <param name=")"></param>
        /// <returns></returns>
        public IQueryable<Course> GetAllAuthorsCourses(string ID)
        {
            return GetAll().Where(c => c.AuthorId == ID);
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

        public Course FirstOrDefault(Expression<Func<Course, bool>> expression)
        {
            return GetAll(expression).Include(x => x.Comments)
                                     .Include(x => x.CourseItems)
                                     .Include(x => x.CourseViews)
                                     .Include(x => x.CourseBlocks)
                                     .Include(x => x.Author)
                                     .FirstOrDefault();
        }
    }
}
