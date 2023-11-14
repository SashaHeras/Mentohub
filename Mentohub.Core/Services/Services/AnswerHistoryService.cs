using Mentohub.Domain.Entities;
using Mentohub.Core.Repositories.Repositories;
using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Core.Services.Interfaces;

namespace Mentohub.Core.Services.Services
{
    public class AnswerHistoryService : IAnswerHistoryService
    {
        private IAnswerHistoryRepository _answerHistoryRepository;

        public AnswerHistoryService(IAnswerHistoryRepository answerHistoryRepository)
        {
            _answerHistoryRepository = answerHistoryRepository;
        }

        public async Task<List<AnswerHistory>> SaveAnswersHistory(List<TaskHistory> taskHistories, List<AnswerHistory> answerHistories)
        {
            foreach (var answerHistory in answerHistories)
            {
                answerHistory.TaskHistoryId = taskHistories.First(t => t.TaskId == answerHistory.TaskId).Id;
                await _answerHistoryRepository.AddAsync(answerHistory);
            }

            return answerHistories;
        }
    }
}
