using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Core.Repositories.Repositories;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Mentohub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly ICourseRepository _courseRepository;

        private readonly ICourseService _courseService;

        public HomeController(
            ILessonRepository lessonRepository,
            ICourseRepository courseRepository,
            ICourseService courseService
            )
        {
            _lessonRepository = lessonRepository;
            _courseRepository = courseRepository;
            _courseService = courseService;
        }

        public IActionResult Index()
        {
            var list = _courseRepository.GetAll().ToList();
            return View(list);
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