using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Entities;

namespace Mentohub.Core.Services.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IAnswerRepository _answerRepository;

        private readonly IAnswerService _answerService;

        public TaskService(
            ITaskRepository taskRepository,
            IAnswerRepository answerRepository,
            IAnswerService answerService
            )
        {
            _taskRepository = taskRepository;
            _answerRepository = answerRepository;
            _answerService = answerService;
        }

        public IQueryable<TestTask> GetTasks(int testId)
        {
            return _taskRepository.GetTaskByTestId(testId).OrderBy(t => t.OrderNumber);
        }

        public TaskDTO GetTask(int id)
        {
            TestTask task = _taskRepository.GetById(id);

            TaskDTO data = new TaskDTO()
            {
                Id = task.Id,
                Name = task.Name,
                OrderNumber = task.OrderNumber,
                IsFewAnswersCorrect = task.IsFewAnswersCorrect,
                Mark = task.Mark,
                TestId = task.TestId,
                Answers = task.TaskAnswers.Select(x => new AnswerDTO()
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsChecked = x.IsCorrect,
                    TaskId = x.TaskId
                }).ToList(),
            };

            return data;
        }

        public async Task<TestTask> UpdateTask(TestTask task)
        {
            return await _taskRepository.UpdateAsync(task);
        }

        public TaskDTO Edit(TaskDTO data)
        {
            if (data == null)
            {
                throw new Exception();
            }

            TestTask task = _taskRepository.GetById(data.Id);
            int sameTestTasksCount = _taskRepository.GetTaskByTestId(data.TestId).ToList().Count();
            sameTestTasksCount = sameTestTasksCount == 0 ? 1 : sameTestTasksCount + 1;

            if (task == null)
            {
                task = new TestTask()
                {
                    TestId = data.TestId,
                    Name = data.Name,
                    Mark = data.Mark,
                    OrderNumber = sameTestTasksCount,
                    IsFewAnswersCorrect = data.IsFewAnswersCorrect
                };

                task = _taskRepository.Add(task);
            }
            else
            {
                task.Mark = data.Mark;
                task.OrderNumber = data.OrderNumber;
                task.Name = data.Name;
                task.IsFewAnswersCorrect = task.IsFewAnswersCorrect;

                _taskRepository.Update(task);
            }

            data.OrderNumber = task.OrderNumber;
            data.Id = task.Id;

            foreach(var answer in data.Answers)
            {
                var currAnswer = _answerRepository.FirstOrDefault(x => x.Id == answer.Id);
                if(currAnswer != null)
                {
                    currAnswer.Name = answer.Name;
                    currAnswer.IsCorrect = answer.IsChecked;

                    _answerRepository.Update(currAnswer);
                }
                else
                {
                    currAnswer = _answerRepository.Add(new TaskAnswer()
                    {
                        Id= _answerRepository.GetAll().ToList().Count+1,
                        Name = answer.Name,
                        IsCorrect = answer.IsChecked,
                        TaskId = task.Id
                    });
                }
               
                //answer.Id = currAnswer.Id;
                //answer.TaskId = currAnswer.TaskId;
            }

            return data;
        }

        public List<TestTask> GetTasksAfter(int testId, int order)
        {
            return _taskRepository.GetTasksOfTestBiggerThanOrder(testId, order);
        }

        /// <summary>
        /// Get list of tesks on test
        /// </summary>
        /// <param name="testId"></param>
        /// <returns></returns>
        public List<TaskDTO> GetTasksList(int testId)
        {
            return _taskRepository.GetTaskByTestId(testId)
                .Select(x => new TaskDTO()
                {
                    Id = x.Id,
                    Name = x.Name,
                    OrderNumber = x.OrderNumber,
                    TestId = x.TestId,
                    Mark = x.Mark
                }).ToList();
        }

        public void DeleteTask(int ID)
        {
            var task = _taskRepository.FirstOrDefault(x=>x.Id == ID);
            var allTasksAfter = GetTasksAfter(task.TestId, task.OrderNumber);
            int currentOrderNumber = task.OrderNumber;

            var taskAnswers = _answerRepository.GetAnswersByTaskId(task.Id);

            _answerService.RemoveAnswers(taskAnswers);
            _taskRepository.DeleteTask(task);

            if (allTasksAfter.Count > 0)
            {
                foreach (var t in allTasksAfter)
                {
                    task.OrderNumber = currentOrderNumber;
                    currentOrderNumber++;
                }

                _taskRepository.UpdateList(allTasksAfter);
            }
        }
    }
}
