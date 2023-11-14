using Mentohub.Core.Context;
using Mentohub.Core.Services.Services;
using Mentohub.Domain.Data.DTO;
using Microsoft.AspNetCore.Mvc;
using Mentohub.Core.Services.Interfaces;

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
        /// <param name="courseId"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetCourseElements(int courseId)
        {
            var result = _courseService.GetCourseElements(courseId);
            return Json(result);
        }

        /// <summary>
        /// Get course comments
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="commentsCount"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetComments(int courseId, int commentsCount = 10)
        {
            var data = _courseService.GetCourseComments(courseId, commentsCount);
            return Json(data);
        }

        /// <summary>
        /// Edit/create comment to course
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EditComment([FromBody] CommentDTO data)
        {
            try
            {
                var comment = _courseService.EditComment(data);
                return Json(new { IsError = false, Data = comment, Message = "Success" });
            }
            catch (Exception ex)
            {
                return Json(new { IsError = true, Message = ex.Message });
            }
        }
    }
}
