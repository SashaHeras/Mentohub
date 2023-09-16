using Mentohub.Core.Context;
using Mentohub.Core.Services.Services;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Mentohub.Controllers
{
    public class LessonController : Controller
    {
        private LessonService _lessonService;
        private AzureService _azureService;
        private MediaService _mediaService;
        private CourseItemService _courseItemService;
        private readonly CourseService _courseService;

        public LessonController(LessonService lessonService, AzureService azureService, MediaService mediaService, 
            CourseItemService courseItemService, CourseService courseService)
        {
            _lessonService = lessonService;
            _azureService = azureService;
            _mediaService = mediaService;
            _courseItemService = courseItemService;
            _courseService = courseService;
        }

        /// <summary>
        /// Main page
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Page of lesson creation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("/Lesson/CreateLesson/{id}")]
        public IActionResult CreateLesson(int id)
        {
            ViewBag.CourseId = id;
            return View();
        }

        /// <summary>
        /// Method of lesson creation
        /// </summary>
        /// <param name="form"></param>
        /// <param name="createLessonModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(IFormCollection form, LessonDTO createLessonModel)
        {
            string videoName = await _azureService.SaveInAsync(createLessonModel.VideoFile);
            CourseItem newCourceItem = await _courseItemService.CreateNewCourseItem(createLessonModel);
            _lessonService.CreateLesson(newCourceItem, createLessonModel, videoName);

            return RedirectToAction("CreateCourse", "Course", new { id = newCourceItem.CourseId });
        }

        [Route("/Lesson/GoToLesson/{id}")]
        public IActionResult GoToLesson(int id)
        {
            Guid lessonGuid = _lessonService.GetLessonByCourseItem(id).Id;
            return RedirectToAction("Lesson", new { id = lessonGuid });
        }

        [Route("/Lesson/Lesson/{id}")]
        public IActionResult Lesson(Guid id)
        {
            var data = _lessonService.GetLesson(id);
            return View(data);
        }

        [HttpPost]
        public JsonResult LessonJson(Guid id)
        {            
            try
            {
                var data = _lessonService.GetLesson(id);
                return Json(new { IsError = false, Data = data, Message = "" });
            }
            catch (Exception ex)
            {
                return Json(new { IsError = true, Message = ex.Message });
            }
        }

        [Route("/Lesson/EditLesson/{id}")]
        public IActionResult EditLesson(int id)
        {
            var data = _lessonService.GetLessonByCourseItem(id);
            return View(data);  
        }

        [Route("/Lesson/Edit")]
        public async Task<IActionResult> Edit(IFormCollection form, Lesson lesson)
        {
            var data = _lessonService.Edit(form, lesson);
            return RedirectToAction("CreateCourse", "Course", new { id = data });
        }
    }
}
