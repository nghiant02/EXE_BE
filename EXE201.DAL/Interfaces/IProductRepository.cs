using EXE201.DAL.DTOs;
using EXE201.DAL.DTOs.ProductDTOs;
using EXE201.DAL.Models;
using LMSystem.Repository.Helpers;
using MCC.DAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.DAL.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetAll();
        Task<Product> GetById(int id);
        Task<ResponeModel> AddProduct(AddProductDTO addProduct);
        Task<ResponeModel> UpdateProduct(UpdateProductDTO updateProductDTO);
        Task<ResponeModel> DeleteProduct(int id);
        Task<ResponeModel> RecoverProduct(int id);
        //Task<IEnumerable<Product>> SearchProduct(string keyword);
        //Task<IEnumerable<Product>> FilterProduct(string category, double? minPrice, double? maxPrice);
        Task<PagedList<ProductWithRatingDTO>> GetFilteredProducts(ProductFilterDTO filter);

    }
}
