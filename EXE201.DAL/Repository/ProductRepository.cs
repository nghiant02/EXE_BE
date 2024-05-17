using EXE201.DAL.DTOs;
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
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly EXE201Context _context;
        public ProductRepository(EXE201Context context) : base(context)
        {
            _context = context;
        }

        public async Task<ResponeModel> AddProduct(AddProductDTO addProduct)
        {
            try
            {
                //var category = _context.Categories.FirstOrDefault(c => c.CategoryId == addProduct.CategoryId);
                var product = new Product
                {
                    Name = addProduct.Name,
                    Description = addProduct.Description,
                    Image = addProduct.Image,
                    Status = "Available",
                    Price = addProduct.Price,
                    CategoryId = addProduct.CategoryId
                };

                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return new ResponeModel { Status = "Success", Message = "Added product successfully", DataObject = product };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while adding the product" };
            }
        }

        public async Task<ResponeModel> DeleteProduct(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponeModel> UpdateProduct(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
