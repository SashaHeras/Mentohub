using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Domain.Entities;

namespace Mentohub.Core.Repositories.Repositories
{
    public class TestHistoryRepository : Repository<TestHistory>, ITestHistoryRepository
    {
        private ProjectContext _context;

        public TestHistoryRepository(ProjectContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
