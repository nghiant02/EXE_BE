using AutoMapper;
using EXE201.BLL.Interfaces;
using EXE201.DAL.DTOs;
using EXE201.DAL.DTOs.ProductDTOs;
using EXE201.DAL.Interfaces;
using EXE201.DAL.Models;
using LMSystem.Repository.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.BLL.Services
{
    public class ProductServices : IProductServices
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductServices(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ResponeModel> AddProduct(AddProductDTO addProduct)
        {
            return await _productRepository.AddProduct(addProduct);
        }

        public async Task<ResponeModel> DeleteProduct(int id)
        {
            return await _productRepository.DeleteProduct(id);
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _productRepository.GetAll();
        }

        public async Task<ProductDetailDTO> GetById(int id)
        {
            return await _productRepository.GetById(id);
        }

        public async Task<ResponeModel> RecoverProduct(int id)
        {
            return await _productRepository.RecoverProduct(id);
        }

        public async Task<ResponeModel> UpdateProduct(UpdateProductDTO updateProductDTO)
        {
            return await _productRepository.UpdateProduct(updateProductDTO);
        }

        public async Task<PagedResponseDTO<ProductListDTO>> GetFilteredProducts(ProductFilterDTO filter)
        {
            return await _productRepository.GetFilteredProducts(filter);
        }

        public async Task<IEnumerable<ProductRecommendationDTO>> GetHotProducts(int topN)
        {
            return await _productRepository.GetHotProducts(topN);
        }

        public async Task<IEnumerable<ProductRecommendationDTO>> GetNewProducts(int topN)
        {
            return await _productRepository.GetNewProducts(topN);
        }

        public async Task<IEnumerable<ProductRecommendationDTO>> GetHighlyRatedProducts(int topN)
        {
            return await _productRepository.GetHighlyRatedProducts(topN);
        }

        public async Task<PagedResponseDTO<ProductListRecommendByCategoryDTO>> GetProductRecommendationsByCategory(int productId, ProductPagingRecommendByCategoryDTO filter)
        {
            return await _productRepository.GetProductRecommendationsByCategory(productId, filter);
        }

        public async Task<IEnumerable<ProductSuggestionDTO>> GetProductSuggestions(string searchTerm)
        {
            return await _productRepository.GetProductSuggestions(searchTerm);
        }
    }
}
