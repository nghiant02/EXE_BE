using EXE201.DAL.DTOs;
using EXE201.DAL.DTOs.ProductDTOs;
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
                var product = new Product
                {
                    ProductName = addProduct.Name,
                    ProductDescription = addProduct.Description,
                    ProductImage = addProduct.Image,
                    ProductStatus = "Available",
                    ProductPrice = addProduct.Price,
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
            var product = await GetByIdAsync(id);
            if (product != null && product.ProductStatus == "Available")
            {
                product.ProductStatus = "Not Available";
                Update(product);
                await SaveChangesAsync();
                return new ResponeModel { Status = "Success", Message = "Product delete successfully" };
            }

            if (product.ProductStatus == "Not Available")
            {
                return new ResponeModel { Status = "Error", Message = "Product already delete" };
            }
            return new ResponeModel { Status = "Error", Message = "Product not found" };
        }

        public async Task<ResponeModel> RecoverProduct(int id)
        {
            var product = await GetByIdAsync(id);
            if (product != null && product.ProductStatus == "Not Available")
            {
                product.ProductStatus = "Available";
                Update(product);
                await SaveChangesAsync();
                return new ResponeModel { Status = "Success", Message = "Product recover successfully" };
            }
            if (product.ProductStatus == "Available")
            {
                return new ResponeModel { Status = "Error", Message = "Product already Available" };
            }
            return new ResponeModel { Status = "Error", Message = "Product not found" };
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await GetAllAsync();
        }

        public async Task<Product> GetById(int id)
        {
            return await GetByIdAsync(id);
        }

        public async Task<ResponeModel> UpdateProduct(UpdateProductDTO updateProductDTO)
        {
            try
            {
                var product = new Product
                {
                    ProductName = updateProductDTO.Name,
                    ProductDescription = updateProductDTO.Description,
                    ProductImage = updateProductDTO.Image,
                    ProductPrice = updateProductDTO.Price,
                    CategoryId = updateProductDTO.CategoryId
                };

                Update(product);
                await SaveChangesAsync();
                return new ResponeModel { Status = "Success", Message = "Product updated successfully", DataObject = product };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while updating the product" };
            }
        }
    }
}
