using Mentohub.Core.Services.Interfaces;
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
        public PaymentController(IOrderService orderService)
        {
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
                    return Json(new { IsError = true, Message = ex.Message });
                }
            }
        }

        [HttpPost]
        [Route("GetOrder")]
        public JsonResult GetOrder([FromForm] string orderId)
        {
            var result = _orderService.GetOrderDTO(orderId);
            if (result == null)
            {
                return Json(new { IsError = true, Message = "Order does not exist" });
            }

            return Json(new { IsError = false, Data = result });
        }

        [HttpPost]
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
                return Json(new { IsError = true, Message = ex.Message });
            }
        }
    }
}
