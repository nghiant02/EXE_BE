using EXE201.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EXE201.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressServices _addressServices;

        public AddressController(IAddressServices addressServices)
        {
            _addressServices = addressServices;
        }

        [HttpGet("GetAddressByUserId")]
        public async Task<IActionResult> GetAddressByUserIdAsync(int userId)
        {
            var result = await _addressServices.GetAddressByUserIdAsync(userId);
            return Ok(result);
        }
    }
}
