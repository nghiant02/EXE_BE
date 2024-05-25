using EXE201.BLL.Interfaces;
using EXE201.BLL.Services;
using EXE201.DAL.DTOs.ProductDTOs;
using Microsoft.AspNetCore.Mvc;

namespace EXE201.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalOrderController : Controller
    {

        private readonly IRentalOrderServices _rentalOrderServices;

        public RentalOrderController(IRentalOrderServices rentalOrderServices)
        {
            _rentalOrderServices = rentalOrderServices;
        }

        [HttpPost("CancelOrder")]
        public async Task<IActionResult> CancelOrder([FromQuery] int orderId)
        {
            var response = await _rentalOrderServices.CancelOrderAsync(orderId);
            if (response.Status == "Error")
            {
                return Conflict(response);
            }
            return Ok(response);
        }

        [HttpPost("ReturnOrder")]
        public async Task<IActionResult> ReturnOrder([FromQuery] int orderId, string returnReason)
        {
            var response = await _rentalOrderServices.ReturnOrderAsync(orderId, returnReason);
            if (response.Status == "Error")
            {
                return Conflict(response);
            }
            return Ok(response);
        }

        [HttpPost("ReturnItem")]
        public async Task<IActionResult> ReturnItem([FromQuery] ReturnItemDTO returnItem)
        {
            var response = await _rentalOrderServices.ReturnItem(returnItem);
            if (response.Status == "Error")
            {
                return Conflict(response);
            }
            return Ok(response);
        }
    }
}
