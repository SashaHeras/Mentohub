using Mentohub.Core.Services.Interfaces;
using Mentohub.Domain.Data.DTO.CourseDTOs;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using static MassTransit.ValidationResultExtensions;

namespace Mentohub.Controllers
{
    [EnableCors("AllowAll")]
    public class CourseOverviewController : Controller
    {
        private readonly IOverviewService _overviewService;

        public CourseOverviewController(
            IOverviewService overviewService
        )
        {
            _overviewService = overviewService;
        }

        /// <summary>
        /// Create/update course overview
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Apply([FromBody] CourseOverviewDTO data)
        {
            try
            {
                var result = _overviewService.Apply(data);
                return Json(new { IsError = false, Data = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                return Json(new { IsError = true, Message = ex.Message });
            }
        }

        /// <summary>
        /// Get list of course overviews
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetCourseOverviews(int ID)
        {
            try
            {
                var result = _overviewService.GetCourseOverviews(ID);
                return Json(new { IsError = false, Data = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                return Json(new { IsError = true, Message = ex.Message });
            }
        }
    }
}
