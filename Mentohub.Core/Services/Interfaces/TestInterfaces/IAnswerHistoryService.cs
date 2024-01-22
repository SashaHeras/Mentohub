using Mentohub.Domain.Entities;

namespace Mentohub.Core.Services.Interfaces
{
    public interface IAnswerHistoryService
    {
        Task<List<AnswerHistory>> SaveAnswersHistory(List<TaskHistory> taskHistories, List<AnswerHistory> answerHistories);
    }
}
