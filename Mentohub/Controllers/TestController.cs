using Antlr.Runtime.Tree;
using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Core.Services.Services;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Amqp.Framing;
using System.Linq.Expressions;

namespace Mentohub.Controllers
{
    public class TestController : Controller
    {
        private readonly ITestService _testService;
        private readonly ITaskService _taskService;
        private readonly IAnswerService _answerService;
        private readonly ICourseItemService _courseItemService;

        private readonly ITestHistoryRepository _testHistoryRepository;
        private readonly IAnswerHistoryRepository _answerHistoryRepository;
        private readonly ITaskHistoryRepository _taskHistoryRepository;

        public TestController(
            ITestService testService, 
            ITaskService taskService,
            IAnswerService answerService, 
            ICourseItemService courseItemService,
            ITestHistoryRepository testHistoryRepository,
            IAnswerHistoryRepository answerHistoryRepository,
            ITaskHistoryRepository taskHistoryRepository)
        {
            _testService = testService;
            _taskService = taskService;
            _answerService = answerService;
            _courseItemService = courseItemService;
            _testHistoryRepository = testHistoryRepository;
            _answerHistoryRepository = answerHistoryRepository;
            _taskHistoryRepository = taskHistoryRepository;
        }

        [HttpPost]
        public JsonResult GetAnswers(int ID)
        {
            var testAnswers = _answerService.GetAnswers(ID);
            return Json(testAnswers);
        }

        #region Pass test

        /// <summary>
        /// Create test
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Edit([FromBody] TestDTO data)
        {
            try
            {
                var newTest = _testService.Edit(data);

                return Json(new { IsError = false, Data = newTest, Message = "Success" });
            }
            catch (Exception ex)
            {
                return Json(new { IsError = true, Message = "Error" });
            }
        }

        /// <summary>
        /// Edit task
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EditTask([FromBody] TaskDTO data)
        {
            try
            {
                var newTask = _taskService.Edit(data);

                return Json(new { IsError = false, Data = newTask.Id, Message = "Success" });
            }
            catch (Exception ex)
            {
                return Json(new { IsError = true, Message = "Error" });
            }
        }

        /// <summary>
        /// Save answers to task
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EditAnswers([FromBody] List<AnswerDTO> data)
        {
            try
            {
                var result = _answerService.EditAnswers(data);

                return Json(new { IsError = false, Data = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                return Json(new { IsError = true, Message = "Error" });
            }
        }

        #endregion

        /// <summary>
        /// Get list of test tasks
        /// </summary>
        /// <param name="testId"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetTestTasks(int testId)
        {
            var tasks = _taskService.GetTasksList(testId);
            return Json(tasks);
        }

        /// <summary>
        /// Get list of answers on task
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetTaskAnswers(int taskId)
        {
            var answers = _answerService.GetAnswersList(taskId);
            return Json(answers);
        }

        /// <summary>
        /// Apply results of test
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> Apply([FromBody] PassTestDTO data)
        {
            try
            {
                var result = await _testService.ApplyTestResult(data);

                return Json(new { IsError = true, Data = result, Message = "Success" });
            }
            catch(Exception ex)
            {
                return Json(new { IsError = true, Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> SaveTest(int courseID, int? testID, string testName)
        {
            var sameCourseItems = _courseItemService.GetCourseItems(courseID);

            Test test = new Test();
            if (testID != null)
            {
                test = await _testService.RenameTest(testID.Value, testName);
            }
            else
            {
                test = await _testService.CreateNewTest(testName, courseID, sameCourseItems);
            }

            return Json(test.Id);
        }

        public IActionResult EditTest(int id)
        {
            var data = _testService.GetTestModel(id);
            return View(data);
        }

        [HttpGet]
        public JsonResult GetTasks(int id)
        {
            var result = _taskService.GetTasks(id);
            return Json(result);
        }

        [HttpGet]
        public JsonResult GetTask(int ID)
        {
            var res = _taskService.GetTask(ID);
            return Json(res);
        }

        [HttpGet]
        public JsonResult GetAnswersForEditting(int id)
        {
            var result = _answerService.GetAnswers(id);
            return Json(result);
        }

        [HttpDelete]
        public JsonResult DeleteTask(int ID)
        {
            try
            {
                _taskService.DeleteTask(ID);
                return Json(new { IsError = false, Message = "Success" });
            }
            catch (Exception ex)
            {
                return Json(new { IsError = true, Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult DeleteAnswer(int ID)
        {
            try
            {
                _answerService.DeleteAnswer(ID);
                return Json(new { IsError = false, Message = "Success" });
            }
            catch (Exception ex)
            {
                return Json(new { IsError = true, Message = ex.Message });
            }
        }
    }
}
