using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Services.Interfaces
{
    public interface IAnswerService
    {
        void RemoveAnswers(IQueryable<TaskAnswer> answers);

        void RemoveAnswer(TaskAnswer answer);

        IQueryable<TaskAnswer> GetAnswers(int taskId);

        List<AnswerDTO> GetAnswersList(int id);

        TaskAnswer GetAnswer(int id);

        Task<bool> SavingAnswers(TestTask editedTask, Dictionary<string, bool> answers, string[] ids);

        List<AnswerDTO> EditAnswers(List<AnswerDTO> answers);

        void AddAnswer(TaskAnswer answer);

        int GetCountOfCorrectAnswers(int taskId);

        Dictionary<string, bool> AnswersSpliter(string answers, string _checked);

        void DeleteAnswer(int ID);

        int PopulateAnswerHistories(List<AnswerDTO> answers, TestTask task, List<AnswerHistory> answerHistories);
    }
}
