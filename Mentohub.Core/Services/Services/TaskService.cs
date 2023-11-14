using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Entities;

namespace Mentohub.Core.Services.Services
{
    public class TaskService : ITaskService
    {
        private ITaskRepository _taskRepository;
        private IAnswerRepository _answerRepository;

        private IAnswerService _answerService;   

        public TaskService(ITaskRepository taskRepository, IAnswerRepository answerRepository, IAnswerService answerService)
        {
            _taskRepository = taskRepository;
            _answerRepository = answerRepository;
            _answerService = answerService;
        }

        public IQueryable<TestTask> GetTasks(int testId)
        {
            return _taskRepository.GetTaskByTestId(testId).OrderBy(t => t.OrderNumber);
        }

        public TestTask GetTask(int id)
        {
            return _taskRepository.GetTaskById(id);
        }

        public async Task<TestTask> UpdateTask(TestTask task)
        {
            return await _taskRepository.UpdateAsync(task);
        }

        public TaskDTO Edit(TaskDTO data)
        {
            if(data.TestId == 0)
            {
                throw new Exception("Unknown test!");
            }

            TestTask task = _taskRepository.GetTaskById(data.Id);
            int sameTestTasksCount = _taskRepository.GetTaskByTestId(data.TestId).ToList().Count();
            sameTestTasksCount = sameTestTasksCount == 0 ? 1 : sameTestTasksCount + 1;

            if (task == null)
            {
                task = new TestTask();
                task.TestId = data.TestId;
                task.Name = data.Name;
                task.Mark = data.Mark;
                task.OrderNumber = sameTestTasksCount;
                task.IsFewAnswersCorrect = false;

                _taskRepository.Add(task);
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

            return data;
        }

        public async void ResetOrderNumbers(int order, List<TestTask> allTasksAfter)
        {
            foreach (var task in allTasksAfter)
            {
                task.OrderNumber = order;
                order++;
                await _taskRepository.UpdateAsync(task);
            }
        }

        public List<TestTask> GetTasksAfter(int testId, int order)
        {
            return _taskRepository.GetTasksOfTestBiggerThanOrder(testId, order);
        }

        public void DeleteTask(TestTask task)
        {
            var taskAnswers = _answerRepository.GetAnswersByTaskId(task.Id);

            _answerService.RemoveAnswers(taskAnswers);
            _taskRepository.DeleteTask(task);
        }

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
    }
}
