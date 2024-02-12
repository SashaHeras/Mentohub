using LiqPay.SDK.Dto.Enums;
using LiqPay.SDK.Dto;
using LiqPay.SDK;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Core.Services.Interfaces.PaymentInterfaces;
using Mentohub.Domain.Helpers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

namespace Mentohub.Controllers
{
    [EnableCors("AllowAll")]
    [ApiController]
    [Route("api/payment")]
    public class PaymentController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICurrencyService _currencyService;
        private readonly IOrderPaymantService _orderPaymentService;
        private readonly IUserCourseService _useCourseService;
        private readonly IConfiguration _config;
        private readonly ILiqpayService _liqpayService;
        public PaymentController(IOrderService orderService, ICurrencyService currencyService,
            IOrderPaymantService orderPaymentService, IUserCourseService useCourseService,
            IConfiguration config, ILiqpayService liqpayService)
        {
            _orderService = orderService;
            _currencyService = currencyService;
            _orderPaymentService = orderPaymentService;
            _useCourseService = useCourseService;
            _config = config;
            _liqpayService = liqpayService;
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
            var currency = _currencyService.GetCurrencyByCode("UAN");
            var createOrder = new CreateOrderPayment();
            try
            {
                if (response.Status == LiqPayResponseStatus.Sandbox)
                {
                    var order = _orderService.GetOrder(orderID);
                    order.Ordered = DateTime.Now;
                    createOrder.OrderId = orderID;
                    createOrder.CurrencyId = currency.ID;
                    createOrder.Total = order.Total;
                    _orderPaymentService.CreateOrderPaymant(createOrder);
                    _orderService.UpdateOrder(order);
                    var model = _liqpayService.GenerateOrderPayModel(orderID);

                    return View("~/Views/Home/Index.cshtml", model);
                }
                if (response.Status == LiqPayResponseStatus.Error)
                {
                    return Json(new { IsError = true, Message = "An unforeseen error occurred" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { IsError = true, ex.Message });
            }

            return Json(new { IsError = true, Message = "The payment was unsuccessful" });

        }
        [HttpPost]
        [Route("CreateOrder")]
        public async Task<JsonResult> CreateOrder([FromForm] string userID, [FromForm] int courseId)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var result = await _orderService.GetActiveUserOrder(userID, courseId);
                    if (result == null)
                    {
                        return Json(new { IsError = true, Message = "Could not create a new order" });
                    }

                    scope.Complete();

                    return Json(new { IsError = false, result });
                }
                catch (Exception ex)
                {
                    return Json(new { IsError = true,  ex.Message });
                }
            }
        }

        [HttpPost]
        [Route("GetOrder")]
        public JsonResult GetOrder([FromForm] string orderId)
        {
            try
            {
                var result = _orderService.GetOrderDTO(orderId);
                if (result == null)
                {
                   return Json(new { IsError = true, Message = "Order does not exist" });
                }

                return Json(new { IsError = false, Data = result });
            }
            catch (Exception ex)
            {
                return Json(new { IsError = true, ex.Message });
            }
        }

        [HttpDelete]
        [Route("DeleteOrder")]
        public JsonResult DeleteOrder([FromForm] string orderId)
        {
            try
            {
                _orderService.DeleteOrder(orderId);
                return Json(new { IsError = false, Message = "Order successfully deleted" });
            }
            catch(Exception ex)
            {
                return Json(new { IsError = true, ex.Message });
            }
        }
        [HttpDelete]
        [Route("DeleteOrderItem")]
        public JsonResult DeleteOrderItem([FromForm] string orderItemId)
        {
            try
            {
                _orderService.DeleteOrder(orderItemId);
                return Json(new { IsError = false, Message = "Order successfully deleted" });
            }
            catch (Exception ex)
            {
                return Json(new { IsError = true, ex.Message });
            }
        }
        [HttpPost]
        [Route("GetCurrencyById")]
        public JsonResult GetCurrencyById([FromForm]int id)
        {           
            try
            {
                var currency = _currencyService.GetCurrency(id);
                if (currency == null)
                {
                    return Json(new { IsError = false, Message = "Currency does not exist" }); 
                }         
                return Json(new { IsError = false, Data= currency });
            }
            catch (Exception ex)
            {
                return Json(new { IsError = true, ex.Message });
            }
            
        }
        [HttpPost]
        [Route("GetOrderPaymentById")]
        public JsonResult GetOrderPaymentById([FromForm] string id)
        {
            try
            {
                var result = _orderPaymentService.GetOrderPayments(id);
                if (result == null)
                {
                    return Json(new { IsError = false, Message = "OrderPayments does not exist" });
                }
                return Json(new { IsError = false, Data=result });
            }
            catch (Exception ex)
            {
                return Json(new { IsError = true, ex.Message });
            }

        }
        [HttpPost]
        [Route("GetUserCoursesByUserId")]
        public JsonResult GetUserCoursesByUserId([FromForm] string userId)
        {
            try
            {
                var result =_useCourseService.GetUserCourses(userId);
                if (result.Count==0)
                {
                    return Json(new { IsError = false, Message = "User's courses does not exist" });
                }
                return Json(new { IsError = false, Data = result });
            }
            catch (Exception ex)
            {
                return Json(new { IsError = true, ex.Message });
            }
        }
        [HttpPost]
        [Route("GetUsersByCourseId")]
        public async Task<JsonResult> GetUsers(int courseId)
        {
            try
            {
                var result = await _useCourseService.GetUsers(courseId);
                if (result == null)
                {
                    return Json(new { IsError = false, Message = "Users does not exist" });
                }
                return Json(new { IsError = false, Data = result });
            }
            catch (Exception ex)
            {
                return Json(new { IsError = true, ex.Message });
            }
        }
    }
}
