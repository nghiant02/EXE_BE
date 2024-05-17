using EXE201.BLL.DTOs.UserDTOs;
using EXE201.BLL.Interfaces;
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

    }
}
