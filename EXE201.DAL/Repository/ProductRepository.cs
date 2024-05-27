using EXE201.DAL.DTOs;
using EXE201.DAL.DTOs.ProductDTOs;
using EXE201.DAL.Interfaces;
using EXE201.DAL.Models;
using LMSystem.Repository.Helpers;
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
            var product = await GetByIdAsync(id);
            if (product != null && product.Status == "Available")
            {
                product.Status = "Not Available";
                Update(product);
                await SaveChangesAsync();
                return new ResponeModel { Status = "Success", Message = "Product delete successfully" };
            }

            if (product.Status == "Not Available")
            {
                return new ResponeModel { Status = "Error", Message = "Product already delete" };
            }
            return new ResponeModel { Status = "Error", Message = "Product not found" };
        }

        public async Task<ResponeModel> RecoverProduct(int id)
        {
            var product = await GetByIdAsync(id);
            if (product != null && product.Status == "Not Available")
            {
                product.Status = "Available";
                Update(product);
                await SaveChangesAsync();
                return new ResponeModel { Status = "Success", Message = "Product recover successfully" };
            }
            if (product.Status == "Available")
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
                    Name = updateProductDTO.Name,
                    Description = updateProductDTO.Description,
                    Image = updateProductDTO.Image,
                    Price = updateProductDTO.Price,
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

        //public async Task<IEnumerable<Product>> SearchProduct(string keyword)
        //{
        //    return await _dbSet.Where(p => p.ProductName.Contains(keyword) || p.ProductDescription.Contains(keyword)).ToListAsync();
        //}

        //public async Task<IEnumerable<Product>> FilterProduct(string category, double? minPrice, double? maxPrice)
        //{
        //    var query = _dbSet.AsQueryable();

        //    if (!string.IsNullOrEmpty(category))
        //    {
        //        query = query.Where(p => p.Category.CategoryName == category);
        //    }

        //    if (minPrice.HasValue)
        //    {
        //        query = query.Where(p => p.ProductPrice >= minPrice);
        //    }

        //    if (maxPrice.HasValue)
        //    {
        //        query = query.Where(p => p.ProductPrice <= maxPrice);
        //    }

        //    return await query.ToListAsync();
        //}

        public async Task<PagedList<Product>> GetFilteredProducts(ProductFilterDTO filter)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(filter.Search))
            {
                query = query.Where(p => p.ProductName.Contains(filter.Search) || p.ProductDescription.Contains(filter.Search));
            }

            if (!string.IsNullOrEmpty(filter.Color))
            {
                query = query.Where(p => p.ProductColor == filter.Color);
            }

            if (!string.IsNullOrEmpty(filter.Size))
            {
                query = query.Where(p => p.ProductSize == filter.Size);
            }

            if (filter.MinPrice.HasValue)
            {
                query = query.Where(p => p.ProductPrice >= filter.MinPrice);
            }

            if (filter.MaxPrice.HasValue)
            {
                query = query.Where(p => p.ProductPrice <= filter.MaxPrice);
            }

            var products = await query.ToListAsync();
            return PagedList<Product>.ToPagedList(products, filter.PageNumber, filter.PageSize);
        }
    }
}
