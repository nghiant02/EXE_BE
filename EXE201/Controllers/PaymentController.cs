using EXE201.BLL.Interfaces;
using EXE201.BLL.Services;
using EXE201.DAL.DTOs.PaymentDTOs;
using EXE201.DAL.DTOs.PaymentDTOs.EXE201.DAL.DTOs.PaymentDTOs;
using EXE201.DAL.Models;
using LMSystem.Repository.Helpers;
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

        [HttpPost("AddPaymentForUser")]
        public async Task<IActionResult> AddPaymentForUser([FromQuery] int userId, [FromQuery] AddPaymentDTO request)
        {
            // Step 1: Generate a random payment ID
            var orderCode = GenerateRandomNumericId(5); // Assuming a 10-digit numeric ID, adjust as needed

            // Step 2: Generate URLs with the actual paymentId
            var cancelUrl = Url.Action("CancelPayment", "Payment", new { returnUrl = request.ReturnUrl, orderCode }, Request.Scheme);
            var successUrl = Url.Action("SuccessPayment", "Payment", new { userId, returnUrl = request.ReturnUrl, orderCode }, Request.Scheme);

            // Step 3: Call the service to create the payment and pass the generated paymentId
            var response = await _paymentService.AddPaymentForUser(userId, request, successUrl, cancelUrl, orderCode);

            if (response.Status == "Error")
            {
                return BadRequest(new { Message = response.Message });
            }

            // Return the final response
            return Ok(response);
        }

        private static int GenerateRandomNumericId(int length)
        {
            var random = new Random();
            var min = (int)Math.Pow(10, length - 1);
            var max = (int)Math.Pow(10, length) - 1;
            return random.Next(min, max);
        }



        [HttpPost("ConfirmPayment")]
        public async Task<IActionResult> ConfirmPayment([FromQuery] int paymentId)
        {
            var response = await _paymentService.ConfirmPayment(paymentId);
            if (response.Status == "Error")
            {
                return BadRequest(new { Message = response.Message });
            }
            return Ok(response.DataObject);
        }

        [HttpGet("ViewHistoryPaymentByUserId")]
        public async Task<IActionResult> GetPaymentsByUserId(int userId, [FromQuery] PaginationParameter paginationParameter)
        {
            try
            {
                var result = await _paymentService.GetPaymentsByUserIdAsync(userId, paginationParameter);
                if (result == null || !result.Items.Any())
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

        //[HttpGet("ViewProfits")]
        //public async Task<IActionResult> ViewProfits([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        //{
        //    var profits = await _paymentService.GetProfitData(startDate, endDate);
        //    if (profits == null || !profits.Any())
        //    {
        //        return NotFound();
        //    }
        //    return Ok(profits);
        //}

        [HttpGet("ViewAllPaymentMethods")]
        public async Task<IActionResult> ViewAllPaymentMethods()
        {
            var paymentMethods = await _paymentService.GetAllPaymentMethods();
            return Ok(paymentMethods);
        }

        [HttpGet("GetPaymentLinkInformation")]
        public async Task<IActionResult> GetPaymentLinkInformation(int orderId)
        {
            var paymentMethods = await _paymentService.GetPaymentLinkInformation(orderId);
            return Ok(paymentMethods);
        }

        [HttpGet("ViewAllPayments")]
        public async Task<IActionResult> ViewAllPayments()
        {
            var payments = await _paymentService.GetAllPayments();
            return Ok(payments);
        }

        [HttpPost("CreatePaymentMethod")]
        public async Task<IActionResult> CreatePaymentMethod([FromQuery] string paymentMethodName)
        {
            var createdPaymentMethod = await _paymentService.CreatePaymentMethod(paymentMethodName);
            return CreatedAtAction(nameof(ViewAllPaymentMethods), new { id = createdPaymentMethod.PaymentMethodId }, createdPaymentMethod);
        }

        [HttpPatch("UpdatePaymentMethodName")]
        public async Task<IActionResult> UpdatePaymentMethodName([FromBody] UpdatePaymentMethodNameDTO updatePaymentMethodNameDTO)
        {
            var updatedPaymentMethod = await _paymentService.UpdatePaymentMethodName(updatePaymentMethodNameDTO.PaymentMethodId, updatePaymentMethodNameDTO.PaymentMethodName);
            if (updatedPaymentMethod == null)
            {
                return NotFound(new { Message = "Payment method not found." });
            }

            return Ok(updatedPaymentMethod);
        }

        [HttpDelete("DeletePaymentMethod/{paymentMethodId}")]
        public async Task<IActionResult> DeletePaymentMethod(int paymentMethodId)
        {
            var result = await _paymentService.DeletePaymentMethod(paymentMethodId);
            if (!result)
            {
                return NotFound(new { Message = "Payment method not found." });
            }

            return Ok(result);
        }

        [HttpDelete("DeletePayment/{paymentId}")]
        public async Task<IActionResult> DeletePayment(int paymentId)
        {
            var result = await _paymentService.DeletePayment(paymentId);
            if (!result)
            {
                return NotFound(new { Message = "Payment not found." });
            }

            return Ok(result);
        }

        [HttpPut("UpdatePaymentStatus")]
        public async Task<IActionResult> UpdatePaymentStatus(int paymentId)
        {
            var result = await _paymentService.UpdatePaymentStatus(paymentId, PaymentStatus.Failed);

            return Ok(result);
        }

        [HttpGet("CancelPayment")]
        public async Task<IActionResult> CancelPayment(string returnUrl, int paymentId)
        {
            await _paymentService.UpdatePaymentStatus(paymentId, PaymentStatus.Failed);
            return Redirect($"{returnUrl}/status=canceled");
        }

        [HttpGet("SuccessPayment")]
        public async Task<IActionResult> SuccessPayment(string userId, string returnUrl, int orderCode)
        {
            var response = await _paymentService.HandleSuccessfulPaymentAsync(orderCode, userId, returnUrl);

            if (response.Status == "Error")
            {
                return Redirect($"{returnUrl}/status=error&message={response.Message}");
            }

            return Redirect($"{returnUrl}/status=success");
        }


        //[HttpGet("PaymentCallback")]
        //public async Task<IActionResult> PaymentCallback(bool success, int paymentId)
        //{
        //    // Update the payment status based on the callback result
        //    var status = success ? "Completed" : "Failed";
        //    await _paymentService.UpdatePaymentStatusAndClearCartAsync(paymentId, status);

        //    if (success)
        //    {
        //        // Redirect to order tracking or a success page
        //        return Redirect("https://voguary.id.vn/orderTracking"); // Replace with your order tracking page URL
        //    }
        //    else
        //    {
        //        // Redirect to a failure page or retry payment
        //        return Redirect("https://voguary.id.vn/cart"); // Replace with your payment failure page URL
        //    }
        //}


    }
}
