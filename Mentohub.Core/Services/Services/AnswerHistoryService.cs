using Mentohub.Domain.Entities;
using Mentohub.Core.Repositories.Repositories;

namespace Mentohub.Core.Services.Services
{
    public class AnswerHistoryService
    {
        private AnswerHistoryRepository _answerHistoryRepository;

        public AnswerHistoryService(AnswerHistoryRepository answerHistoryRepository)
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
