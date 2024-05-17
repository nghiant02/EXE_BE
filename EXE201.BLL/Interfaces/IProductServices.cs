using EXE201.BLL.DTOs.UserDTOs;
using EXE201.DAL.DTOs;
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

    }
}
