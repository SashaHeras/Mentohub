using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Entities;

namespace Mentohub.Core.Services.Interfaces
{
    public interface ITaskService : IService
    {
        IQueryable<TestTask> GetTasks(int testId);

        List<TaskDTO> GetTasksList(int testId);

        public TaskDTO GetTask(int id);

        Task<TestTask> UpdateTask(TestTask task);

        TaskDTO Edit(TaskDTO data);

        List<TestTask> GetTasksAfter(int testId, int order);

        void DeleteTask(int ID);
    }
}
