using Mentohub.Core.Repositories.Repositories;
using Mentohub.Domain.Entities;

namespace Mentohub.Core.Services.Services
{
    public class TaskService
    {
        private TaskRepository _taskRepository;
        private AnswerRepository _answerRepository;

        private AnswerService _answerService;   

        public TaskService(TaskRepository taskRepository, AnswerRepository answerRepository, AnswerService answerService)
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

        public async Task<TestTask> CreateNewTask(string name, int order, int mark, int testId)
        {
            TestTask testTask = new TestTask()
            {
                Name = name,
                OrderNumber = order,
                Mark = mark,
                TestId = testId
            };

            await _taskRepository.AddAsync(testTask);

            return testTask;
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
    }
}
