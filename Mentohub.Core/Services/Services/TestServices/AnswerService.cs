using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Entities;

namespace Mentohub.Core.Services.Services
{
    public class AnswerService : IAnswerService
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly ITaskRepository _taskRepository;

        public AnswerService(
            IAnswerRepository answerRepository,
            ITaskRepository taskRepository)
        {
            _answerRepository = answerRepository;
            _taskRepository = taskRepository;
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

        public List<AnswerDTO> GetAnswersList(int id)
        {
            return _answerRepository.GetAnswersByTaskId(id)
                .Select(x => new AnswerDTO()
                {
                    Name = x.Name,
                    Id = x.Id,
                    IsChecked = x.IsCorrect,
                    TaskId = x.TaskId
                }).ToList();
        }

        public TaskAnswer GetAnswer(int id)
        {
            return _answerRepository.GetById(id);
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
                        TaskAnswer newAnswer = new()
                        {
                            Name = answer.Key,
                            IsCorrect = answer.Value,
                            TaskId = editedTask.Id
                        };

                        await _answerRepository.AddAsync(newAnswer);
                    }
                    else
                    {
                        TaskAnswer updateAnswer = _answerRepository.GetById(currentAnswerId);
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

        public int PopulateAnswerHistories(List<AnswerDTO> answers, TestTask task, List<AnswerHistory> answerHistories)
        {
            var result = 0;

            //foreach (var taskAnswer in answers)
            //{
            //    bool isCorrect = _answerRepository.GetAnswerByIdAndTask(taskAnswer.AnswerId, task.Id).IsCorrect;

            //    var answerHistory = new AnswerHistory
            //    {
            //        TaskId = task.Id,
            //        AnswerId = taskAnswer.AnswerId,
            //        IsCorrect = (taskAnswer.IsChecked == isCorrect)
            //    };

            //    if (answerHistory.IsCorrect)
            //    {
            //        result++;
            //    }

            //    answerHistories.Add(answerHistory);
            //}

            return result;
        }

        public List<AnswerDTO> EditAnswers(List<AnswerDTO> answers)
        {
            List<AnswerDTO> result = new();
            var task = _taskRepository.GetById(answers.First().TaskId);

            int correctAnswersCnt = 0;
            foreach (var item in answers)
            {
                var answer = _answerRepository.GetById(item.Id);
                if (answer != null)
                {
                    answer.IsCorrect = item.IsChecked;
                    answer.Name = item.Name;

                    _answerRepository.Update(answer);
                }
                else
                {
                    answer = new TaskAnswer()
                    {
                        IsCorrect = item.IsChecked,
                        Name = item.Name,
                        TaskId = item.TaskId
                    };

                    _answerRepository.Add(answer);
                }

                if (answer.IsCorrect == true)
                {
                    correctAnswersCnt++;
                }

                result.Add(answer.ToDTO());
            }

            task.IsFewAnswersCorrect = correctAnswersCnt > 1;
            _taskRepository.Update(task);

            return result;
        }

        public void DeleteAnswer(int ID)
        {
            var answer = GetAnswer(ID) ?? throw new Exception("Answer not found!");
            _answerRepository.DeleteAnswer(answer);
        }
    }
}
