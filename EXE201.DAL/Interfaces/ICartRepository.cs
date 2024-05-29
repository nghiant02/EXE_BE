using EXE201.DAL.DTOs.CartDTOs;
using EXE201.DAL.Models;
using MCC.DAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.DAL.Interfaces
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<IEnumerable<Cart>> GetAll();
        Task<Cart> GetCartById(int id);
        Task<Cart> UpdateCart(Cart cart); 
        Task<bool> DeleteCartById(int id);
        Task<Cart> AddNewCart(Cart cart);
    }
}
