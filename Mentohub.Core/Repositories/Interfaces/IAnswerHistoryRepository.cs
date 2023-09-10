using Mentohub.Core.Infrastructure;
using Mentohub.Domain.Entities;

namespace Mentohub.Core.Repositories.Intefaces 
{
    public interface IAnswerHistoryRepository : ISingletoneService, IRepository<AnswerHistory>
    {

    }
}
