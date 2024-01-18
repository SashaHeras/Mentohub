﻿using Mentohub.Core.Services.Interfaces;
using Mentohub.Domain.PayMent;
using Microsoft.AspNetCore.Mvc;

namespace Mentohub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : Controller
    {
        private readonly IPrivatBankService _privatBankService;

        public PaymentController(IPrivatBankService privatBankService)
        {
            _privatBankService = privatBankService;
        }

        [HttpPost("makePayment")]
        public async Task<IActionResult> MakePayment([FromBody] PrivatBankRequest paymentRequest)
        {
            try
            {
                var paymentResult = await _privatBankService.MakePaymentAsync(paymentRequest);
                return Ok(paymentResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}