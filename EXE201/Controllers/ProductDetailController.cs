using EXE201.BLL.Interfaces;
using EXE201.BLL.Services;
using EXE201.DAL.DTOs.ProductDTOs;
using Microsoft.AspNetCore.Mvc;

namespace EXE201.Controllers
{
    public class ProductDetailController : Controller
    {
        private readonly IProductDetailServices _productDetailServices;

        public ProductDetailController(IProductDetailServices productDetailServices)
        {
            _productDetailServices = productDetailServices;
        }

        [HttpPost("ViewProductDetailByProductId")]
        public async Task<IActionResult> GetProductDetailByProductId([FromQuery] int productId)
        {
            var productDetail = await _productDetailServices.GetProductDetailByProductId(productId);

            if (productDetail == null)
            {
                return NotFound(new { Message = "Product detail not found" });
            }

            var productSpecificationDTO = new ProductSpecificationDTO
            {
                ProductDetailId = productDetail.ProductDetailId,
                ProductId = productDetail.ProductId ?? 0,
                Description = productDetail.Description,
                AdditionalInformation = productDetail.AdditionalInformation,
                ShippingAndReturns = productDetail.ShippingAndReturns,
                SizeChart = productDetail.SizeChart,
                Reviews = productDetail.Reviews,
                Questions = productDetail.Questions,
                VendorInfo = productDetail.VendorInfo,
                MoreProducts = productDetail.MoreProducts,
                ProductPolicies = productDetail.ProductPolicies
            };

            return Ok(productSpecificationDTO);
        }

        [HttpPost("AddProductDetail")]
        public async Task<IActionResult> AddProductDetail([FromBody] AddProductDetailDTO addProductDetailDTO)
        {
            var response = await _productDetailServices.AddProductDetail(addProductDetailDTO);
            if (response.Status == "Error")
            {
                return Conflict(response);
            }
            return Ok(response);
        }

        [HttpPost("DeleteProductDetail")]
        public async Task<IActionResult> DeleteProductDetail([FromQuery] int productId)
        {
            var response = await _productDetailServices.DeleteProductDetail(productId);
            if (response.Status == "Error")
            {
                return Conflict(response);
            }
            return Ok(response);
        }

        [HttpPost("UpdateProductDetail")]
        public async Task<IActionResult> UpdateProductDetail([FromBody] UpdateProductDetailDTO updateProductDetailDTO)
        {
            var response = await _productDetailServices.UpdateProductDetail(updateProductDetailDTO);
            if (response.Status == "Error")
            {
                return Conflict(response);
            }
            return Ok(response);
        }
    }
}
