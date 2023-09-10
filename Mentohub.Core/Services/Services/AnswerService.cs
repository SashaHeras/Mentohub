using Mentohub.Core.Repositories.Repositories;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Entities;

namespace Mentohub.Core.Services.Services
{
    public class AnswerService
    {
        private AnswerRepository _answerRepository;

        public AnswerService(AnswerRepository answerRepository) { 
            _answerRepository = answerRepository;
        }

        public void RemoveAnswers(IQueryable<TaskAnswer> answers)
        {
            foreach (var answer in answers)
            {
                _answerRepository.DeleteAnswer(answer);
            }
        }

        public void RemoveAnswer(TaskAnswer answer)
        {
            _answerRepository.DeleteAnswer(answer);
        }

        public IQueryable<TaskAnswer> GetAnswers(int taskId)
        {
            return _answerRepository.GetAnswersByTaskId(taskId);
        }

        public TaskAnswer GetAnswer(int id)
        {
            return _answerRepository.GetAnswerById(id);
        }

        public async Task<bool> SavingAnswers(TestTask editedTask, Dictionary<string, bool> answers, string[] ids)
        {
            int counter = 0;

            foreach (var answer in answers)
            {
                int currentAnswerId = 0;

                try
                {
                    if (int.TryParse(ids[counter], out currentAnswerId) == false)
                    {
                        TaskAnswer newAnswer = new TaskAnswer()
                        {
                            Name = answer.Key,
                            IsCorrect = answer.Value,
                            TaskId = editedTask.Id
                        };

                        await _answerRepository.AddAsync(newAnswer);
                    }
                    else
                    {
                        TaskAnswer updateAnswer = _answerRepository.GetAnswerById(currentAnswerId);
                        updateAnswer.Name = answer.Key;
                        updateAnswer.IsCorrect = answer.Value;

                        await _answerRepository.UpdateAsync(updateAnswer);
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }

                counter++;
            }

            return true;
        }

        public async void AddAnswer(TaskAnswer answer)
        {
            try
            {
                await _answerRepository.AddAsync(answer);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetCountOfCorrectAnswers(int taskId)
        {
            return _answerRepository.GetCountOfCorrectAnswers(taskId);
        }

        public Dictionary<string, bool> AnswersSpliter(string answers, string _checked)
        {
            string[] _answers = answers.Split(',');
            string[] checkedAns = _checked.Split(',');

            Dictionary<string, bool> result = new Dictionary<string, bool>();

            for (int i = 0; i < _answers.Length; i++)
            {
                result.Add(_answers[i].ToString().Replace('|', ','), checkedAns[i] == "true" ? true : false);
            }

            return result;
        }

        public int PopulateAnswerHistories(List<AnswerDTO> answers, TestTask task, List<AnswerHistory> answerHistories)
        {
            var result = 0;

            foreach (var taskAnswer in answers)
            {
                bool isCorrect = _answerRepository.GetAnswerByIdAndTask(taskAnswer.AnswerId, task.Id).IsCorrect;

                var answerHistory = new AnswerHistory
                {
                    TaskId = task.Id,
                    AnswerId = taskAnswer.AnswerId,
                    IsCorrect = (taskAnswer.IsChecked == isCorrect)
                };

                if (answerHistory.IsCorrect)
                {
                    result++;
                }

                answerHistories.Add(answerHistory);
            }

            return result;
        }
    }
}
