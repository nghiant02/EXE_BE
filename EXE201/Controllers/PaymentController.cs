using EXE201.BLL.Interfaces;
using EXE201.DAL.DTOs.PaymentDTOs;
using Microsoft.AspNetCore.Mvc;

namespace EXE201.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentServices _paymentService;

        public PaymentController(IPaymentServices paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("EnterPaymentDetails")]
        public async Task<IActionResult> EnterPaymentDetails([FromBody] EnterPaymentDetailsDTO paymentDetails)
        {
            var result = await _paymentService.EnterPaymentDetails(paymentDetails);
            if (result.Status == "Success")
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("ProcessPayment")]
        public async Task<IActionResult> ProcessPayment([FromBody] ProcessPaymentDTO processPayment)
        {
            var result = await _paymentService.ProcessPayment(processPayment);
            if (result.Status == "Success")
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
