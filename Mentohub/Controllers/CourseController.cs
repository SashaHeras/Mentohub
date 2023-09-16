using Mentohub.Core.Context;
using Mentohub.Core.Services.Services;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Entities;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Mentohub.Controllers
{
    public class CourseController : Controller
    {
        private readonly ProjectContext _context;

        private readonly CourseService _courseService;
        private readonly LessonService _lessonService;
        private readonly MediaService _mediaService;
        private readonly CourseItemService _courseItemService;

        public CourseController(ProjectContext projectContext, 
            CourseService service, 
            LessonService lessonService,
            MediaService mediaService, 
            CourseItemService courseItemService)
        {
            _context = projectContext;
            _courseService = service;  
            _courseItemService = courseItemService;
            _lessonService = lessonService;
            _mediaService = mediaService;
        }

        public IActionResult Index(int id)
        {
            var course = _courseService.GetCourse(id);
            return View(course);
        }

        [Route("/Course/Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var course = _courseService.GetCourse(id);
            return View(course);
        }

        [HttpGet]
        public JsonResult GetElements()
        {
            var courseId = Convert.ToInt32(Request.Form["id"]);
            var elements = _courseItemService.GetElementsByCourseId(courseId);

            return Json(elements);
        }

        [HttpGet]
        public JsonResult GetElementName()
        {
            int courseItemId = Convert.ToInt32(Request.Form["elementId"]);
            var element = _courseItemService.GetCourseItem(courseItemId);
            var typeName = _courseItemService.GetItemType(element.TypeId).Name;

            return Json(element, typeName);
        }

        [HttpPost]
        public JsonResult GetCourseElements()
        {
            var course = Request.Form["course"];
            var result = _courseService.GetCourseElements(Convert.ToInt32(course));
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> SaveCource()
        {
            int courseID = await _courseService.SaveCource(Request.Form);
            return Json(courseID);
        }

        [HttpDelete]
        [Route("/Course/DeleteCourseItem/{courseItemId}/{typeId}")]
        public async Task<JsonResult> DeleteCourseItem(int courseItemId, int typeId)
        {          
            try
            {
                var data = await _courseItemService.DeleteCourseItem(courseItemId, typeId);
                return Json(new { IsError = false, Data = data, Message = "" });
            }
            catch(Exception ex)
            {
                return Json(new { IsError = true, Message = ex.Message });
            }
        }

        [HttpGet]
        [Route("/Course/Comments/{courseId}/{commentsCount}")]
        public JsonResult GetComments(int courseId, int commentsCount = 10)
        {
            var data = _courseService.GetCourseComments(courseId, commentsCount);
            return Json(data);
        }

        [HttpPost]
        public IActionResult LoadPartialPage(string type, string courseItemId)
        {
            if (type == "1")
            {

            }

            int itemId = Convert.ToInt32(courseItemId);
            int courseId = _courseItemService.GetCourseItem(itemId).CourseId;

            CourseDTO c = _courseService.GetCourse(courseId);
            Lesson l = _lessonService.GetLessonByCourseItem(itemId);

            LessonDTO lessonPartial = new LessonDTO()
            {
                Id = l.Id,
                Theme = l.Theme,
                Description = l.Description,
                VideoPath = l.VideoPath,
                Body = l.Body,
                DateCreation = l.DateCreation,
                CourseItemId = l.CourseItemId,
                CourseID = c.Id,
                CourseRating = c.Rating
            };

            return PartialView("~/Views/Partial/_LessonPartial.cshtml", lessonPartial);
        }        
    }
}
