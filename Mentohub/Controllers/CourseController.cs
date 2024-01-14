using Mentohub.Core.Context;
using Mentohub.Core.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Mentohub.Core.Services.Interfaces;
using Microsoft.Azure.Amqp.Framing;
using Mentohub.Domain.Data.DTO.CourseDTOs;
using Microsoft.AspNetCore.Cors;
using Mentohub.Domain.Filters;
using Mentohub.Domain.Data.DTO.ResultDTO;
using static MassTransit.ValidationResultExtensions;

namespace Mentohub.Controllers
{
    [EnableCors("MentoPolicy")]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly ICourseLevelService _courseLevelService;
        private readonly ICourseSubjectService _courseSubjectService;
        private readonly ICourseLanguageService _courseLanguageService;        

        public CourseController(
            ICourseService service,
            ICourseLevelService courseLevelService,
            ICourseSubjectService courseSubjectService,
            ICourseLanguageService courseLanguageService
        )
        {
            _courseService = service;
            _courseLevelService = courseLevelService;
            _courseSubjectService = courseSubjectService;
            _courseLanguageService = courseLanguageService;
        }

        /// <summary>
        /// Create/edit course
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Apply(CourseDTO data)
        {
            try
            {
                var course = _courseService.Apply(data);
                return Json(new { IsError = false, Data = course, Message = "Success" });
            }
            catch (Exception ex)
            {
                return Json(new { IsError = true, Message = ex.Message });
            }
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

        /// <summary>
        /// Пошук курсів
        /// </summary>
        /// <param name="filterModel"></param>
        /// <returns></returns>
        [Route("Course/Table")]
        [HttpPost]
        public JsonResult Table([FromBody] SearchFilterModel filterModel)
        {
            try
            {
                var result = new SearchCourseResult();
                result.Courses = _courseService.List(filterModel, out int totalCount);
                result.TotalCount = totalCount;

                return Json(new { IsError = false, Data = result });
            }
            catch(Exception ex)
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
        /// Get list of categories
        /// </summary>
        /// <returns></returns>
        [Route("Course/GetCategoriesList")]
        [HttpGet]
        public JsonResult GetCategoriesList()
        {
            return Json(_courseSubjectService.SubjectsList());
        }

        /// <summary>
        /// Get list of languages to course
        /// </summary>
        /// <returns></returns>
        [Route("Course/GetLanguagesList")]
        [HttpGet]
        public JsonResult GetLanguagesList()
        {
            return Json(_courseLanguageService.GetLanguagesList());
        }


        /// <summary>
        /// Метод отримання списку рівнів
        /// </summary>
        /// <returns></returns>
        [Route("Course/GetLevelsList")]
        [HttpGet]
        public JsonResult GetLevelsList()
        {
            var levels = _courseLevelService.GetLevelsList();
            return Json(levels);
        }
    }
}
