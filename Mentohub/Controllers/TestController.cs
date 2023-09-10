using Mentohub.Core.Context;
using Mentohub.Core.Services.Services;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Mentohub.Controllers
{
    public class TestController : Controller
    {
        private TestService _testService;
        private TaskService _taskService;
        private AnswerService _answerService;
        private CourseItemService _courseItemService;

        public TestController(TestService testService, TaskService taskService,
            AnswerService answerService, CourseItemService courseItemService)
        {
            _testService = testService;
            _taskService = taskService;
            _answerService = answerService;
            _courseItemService = courseItemService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("/Test/GoToTest/{courseItemId}")]
        public IActionResult GoToTest(int courseItemId)
        {
            int testId = _testService.GetTestByCourseItem(courseItemId).Id;
            return RedirectToAction("PassTest", new { id = testId });
        }

        [Route("/Test/PassTest/{id}")]
        public IActionResult PassTest(int id)
        {
            var test = _testService.GetTest(id);
            var courseItemId = test.CourseItemId;
            var courseId = _courseItemService.GetCourseItem(courseItemId).CourseId;

            ViewBag.CourseId = courseId;
            ViewBag.Test = test;

            return View();
        }

        public JsonResult GetTasks()
        {
            int testId = Convert.ToInt32(Request.Form["test"]);
            var tasks = _taskService.GetTasks(testId);

            return Json(tasks);
        }

        public JsonResult GetAnswers()
        {
            int taskId = Convert.ToInt32(Request.Form["task"]);
            var testAnswers = _answerService.GetAnswers(taskId);
            return Json(testAnswers);
        }

        [HttpPost]
        public JsonResult GetMark()
        {
            double total = 0;
            double mark = 0;

            int testId = int.Parse(Request.Form["test"]);
            int userId = int.Parse(Request.Form["userid"]);
            List<TaskHistory> taskHistories = new List<TaskHistory>();
            List<AnswerHistory> answerHistories = new List<AnswerHistory>();

            var tasks = _taskService.GetTasks(testId);
            var taskAnswers = Request.Form["answers"].ToString().Split(',')
                                .Select(a => new AnswerDTO {
                                    TaskId = int.Parse(a.Split('_')[0]),
                                    AnswerId = int.Parse(a.Split('_')[1]),
                                    IsChecked = bool.Parse(a.Split('_')[2])
                                }).ToList();

            foreach (var task in tasks)
            {
                double markForTask = 0;
                var taskAnswerIds = taskAnswers.Where(a => a.TaskId == task.Id).ToList();
                var corectAnswersCount = _answerService.PopulateAnswerHistories(taskAnswerIds, task, answerHistories);
                int corAnsCnt = _answerService.GetCountOfCorrectAnswers(task.Id);

                if (corAnsCnt == corectAnswersCount) {
                    markForTask = task.Mark;
                }
                else if (corAnsCnt > corectAnswersCount) {
                    double perOne = task.Mark / corAnsCnt;
                    markForTask = task.Mark / (perOne * corectAnswersCount);
                }

                mark += markForTask;
                total += task.Mark;
                taskHistories.Add(new TaskHistory {
                    TaskId = task.Id,
                    UserMark = markForTask
                });
            }

            var testHistory = new TestHistory
            {
                TestId = testId,
                UserId = userId,
                TotalMark = total,
                Mark = mark
            };
            _testService.SaveHistory(testHistory, taskHistories, answerHistories);

            return Json(mark);
        }

        [Route("/Test/CreateTest/{id}")]
        public IActionResult CreateTest(int id)
        {
            ViewBag.CourseId = id;
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> SaveTest()
        {
            CourseItem currecntCourceItem = new CourseItem();
            int courseId = Convert.ToInt32(Request.Form["courseId"]);
            var sameCourseItems = _courseItemService.GetCourseItems(courseId);

            int? testId = Request.Form.Keys.Contains("testId")
                ? Convert.ToInt32(Request.Form["testId"])
                : (int?)null;

            string newName = Request.Form["test"].ToString();

            Test test = testId.HasValue
                ? await _testService.RenameTest(testId.Value, newName)
                : await _testService.CreateNewTest(newName, courseId, sameCourseItems);

            return Json(test.Id);
        }

        [HttpPost]
        public async Task<JsonResult> SaveAnswers()
        {
            var allAnsws = Request.Form["answers"];
            var allChecked = Request.Form["checked"];
            Dictionary<string, bool> answers = _answerService.AnswersSpliter(allAnsws, allChecked);

            //
            TestTask tt = _taskService.CreateNewTask(
                Request.Form["taskName"].ToString(),
                Convert.ToInt32(Request.Form["orderNumber"]),
                Convert.ToInt32(Request.Form["taskMark"]),
                Convert.ToInt32(Request.Form["testId"])
            ).Result;

            foreach (var answer in answers)
            {
                TaskAnswer newAnswer = new TaskAnswer()
                {
                    Name = answer.Key,
                    IsCorrect = answer.Value,
                    TaskId = tt.Id
                };

                _answerService.AddAnswer(newAnswer);
            }

            return Json(true);
        }

        [Route("/Test/EditTest/{id}")]
        public IActionResult EditTest(int id)
        {
            ViewBag.Test = _testService.GetTestByCourseItem(id);
            ViewBag.CourseId = _courseItemService.GetCourseItem(id).CourseId;
            return View();
        }

        [HttpGet]
        [Route("/Test/GetTasks/{id}")]
        public JsonResult GetTasks(int id)
        {
            var result = _taskService.GetTasks(id);
            return Json(result);
        }

        [HttpGet]
        [Route("/Test/GetTask/{id}")]
        public JsonResult GetTask(int id)
        {
            var res = _taskService.GetTask(id);
            return Json(res);
        }

        [Route("/Test/GetAnswersForEditting/{id}")]
        public JsonResult GetAnswersForEditting(int id)
        {
            var result = _answerService.GetAnswers(id);

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> SaveEdittedAnswers()
        {
            int taskId = Convert.ToInt32(Request.Form["taskId"]);
            TestTask editedTask = _taskService.GetTask(taskId);
            editedTask.Name = Request.Form["taskName"].ToString();
            editedTask.Mark = Convert.ToInt32(Request.Form["taskMark"].ToString());

            var allAnsws = Request.Form["answers"];
            var allChecked = Request.Form["checked"];
            Dictionary<string, bool> answers = _answerService.AnswersSpliter(allAnsws, allChecked);
            string[] ids = Request.Form["ids"].ToString().Split(',');

            await _answerService.SavingAnswers(editedTask, answers, ids);

            await _taskService.UpdateTask(editedTask);

            return Json(true);
        }

        [HttpDelete]
        [Route("/Test/DeleteTask/{taskId}")]
        public JsonResult DeleteTask(int taskId)
        {
            var task = _taskService.GetTask(taskId);            
            int orderNumber = task.OrderNumber;
            int testId = task.TestId;
            var allTasksAfter = _taskService.GetTasksAfter(testId, orderNumber);

            try
            {
                _taskService.DeleteTask(task);
                _taskService.ResetOrderNumbers(orderNumber, allTasksAfter);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

            return Json(true);
        }

        [HttpPost]
        public JsonResult DeleteAnswer()
        {
            var answerId = Convert.ToInt32(Request.Form["answerId"]);
            var answer = _answerService.GetAnswer(answerId);
            var taskId = answer.TaskId; 

            if (answer != null)
            {
                _answerService.RemoveAnswer(answer);

                var answers = _answerService.GetAnswers(taskId);

                return Json(answers);
            }

            return Json(false);
        }
    }
}
