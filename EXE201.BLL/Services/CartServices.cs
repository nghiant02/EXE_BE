using AutoMapper;
using EXE201.BLL.Interfaces;
using EXE201.DAL.Interfaces;
using EXE201.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.BLL.Services
{
    public class CartServices : ICartServices
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public CartServices(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<bool> DeleteCart(int id)
        {
            return await _cartRepository.DeleteCartById(id);
        }

        public async Task<IEnumerable<Cart>> GetAllCarts()
        {
            return await _cartRepository.GetAll();
        }

        public async Task<Cart> GetCartById(int id)
        {
            return await _cartRepository.GetCartById(id);
        }

        public async Task<Cart> UpdateCart(Cart cart)
        {
            return await _cartRepository.UpdateCart(cart);
        }
    }
}
