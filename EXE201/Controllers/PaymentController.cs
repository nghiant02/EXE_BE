using EXE201.BLL.Interfaces;
using EXE201.BLL.Services;
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
            var response = await _paymentService.EnterPaymentDetails(paymentDetails);
            if (response.Status == "Error")
            {
                return BadRequest(response.Message);
            }
            return Ok(response.DataObject);
        }

        [HttpPost("ProcessPayment")]
        public async Task<IActionResult> ProcessPayment([FromBody] ProcessPaymentDTO processPayment)
        {
            var response = await _paymentService.ProcessPayment(processPayment);
            if (response.Status == "Error")
            {
                return BadRequest(response.Message);
            }
            return Ok(response);
        }

        [HttpGet("ViewHistoryPaymentByUserId")]
        public async Task<IActionResult> GetPaymentsByUserId(int userId)
        {
            try
            {
                var result = await _paymentService.GetPaymentsByUserIdAsync(userId);
                if (result == null || !result.Any())
                {
                    return BadRequest("No payment history found for the specified user.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ViewProfits")]
        public async Task<IActionResult> ViewProfits([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var profits = await _paymentService.GetProfitData(startDate, endDate);
            if (profits == null || !profits.Any())
            {
                return NotFound();
            }
            return Ok(profits);
        }
    }
}
