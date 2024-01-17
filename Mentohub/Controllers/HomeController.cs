using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Core.Repositories.Interfaces.CourseInterfaces;
using Mentohub.Core.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Mentohub.Controllers
{
    [EnableCors("MentoPolicy")]
    public class HomeController : Controller
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly ICourseRepository _courseRepository;

        private readonly ICourseService _courseService;
        private readonly IAzureService _azureService;

        public HomeController(
            ILessonRepository lessonRepository,
            ICourseRepository courseRepository,
            ICourseService courseService,
            IAzureService azureService
            )
        {
            _lessonRepository = lessonRepository;
            _courseRepository = courseRepository;
            _courseService = courseService;
            _azureService = azureService;
        }

        public IActionResult Index()
        {
            var videoBytes = ConvertIFormFileToBytes("C:\\Users\\acsel\\source\\repos\\Mentohub\\Mentohub\\125.mp4");

            // Convert the byte array to a base64 string
            var base64Video = Convert.ToBase64String(videoBytes);

            ViewBag.Base64Video = base64Video;
            ViewBag.VideoLength = videoBytes.Length;

            return View();
        }

        public JsonResult Load()
        {
            return Json(true);
        }

        // Helper method to convert IFormFile to byte array
        private byte[] ConvertIFormFileToBytes(string fileName)
        {
            using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                // Create a MemoryStream to store the file contents
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    // Copy the file contents to the MemoryStream
                    fileStream.CopyTo(memoryStream);

                    return memoryStream.ToArray();
                }
            }
        }

        public IActionResult Video()
        {
            return View();
        }

        public IActionResult Lesson(Guid id)
        {
            var lesson = _lessonRepository.GetLessonById(id);
            return View(lesson);
        }

        [HttpGet]
        public JsonResult GetVersion()
        {
            var currentAssamblyInfo = Assembly.GetExecutingAssembly();
            var appVersion = currentAssamblyInfo.GetName().Version;
            return Json(appVersion);
        }

        [HttpGet]
        public JsonResult GetMostFamoustCourseList()
        {
            var courseList = _courseService.MostFamoustList();
            return Json(courseList);
        }
    }
}