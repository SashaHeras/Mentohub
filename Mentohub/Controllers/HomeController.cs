using Mentohub.Core.Repositories.Repositories;
using Mentohub.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Mentohub.Controllers
{
    public class HomeController : Controller
    {
        private LessonRepository _lessonRepository;
        private CourseRepository _courseRepository;

        public HomeController(LessonRepository lessonRepository, CourseRepository courseRepository)
        {
            _lessonRepository = lessonRepository;
            _courseRepository = courseRepository;
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

        [Route("/Home/Lesson/{id}")]
        public IActionResult Lesson(Guid id)
        {
            ViewBag.Lesson = _lessonRepository.GetLessonById(id);

            // ReSharper disable once Mvc.ViewNotResolved
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}