using Mentohub.Core.Context;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Core.Services.Services;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Mentohub.Controllers
{
    public class LessonController : Controller
    {
        private readonly ILessonService _lessonService;
        private readonly IAzureService _azureService;
        private readonly IMediaService _mediaService;
        private readonly ICourseItemService _courseItemService;
        private readonly ICourseService _courseService;

        public LessonController( 
            ILessonService lessonService, 
            IAzureService azureService, 
            IMediaService mediaService, 
            ICourseItemService courseItemService, 
            ICourseService courseService)
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
        [HttpPost]
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
            //var data = await _lessonService.Create(form, createLessonModel);
            return RedirectToAction("CreateCourse", "Course", new { id = 0 });
        }

        [HttpGet]
        public IActionResult GoToLesson(int id)
        {
            Guid lessonGuid = _lessonService.GetLessonByCourseItem(id).Id;
            return RedirectToAction("Lesson", new { id = lessonGuid });
        }

        [HttpGet]
        public IActionResult Lesson(Guid id)
        {
            var data = _lessonService.GetLesson(id);
            return View(data);
        }

        [HttpGet]
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

        [HttpGet]
        [Route("/Lesson/EditLesson/{id}")]
        public IActionResult EditLesson(int id)
        {
            LessonDTO lesson = _lessonService.GetLessonByCourseItem(id);

            return View(lesson);  
        }

        //[HttpGet]
        //[Route("/Lesson/Edit")]
        //public async Task<IActionResult> Edit(IFormCollection form, Lesson lesson)
        //{
        //    string videoPath = String.Empty;
        //    string oldVideoName = String.Empty;

        //    if (form.Files.Count != 0)
        //    {
        //        await _azureService.DeleteFromAzure(lesson.VideoPath);
        //        videoPath = _azureService.SaveInAsync(form.Files[0]).Result;
        //        lesson.VideoPath = videoPath;
        //    }
        //    else
        //    {
        //        oldVideoName = _lessonService.GetLesson(lesson.Id).VideoPath;
        //        lesson.VideoPath = oldVideoName;
        //    }

        //    CourseItem currentCourseItem = _courseItemService.GetCourseItem(lesson.CourseItemId);
        //    currentCourseItem.DateCreation = DateTime.Now;

        //    int courseId = currentCourseItem.CourseId;
        //    await _courseItemService.UpdateCourseItem(currentCourseItem);  
            
        //    _lessonService.UpdateLesson(lesson);

        //    _mediaService.DeleteMediaFromProject(form.Files[0]);

        //    return RedirectToAction("CreateCourse", "Course", new { id = courseId });
        //}

        /// <summary>
        /// Edit/create lesson
        /// </summary>
        /// <param name="lesson"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> Edit(LessonDTO lesson)
        {
            try
            {
                var data = await _lessonService.Edit(lesson);

                return Json(new { IsError = false, Data = data, Message = "Success" });
            }
            catch (Exception ex)
            {
                return Json(new { IsError = true, Message = ex.Message });
            }
        }

        /// <summary>
        /// Delete lesson
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public JsonResult Delete(string id)
        {
            try
            {
                _lessonService.Delete(Guid.Parse(id));

                return Json(new { IsError = false, Message = "Success" });
            }
            catch (Exception ex)
            {
                return Json(new { IsError = true, Message = ex.Message });
            }
        }
    }
}
