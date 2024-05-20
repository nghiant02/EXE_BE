using EXE201.DAL.DTOs;
using EXE201.DAL.DTOs.ProductDTOs;
using EXE201.DAL.Models;
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
        Task<Product> GetById(int id);
        Task<ResponeModel> DeleteProduct(int id);
        Task<ResponeModel> RecoverProduct(int id);
        Task<ResponeModel> UpdateProduct(UpdateProductDTO updateProductDTO);
    }
}
