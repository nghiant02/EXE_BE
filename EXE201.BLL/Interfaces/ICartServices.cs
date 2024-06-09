using EXE201.DAL.DTOs.CartDTOs;
using EXE201.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.BLL.Interfaces
{
    public interface ICartServices
    {
        Task<IEnumerable<Cart>> GetAllCarts();
        Task<Cart> GetCartById(int userId);
        Task<Cart> UpdateCart(Cart cart);
        Task<bool> DeleteCart(int id);
        Task<Cart> AddNewCart(AddNewCartDTO cart);
    }
}
