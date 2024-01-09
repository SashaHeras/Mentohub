using Mentohub.Core.Context;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Core.Services.Services;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Mentohub.Controllers
{
    [EnableCors("MentoPolicy")]
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
        /// Edit/create lesson
        /// </summary>
        /// <param name="lesson"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> Apply(LessonDTO lesson)
        {
            try
            {
                var data = await _lessonService.Apply(lesson);

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
