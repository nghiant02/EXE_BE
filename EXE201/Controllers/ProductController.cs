using EXE201.BLL.Interfaces;
using EXE201.DAL.DTOs;
using EXE201.DAL.DTOs.ProductDTOs;
using EXE201.DAL.Models;
using EXE201.ViewModel.UserViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EXE201.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductServices _productServices;

        public ProductController(IProductServices productServices)
        {
            _productServices = productServices;
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var product = await _productServices.GetAll();
            return Ok(product);
        }

        [HttpPost("GetProductById")]
        public async Task<IActionResult> GetProductById([FromQuery] int id)
        {
            var response = await _productServices.GetById(id);
            if (response.ProductStatus == "Error")
            {
                return Conflict(response);
            }
            return Ok(response);
        }

        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct([FromQuery] AddProductDTO addProductDTO)
        {
            var response = await _productServices.AddProduct(addProductDTO);
            if (response.Status == "Error")
            {
                return Conflict(response);
            }
            return Ok(response);
        }

        [HttpPost("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct([FromQuery] int id)
        {
            var response = await _productServices.DeleteProduct(id);
            if (response.Status == "Error")
            {
                return Conflict(response);
            }
            return Ok(response);
        }

        [HttpPost("RecoverProduct")]
        public async Task<IActionResult> RecoverProduct([FromQuery] int id)
        {
            var response = await _productServices.RecoverProduct(id);
            if (response.Status == "Error")
            {
                return Conflict(response);
            }
            return Ok(response);
        }

        [HttpPost("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct([FromQuery] UpdateProductDTO updateProductDTO)
        {
            var response = await _productServices.UpdateProduct(updateProductDTO);
            if (response.Status == "Error")
            {
                return Conflict(response);
            }
            return Ok(response);
        }

    }
}
