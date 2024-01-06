using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Core.Repositories.Interfaces;
using Mentohub.Core.Repositories.Interfaces.CourseInterfaces;
using Mentohub.Core.Repositories.Repositories;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.DTO.Test;
using Mentohub.Domain.Data.Entities.CourseEntities;
using Mentohub.Domain.Data.Enums;
using Mentohub.Domain.Entities;
using Mentohub.Domain.Helpers;
using System.Threading.Tasks;

namespace Mentohub.Core.Services.Services
{
    public class TestService : ITestService
    {
        private readonly ITestRepository _testRepository;
        private readonly ICRUD_UserRepository _userRepository;
        private readonly ICourseItemRepository _courseItemRepository;
        private readonly ITestHistoryRepository _testHistoryRepository;
        private readonly ICourseBlockRepository _courseBlockRepository;

        private readonly ITaskService _taskService;
        private readonly IAnswerService _answerService;
        private readonly ICourseItemService _courseItemService;
        private readonly ITestHistoryService _testHistoryService;
        private readonly ITaskHistoryService _taskHistoryService;
        private readonly IAnswerHistoryService _answerHistoryService;

        public TestService( 
            ICourseItemService courseItemService, 
            ITestHistoryService testHistoryService,
            ITaskHistoryService taskHistoryService, 
            IAnswerHistoryService answerHistoryService,
            ITaskService taskService,
            IAnswerService answerService,
            ITestHistoryRepository testHistoryRepository,
            ICourseBlockRepository courseBlockRepository,
            ITestRepository testRepository,
            ICRUD_UserRepository userRepository,
            ICourseItemRepository courseItemRepository)
        {
            _testRepository = testRepository;
            _courseItemRepository = courseItemRepository;
            _courseItemService = courseItemService;
            _testHistoryService = testHistoryService;
            _taskHistoryService = taskHistoryService;
            _answerHistoryService = answerHistoryService;
            _testHistoryRepository = testHistoryRepository;
            _taskService = taskService;
            _answerService = answerService;
            _userRepository = userRepository;
            _courseBlockRepository = courseBlockRepository;
        }             

        public Test GetTest(int id)
        {
            return _testRepository.GetById(id);
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

        public TestDTO Apply(TestDTO data)
        {
            Test test = _testRepository.GetById(data.Id);
            var block = _courseBlockRepository.GetById(data.CourseBlockID) ?? throw new Exception("Anknown block!");

            var sameCourseItems = _courseItemRepository.GetAll()
                                                       .Where(x => x.CourseId == data.CourseID)
                                                       .ToList();
     
            if (test == null)
            {
                CourseItem newCourseItem = new()
                {
                    DateCreation = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                    CourseId = data.CourseID,
                    StatusId = (int)e_ItemStatus.OK,
                    TypeId = (int)e_ItemType.Test,
                    OrderNumber = sameCourseItems.Count > 0 ? sameCourseItems.Count + 1 : 1,
                    CourseBlockID = data.CourseBlockID
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

        public async Task<PassTestResultDTO> ApplyTestResult(PassTestDTO data)
        {
            var test = GetTest(data.Id) ?? throw new Exception("Test wasn`t found");

            var currentUserID = MentoShyfr.Decrypt(data.UserID);
            var currentUser = await _userRepository.FindCurrentUserById(currentUserID) ?? throw new Exception("User not found");

            var currentTestTasks = _taskService.GetTasks(test.Id).ToList();

            PassTestResultDTO result = new PassTestResultDTO() {
                TotalMark = currentTestTasks.Sum(x => x.Mark),
                TestID = test.Id
            };

            TestHistory testHistory = new TestHistory(
                currentTestTasks.Sum(x => x.Mark), 
                DateTime.Now, 
                test.Id, 
                currentUserID);

            testHistory.TaskHistory = new List<TaskHistory>();

            var tasks = data.Tasks.ToList();

            foreach (var task in tasks)
            {
                var currentTask = currentTestTasks.FirstOrDefault(x => x.Id == task.ID) 
                                                   ?? throw new Exception("Task wasn`t found");

                var taskAnswers = _answerService.GetAnswers(task.ID).ToList();
                if(taskAnswers.Count == 0)
                {
                    throw new Exception("Answers weren`t found");
                }

                int correctCount = taskAnswers.Where(x => x.IsCorrect).Count();
                TaskHistory taskHistory = new TaskHistory()
                {
                    TestHistory = testHistory,
                    TestTask = currentTask,
                    UserMark = 0,
                    AnswerHistory = new List<AnswerHistory>()
                };

                foreach (var answer in task.Answers)
                {
                    var currentAnswer = taskAnswers.FirstOrDefault(x => x.Id == answer.ID);

                    AnswerHistory answerHistory = new AnswerHistory() {
                        TaskId = task.ID,
                        AnswerId = answer.ID
                    };

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
