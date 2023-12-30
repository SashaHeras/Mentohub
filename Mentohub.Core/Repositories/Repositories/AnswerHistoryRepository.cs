using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Domain.Entities;

namespace Mentohub.Core.Repositories.Repositories
{
    public class AnswerHistoryRepository : Repository<AnswerHistory>, IAnswerHistoryRepository
    {
        #pragma warning disable 8603
        private readonly ProjectContext _context;

        public AnswerHistoryRepository(ProjectContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
