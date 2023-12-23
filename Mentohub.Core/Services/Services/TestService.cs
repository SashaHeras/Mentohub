using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Core.Repositories.Repositories;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.DTO.Test;
using Mentohub.Domain.Data.Enums;
using Mentohub.Domain.Entities;

namespace Mentohub.Core.Services.Services
{
    public class TestService : ITestService
    {
        private readonly ITestRepository _testRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseItemRepository _courseItemRepository;
        private readonly ITestHistoryRepository _testHistoryRepository;
        private readonly IAnswerHistoryRepository _answerHistoryRepository;
        private readonly ITaskHistoryRepository _taskHistoryRepository;
        private readonly ProjectContext repositoryContext;

        private readonly ITaskService _taskService;
        private readonly IAnswerService _answerService;
        private readonly ICourseItemService _courseItemService;
        private readonly ITestHistoryService _testHistoryService;
        private readonly ITaskHistoryService _taskHistoryService;
        private readonly IAnswerHistoryService _answerHistoryService;

        public TestService( 
            ITestRepository testRepository, 
            ICourseItemRepository courseItemRepository,
            ICourseItemService courseItemService, 
            ITestHistoryService testHistoryService,
            ITaskHistoryService taskHistoryService, 
            IAnswerHistoryService answerHistoryService,
            ICourseRepository courseRepository,
            ITaskHistoryRepository taskHistoryRepository,
            IAnswerHistoryRepository answerHistoryRepository,
            ITestHistoryRepository testHistoryRepository,
            ITaskService taskService,
            IAnswerService answerService,
            ProjectContext repositoryContext)
        {
            _testRepository = testRepository;
            _courseItemRepository = courseItemRepository;
            _courseItemService = courseItemService;
            _testHistoryService = testHistoryService;
            _taskHistoryService = taskHistoryService;
            _answerHistoryService = answerHistoryService;
            _courseRepository = courseRepository;
            this.repositoryContext = repositoryContext;
            _answerHistoryRepository = answerHistoryRepository;
            _taskHistoryRepository = taskHistoryRepository;
            _testHistoryRepository = testHistoryRepository;
            _taskService = taskService;
            _answerService = answerService;
        }             

        public Test GetTest(int id)
        {
            return _testRepository.GetTestById(id);
        }

        public Test GetTestByCourseItem(int courseItemId)
        {
            return _testRepository.GetTestByCourseItemId(courseItemId);
        }

        public TestDTO GetTestModel(int courseItemId)
        {
            Test test = _testRepository.GetTestByCourseItemId(courseItemId);
            int courseID = _courseItemService.GetCourseItem(courseItemId).CourseId;

            return new TestDTO()
            {
                Id = test.Id,
                Name = test.Name,
                CourseItemId = courseItemId,
                CourseID = courseID
            };
        }

        public async void SaveHistory(TestHistory history, List<TaskHistory> taskHistories, List<AnswerHistory> answerHistories)
        {
            history = await _testHistoryService.SaveTestHistory(history);

            taskHistories = await _taskHistoryService.SaveTasksHistory(history, taskHistories);

            await _answerHistoryService.SaveAnswersHistory(taskHistories, answerHistories);
        }
         
        public async Task<Test> CreateNewTest(string testName, int courseId, IQueryable<CourseItem> sameCourseItems)
        {
            CourseItem newCourseItem = new CourseItem()
            {
                TypeId = _courseItemService.GetItemTypeByName("Test").Id,
                CourseId = courseId,
                DateCreation = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                OrderNumber = sameCourseItems.Count() > 0 ? sameCourseItems.Last().OrderNumber + 1 : 1
            };
            await _courseItemRepository.AddAsync(newCourseItem);

            Test test = new Test()
            {
                Name = testName,
                CourseItemId = newCourseItem.Id
            };
            await _testRepository.AddAsync(test);

            return test;
        }

        public async Task<Test> RenameTest(int testId, string newName)
        {
            Test test = _testRepository.GetTestById(testId);
            test.Name = newName;

            await _testRepository.UpdateAsync(test);

            return test;
        }

        public TestDTO Edit(TestDTO data)
        {
            Test test = _testRepository.GetTestById(data.Id);

            var sameCourseItems = _courseItemRepository.GetAll().Where(x => x.CourseId == data.CourseID).ToList();

            if (test == null)
            {
                CourseItem newCourseItem = new CourseItem()
                {
                    DateCreation = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                    CourseId = data.CourseID,
                    StatusId = (int)e_ItemStatus.OK,
                    TypeId = (int)e_ItemType.Test,
                    OrderNumber = sameCourseItems.Count > 0 ? sameCourseItems.Count + 1 : 1
                };

                _courseItemRepository.Add(newCourseItem);

                test = new Test()
                {
                    Name = data.Name,
                    CourseItemId = newCourseItem.Id,
                };

                _testRepository.Add(test);

                data.CourseItemId = newCourseItem.Id;
                data.Id = test.Id;
            }
            else
            {
                test.Name = data.Name;

                _testRepository.Update(test);
            }

            return data;
        }

        public PassTestResultDTO ApplyTestResult(PassTestDTO data)
        {
            PassTestResultDTO result = new PassTestResultDTO();
            double mark = 0;

            string guid = "5CC5918D-81B7-4A84-BD64-E79FD914EBF7";

            var test = GetTest(data.Id);
            if (test == null)
            {
                throw new Exception("Test wasn`t found");
            }

            Guid userID = Guid.Parse(guid);
            var currentTestTasks = _taskService.GetTasks(test.Id).ToList();
            result.TotalMark = currentTestTasks.Sum(x => x.Mark);
            result.TestID = test.Id;

            TestHistory testHistory = new TestHistory(currentTestTasks.Sum(x => x.Mark), DateTime.Now, test.Id, userID);
            testHistory.TaskHistory = new List<TaskHistory>();

            var tasks = data.Tasks.ToList();

            foreach (var task in tasks)
            {
                var currentTask = currentTestTasks.FirstOrDefault(x => x.Id == task.ID);
                if (currentTask == null)
                {
                    throw new Exception("Task wasn`t found");
                }

                var taskAnswers = _answerService.GetAnswers(task.ID).ToList();
                int correctCount = taskAnswers.Where(x => x.IsCorrect).Count();
                int userCorrectCount = 0;

                TaskHistory taskHistory = new TaskHistory();
                taskHistory.AnswerHistory = new List<AnswerHistory>();
                taskHistory.TestHistory = testHistory;
                taskHistory.TestTask = currentTask;
                taskHistory.UserMark = 0;

                foreach (var answer in task.Answers)
                {
                    AnswerHistory answerHistory = new AnswerHistory();
                    var currentAnswer = taskAnswers.FirstOrDefault(x => x.Id == answer.ID);

                    answerHistory.TaskId = task.ID;
                    answerHistory.AnswerId = answer.ID;

                    if (currentAnswer.IsCorrect == true && answer.Checked == true)
                    {
                        answerHistory.IsCorrect = true;
                        taskHistory.UserMark += currentTask.Mark / correctCount;
                    }
                    else
                    {
                        answerHistory.IsCorrect = false;
                    }

                    taskHistory.AnswerHistory.Add(answerHistory);
                }

                testHistory.Mark += taskHistory.UserMark;
                testHistory.TaskHistory.Add(taskHistory);
            }

            result.Mark = testHistory.Mark;
            _testHistoryRepository.Add(testHistory);

            return result;
        }
    }
}
