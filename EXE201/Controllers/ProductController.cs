using EXE201.BLL.Interfaces;
using EXE201.BLL.Services;
using EXE201.DAL.DTOs;
using EXE201.DAL.DTOs.ProductDTOs;
using EXE201.DAL.Models;
using EXE201.ViewModel.UserViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EXE201.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductServices _productServices;

        public ProductController(IProductServices productServices)
        {
            _productServices = productServices;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var product = await _productServices.GetAll();
            return Ok(product);
        }

        [HttpGet("GetProductById")]
        public async Task<IActionResult> GetProductById([FromQuery] int productId)
        {
            var response = await _productServices.GetById(productId);
            if (response.ProductStatus == "Error")
            {
                return Conflict(response);
            }
            return Ok(response);
        }

        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct([FromBody] AddProductDTO addProductDTO)
        {
            var response = await _productServices.AddProduct(addProductDTO);
            if (response.Status == "Error")
            {
                return Conflict(response);
            }
            return Ok(response);
        }

        [HttpPost("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct([FromQuery] int productId)
        {
            var response = await _productServices.DeleteProduct(productId);
            if (response.Status == "Error")
            {
                return Conflict(response);
            }
            return Ok(response);
        }

        [HttpPost("RecoverProduct")]
        public async Task<IActionResult> RecoverProduct([FromQuery] int productId)
        {
            var response = await _productServices.RecoverProduct(productId);
            if (response.Status == "Error")
            {
                return Conflict(response);
            }
            return Ok(response);
        }

        [HttpPost("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductDTO updateProductDTO)
        {
            var response = await _productServices.UpdateProduct(updateProductDTO);
            if (response.Status == "Error")
            {
                return Conflict(response);
            }
            return Ok(response);
        }

        [HttpGet("PagingAndFilteredProducts")]
        public async Task<IActionResult> GetFilteredProducts([FromQuery] ProductFilterDTO filter)
        {
            var products = await _productServices.GetFilteredProducts(filter);
            return Ok(products);
        }

        [HttpGet("RecommendHot")]
        public async Task<IActionResult> RecommendHot(int topN = 10)
        {
            var products = await _productServices.GetHotProducts(topN);
            return Ok(products);
        }

        [HttpGet("RecommendNew")]
        public async Task<IActionResult> RecommendNew(int topN = 10)
        {
            var products = await _productServices.GetNewProducts(topN);
            return Ok(products);
        }

        [HttpGet("RecommendHighlyRated")]
        public async Task<IActionResult> RecommendHighlyRated(int topN = 10)
        {
            var products = await _productServices.GetHighlyRatedProducts(topN);
            return Ok(products);
        }
    }
}
