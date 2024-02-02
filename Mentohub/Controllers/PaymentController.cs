using Mentohub.Core.Services.Interfaces;
using Mentohub.Core.Services.Interfaces.PaymentInterfaces;
using Mentohub.Core.Services.Services.PaymentServices;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.Entities.Order;
using Mentohub.Domain.PayMentAlla;
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
        public PaymentController(IOrderService orderService, ICurrencyService currencyService,
            IOrderPaymantService orderPaymentService, IUserCourseService useCourseService)
        {
            _orderService = orderService;
            _currencyService = currencyService;
            _orderPaymentService = orderPaymentService;
            _useCourseService = useCourseService;
        }

        //[HttpPost("makePayment")]
        //public async Task<IActionResult> MakePayment([FromBody] PrivatBankRequest paymentRequest)
        //{
        //    try
        //    {
        //        var paymentResult = await _privatBankService.MakePaymentAsync(paymentRequest);
        //        return Ok(paymentResult);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { error = ex.Message });
        //    }
        //}

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
        public JsonResult GetUsers(int courseId)
        {
            try
            {
                var result = _useCourseService.GetUsers(courseId);
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
