using EXE201.DAL.DTOs.CartDTOs;
using EXE201.DAL.Interfaces;
using EXE201.DAL.Models;
using MCC.DAL.Repository.Implements;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.DAL.Repository
{
    public class CartRepostiory : GenericRepository<Cart>, ICartRepository
    {
        public CartRepostiory(EXE201Context context) : base(context)
        {
        }

        public async Task<Cart> AddNewCart(Cart cart)
        {
            try
            {
                await _context.Carts.AddAsync(cart);
                await _context.SaveChangesAsync();  
                return cart;
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteCartById(int id)
        {
            try
            {
                var checkCart = await _context.Carts.Where(x => x.CartId == id).FirstOrDefaultAsync();
                if (checkCart != null)
                {
                    _context.Carts.Remove(checkCart);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Cart>> GetAll()
        {
            try
            {
                var checkCart = await _context.Carts
                    .Include(x => x.User)
                    .Include(x => x.Product)
                    .ToListAsync();
                if (checkCart != null )
                {
                    return checkCart;
                }
                return null;
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Cart> GetCartById(int id)
        {
            try
            {
                var checkCart = await _context.Carts.Where(x => x.CartId == id)
                    .Include(x => x.User)
                    .Include(x => x.Product)
                    .FirstOrDefaultAsync();

                if(checkCart != null)
                {
                    return checkCart;
                }
                return null;
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Cart> UpdateCart(Cart cart)
        {
            try
            {
                var checkCart = await _context.Carts.Where(x => x.CartId == cart.CartId).FirstOrDefaultAsync();
                if( checkCart != null )
                {
                    return checkCart;
                }
                return null;
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
