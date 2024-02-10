using Mentohub.Domain.Entities;

namespace Mentohub.Core.Services.Interfaces
{
    public interface IAnswerHistoryService : IService
    {
        Task<List<AnswerHistory>> SaveAnswersHistory(List<TaskHistory> taskHistories, List<AnswerHistory> answerHistories);
    }
}
