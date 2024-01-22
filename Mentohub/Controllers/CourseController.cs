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
using Mentohub.Core.Services.Interfaces.CourseInterfaces;

namespace Mentohub.Controllers
{
    [EnableCors("MentoPolicy")]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IAzureService _azureService;

        public CourseController(
            ICourseService service,
            IAzureService azureService
        )
        {
            _courseService = service;
            _azureService = azureService;
        }

        /// <summary>
        /// Create/edit course
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> Apply(CourseDTO data)
        {
            try
            {
                var course = await _courseService.Apply(data);
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
        /// Generate filters for search course page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetSearchFilters()
        {
            return Json(_courseService.InitSearchFilterData());
        }

        /// <summary>
        /// Get authors top 6 courses
        /// </summary>
        /// <param name="authorID"></param>
        /// <returns></returns>
        [Route("Course/GetAuthorsTopCourses")]
        [HttpPost]
        public JsonResult GetAuthorsTopCourses(string authorID)
        {
            return Json(_courseService.GetAuthorsToCourses(authorID));
        }

        [HttpPost]
        public async Task<JsonResult> GetVideo([FromForm] string name)
        {
            return Json(await _azureService.CopyVideoFromBlob(name));
        }
        /// <summary>
        /// Information about author
        /// </summary>
        /// <param name="encriptId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Course/AuthorInfo")]
        public JsonResult GetAuthorInfo([FromForm] string encriptId)
        {
            try
            {
                var result=_courseService.GetAuthorInfoDTO(encriptId);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { IsError = true, Message = ex.Message });
            }
        }
    }
}