using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Domain.Entities;

namespace Mentohub.Core.Repositories.Repositories
{
    public class TaskHistoryRepository : Repository<TaskHistory>, ITaskHistoryRepository
    {
        private ProjectContext _context;

        public TaskHistoryRepository(ProjectContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }      
    }
}
