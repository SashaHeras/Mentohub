using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Core.Repositories.Repositories;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Domain.Entities;

namespace Mentohub.Core.Services.Services
{
    public class TaskHistoryService : ITaskHistoryService
    {
        private ITaskHistoryRepository _taskHistoryRepository;

        public TaskHistoryService(ITaskHistoryRepository taskHistoryRepository) {
            _taskHistoryRepository = taskHistoryRepository;
        }

        public async Task<List<TaskHistory>> SaveTasksHistory(TestHistory history, List<TaskHistory> taskHistories)
        {
            foreach (var taskHistory in taskHistories)
            {
                taskHistory.TestHistoryId = history.Id;
                await _taskHistoryRepository.AddAsync(taskHistory);
            }

            return taskHistories;
        }
    }
}
