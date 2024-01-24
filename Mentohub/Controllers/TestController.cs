using Mentohub.Core.Services.Interfaces;
using Mentohub.Domain.Data.DTO;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

namespace Mentohub.Controllers
{
    [EnableCors("AllowAll")]
    public class TestController : Controller
    {
        private readonly ITestService _testService;
        private readonly ITaskService _taskService;
        private readonly IAnswerService _answerService;

        public TestController(
            ITestService testService, 
            ITaskService taskService,
            IAnswerService answerService
        )
        {
            _testService = testService;
            _taskService = taskService;
            _answerService = answerService;
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
        public JsonResult Apply([FromBody] TestDTO data)
        {
            try
            {
                var newTest = _testService.Apply(data);

                return Json(new { IsError = false, Data = newTest, Message = "Success" });
            }
            catch (Exception ex)
            {
                return Json(new { IsError = true, Message = ex.Message });
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

                return Json(new { IsError = false, Data = newTask, Message = "Success" });
            }
            catch (Exception ex)
            {
                return Json(new { IsError = true, Message = ex.Message });
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
                return Json(new { IsError = true, ex.Message });
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
        public async Task<JsonResult> Pass([FromBody] PassTestDTO data)
        {
            using(TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var result = await _testService.ApplyTestResult(data);
                    scope.Complete();
                    return Json(new { IsError = true, Data = result, Message = "Success" });
                }
                catch (Exception ex)
                {
                    return Json(new { IsError = true, ex.Message });
                }
            }           
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

        /// <summary>
        /// Delete task answer
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
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
