using Mentohub.Core.Services.Interfaces;
using Mentohub.Domain.PayMentAlla;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Mentohub.Controllers
{
    [EnableCors("AllowAll")]
    [ApiController]
    [Route("api/payment")]
    public class PaymentController : Controller
    {
        //private readonly IPrivatBankService _privatBankService;
        private readonly IOrderService _orderService;
        public PaymentController(/*IPrivatBankService privatBankService,*/ IOrderService orderService)
        {
            //_privatBankService = privatBankService;
            _orderService = orderService;
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
        public async Task<JsonResult> CreateOrder(string userID, int courseId)
        {
            var result = await _orderService.GetActiveUserOrder(userID, courseId);
            if (result == null)
            {
                return Json(new { IsError = true, Message = "Could not create a new order" });
            }
            return Json(new { IsError = false, result });
        }
        [HttpPost]
        [Route("GetOrder")]
        public JsonResult GetOrder(string orderId)
        {
            var result = _orderService.GetOrderDTO(orderId);
            if (result == null)
            {
                return Json(new { IsError = true, Message = "Order does not exist" });
            }
            return Json(new { IsError = false, result });
        }
        [HttpPost]
        [Route("DeleteOrder")]
        public JsonResult DeleteOrder(string orderId)
        {
            var result = _orderService.GetOrder(orderId);
            if (result == null)
            {
                return Json(new { IsError = true, Message = "Order does not exist" });
            }
            _orderService.DeleteOrder(orderId);
            return Json(new { IsError = false,Message= "Order successfully deleted" });
        }
    }
}
