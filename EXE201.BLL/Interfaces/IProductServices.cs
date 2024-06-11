﻿using EXE201.DAL.DTOs;
using EXE201.DAL.DTOs.ProductDTOs;
using EXE201.DAL.Models;
using LMSystem.Repository.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.BLL.Interfaces
{
    public interface IProductServices
    {
        Task<ResponeModel> AddProduct(AddProductDTO addProduct);
        Task<IEnumerable<Product>> GetAll();
        Task<ProductDetailDTO> GetById(int id);
        Task<ResponeModel> DeleteProduct(int id);
        Task<ResponeModel> RecoverProduct(int id);
        Task<ResponeModel> UpdateProduct(UpdateProductDTO updateProductDTO);
        Task<PagedResponseDTO<ProductListDTO>> GetFilteredProducts(ProductFilterDTO filter);
        Task<IEnumerable<ProductRecommendationDTO>> GetHotProducts(int topN);
        Task<IEnumerable<ProductRecommendationDTO>> GetNewProducts(int topN);
        Task<IEnumerable<ProductRecommendationDTO>> GetHighlyRatedProducts(int topN);
        Task<PagedResponseDTO<ProductListRecommendByCategoryDTO>> GetProductRecommendationsByCategory(int productId, int pageNumber, int pageSize);
    }
}
