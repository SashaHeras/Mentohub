using LiqPay.SDK;
using LiqPay.SDK.Dto;
using LiqPay.SDK.Dto.Enums;
using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Core.Repositories.Interfaces.CourseInterfaces;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Domain.Data.Entities.Order;
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
        private readonly IAzureService _azureService;

        public HomeController(
            ILessonRepository lessonRepository,
            ICourseService courseService,
            IAzureService azureService,
            ILiqpayService liqpayService,
            IConfiguration config
            )
        {
            _lessonRepository = lessonRepository;
            _liqpayService = liqpayService;
            _courseService = courseService;
            _azureService = azureService;
            _config = config;
        }

        public IActionResult Index()
        {
            var order = _config["OrderID:ID"];
            var model = _liqpayService.GenerateOrderPayModel(order);
            return View(model);
        }

        /// <summary>
        /// На цю сторінку LiqPay відправляє результат оплати. Вона вказана в data.result_url
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Redirect()
        {
            // --- Перетворюю відповідь LiqPay в Dictionary<string, string> для зручності:
            var orderID = Request.Query["order_id"];

            var liqpayClient = new LiqPayClient(_config["Payment:Public"], _config["Payment:Private"]);

            var invoiceRequest = new LiqPayRequest()
            {
                OrderId = orderID,
                Action = LiqPayRequestAction.Status,
            };

            LiqPayResponse response = await liqpayClient.RequestAsync("request", invoiceRequest);

            var order = _config["OrderID:ID"];
            var model = _liqpayService.GenerateOrderPayModel(order);
            return View("~/Views/Home/Index.cshtml", model);
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