using EXE201.BLL.DTOs.UserDTOs;
using EXE201.DAL.Models;
using LMSystem.Repository.Data;
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
        Task<ResponeModel> UpdateProduct(Product product);
        Task<ResponeModel> DeleteProduct(int id);
    }
}
