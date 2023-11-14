using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Domain.Entities;

namespace Mentohub.Core.Repositories.Repositories
{
    public class TestRepository : Repository<Test>, ITestRepository
    {
        private readonly ProjectContext _context;

        public TestRepository(ProjectContext repositoryContext) : base(repositoryContext)
        {
            this._context = repositoryContext;
        }

        public Test GetTestById(int testId)
        {
            return GetAll().Where(t => t.Id == testId).FirstOrDefault();
        }

        public Test GetTestByCourseItemId(int courseItemId)
        {
            return GetAll().Where(t => t.CourseItemId == courseItemId).FirstOrDefault();
        }
    }
}
