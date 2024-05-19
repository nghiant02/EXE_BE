using EXE201.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EXE201.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class InventoryController : Controller
    {
        private readonly IInventoryServices _inventoryService;
        public InventoryController(IInventoryServices inventoryServices)
        {
            _inventoryService = inventoryServices;
        }
        [HttpGet("GetInventories")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _inventoryService.GetInventories();
                return Ok(result);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
