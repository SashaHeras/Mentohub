using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mentohub.Core.Repositories.Repositories
{
    public class TestRepository : Repository<Test>, ITestRepository
    {
        #pragma warning disable 8603
        private readonly ProjectContext _context;

        public TestRepository(ProjectContext repositoryContext) : base(repositoryContext)
        {
            this._context = repositoryContext;
        }

        public Test GetById(int testId)
        {
            return GetAll()
                   .Where(t => t.Id == testId)
                   .Include(x => x.CourseItem)
                   .Include(x => x.TestHistory)
                   .Include(x => x.TestTasks)
                   .FirstOrDefault();
        }

        public Test GetTestByCourseItemId(int courseItemId)
        {
            return GetAll().Where(t => t.CourseItemId == courseItemId).FirstOrDefault();
        }
    }
}
