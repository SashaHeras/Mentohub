using Mentohub.Core.Context;
using Mentohub.Core.Services.Services;
using Mentohub.Domain.Data.DTO;
using Microsoft.AspNetCore.Mvc;
using Mentohub.Core.Services.Interfaces;
using Microsoft.Azure.Amqp.Framing;

namespace Mentohub.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService service)
        {
            _courseService = service;  
        }

        /// <summary>
        /// Create/edit course
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Edit(CourseDTO data)
        {
            try
            {
                var course = _courseService.Edit(data);
                return Json(new { IsError = false, Data = course, Message = "Success" });
            }
            catch (Exception ex)
            {
                return Json(new { IsError = true, Message = ex.Message });
            }
        }

        /// <summary>
        /// Get course elements (tests/lessons)
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetCourseElements(int Id)
        {
            var result = _courseService.GetCourseElements(Id);
            return Json(result);
        }

        /// <summary>
        /// View course
        /// </summary>
        /// <param name="CourseID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ViewCourse(int CourseID, string UserID)
        {
            try
            {
                var course = await _courseService.ViewCourse(CourseID, UserID);
                return Json(new { IsError = false, Data = course, Message = "Success" });
            }
            catch (Exception ex)
            {
                return Json(new { IsError = true, Message = ex.Message });
            }
        }

        /// <summary>
        /// Get list about what course contains
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetCourseInfoList(int ID)
        {
            try
            {
                return Json(_courseService.GetCourseInfoList(ID));
            }
            catch(Exception ex)
            {
                return Json(new { IsError = true, Message = ex.Message });
            }
        }
    }
}
