using Mentohub.Core.Infrastructure;
using Mentohub.Domain.Entities;

namespace Mentohub.Core.Repositories.Intefaces
{
    public interface IAnswerRepository : ISingletoneService, IRepository<TaskAnswer>
    {
        public TaskAnswer GetAnswerById(int id);

        public bool DeleteAnswer(TaskAnswer answer);

        public IQueryable<TaskAnswer> GetAnswersByTaskId(int taskId);

        public int GetCountOfCorrectAnswers(int taskId);

        public TaskAnswer GetAnswerByIdAndTask(int id, int taskId);
    }
}
