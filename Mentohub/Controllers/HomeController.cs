using LiqPay.SDK;
using LiqPay.SDK.Dto;
using LiqPay.SDK.Dto.Enums;
using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Core.Repositories.Interfaces.CourseInterfaces;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Core.Services.Interfaces.PaymentInterfaces;
using Mentohub.Domain.Data.Entities.Order;
using Mentohub.Domain.Helpers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Mentohub.Controllers
{
    [EnableCors("AllowAll")]
    public class HomeController : Controller
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly ILiqpayService _liqpayService;
        private readonly IConfiguration _config;
        private readonly ICourseService _courseService;
        private readonly IOrderPaymantService _orderPaymantService;
        private readonly IOrderService _orderService;
        private readonly IAzureService _azureService;
        private readonly ICurrencyService _currencyService;
        public HomeController(
            ILessonRepository lessonRepository,
            ICourseService courseService,
            IAzureService azureService,
            ILiqpayService liqpayService,
            IOrderPaymantService orderPaymantService,
            IOrderService orderService,
            IConfiguration config,
            ICurrencyService currencyService
            )
        {
            _lessonRepository = lessonRepository;
            _liqpayService = liqpayService;
            _courseService = courseService;
            _azureService = azureService;
            _orderPaymantService= orderPaymantService;
            _config = config;
            _orderService = orderService;
            _currencyService = currencyService;
        }

        public IActionResult Index()
        {
            return View(null);
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