using Mentohub.Core.Infrastructure;
using Mentohub.Domain.Entities;

namespace Mentohub.Core.Repositories.Intefaces
{
    public interface ITaskRepository : ISingletoneService, IRepository<TestTask>
    {
        public IQueryable<TestTask> GetTaskByTestId(int testId);

        public TestTask GetById(int taskId);

        public List<TestTask> GetTasksOfTestBiggerThanOrder(int testId, int order);

        public bool DeleteTask(TestTask task);
    }
}
