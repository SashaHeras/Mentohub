using Mentohub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Services.Interfaces
{
    public interface IAnswerHistoryService
    {
        Task<List<AnswerHistory>> SaveAnswersHistory(List<TaskHistory> taskHistories, List<AnswerHistory> answerHistories);
    }
}
