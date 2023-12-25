using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Core.Repositories.Repositories;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
            ViewBag.Courses = _courseRepository.GetAll();
            return View();
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

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetMostFamoustCourseList()
        {
            var courseList = _courseService.MostFamoustList();
            return Json(courseList);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}