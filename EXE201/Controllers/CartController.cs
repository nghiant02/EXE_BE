using EXE201.BLL.Interfaces;
using EXE201.DAL.DTOs.CartDTOs;
using EXE201.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EXE201.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartServices _cartService;
        public CartController(ICartServices cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("GetAllCarts")]
        public async Task<IActionResult> GetCarts()
        {
            var result = await _cartService.GetAllCarts();
            return Ok(result);
        }

        [HttpGet("GetCartById")]
        public async Task<IActionResult> GetCartById(int cartId)
        {
            var result = await _cartService.GetCartById(cartId);
            return Ok(result);
        }

        [HttpPost("AddNewCart")]
        public async Task<IActionResult> AddCart(AddNewCartDTO cart)
        {
            var result = await _cartService.AddNewCart(cart);
            return Ok(result);
        }

        [HttpDelete("DeleteCart")]
        public async Task<IActionResult> DeleteCart(int cartId)
        {
            var result = await _cartService.DeleteCart(cartId);
            return Ok(result);
        }

        [HttpPut("UpdateCart")]
        public async Task<IActionResult> UpdateCart(Cart cart)
        {
            var result = await _cartService.UpdateCart(cart);
            return Ok(result);
        }
    }
}
