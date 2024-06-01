using EXE201.DAL.DTOs;
using EXE201.DAL.DTOs.FeedbackDTOs;
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

        public async Task<ProductDetailDTO> GetById(int id)
        {
            var product = await _context.Products
            .Include(p => p.Ratings)
                .ThenInclude(r => r.User)
            .Include(p => p.Ratings)
                .ThenInclude(r => r.Feedback)
            .Include(p => p.Category)
            .Where(p => p.ProductId == id)
            .Select(p => new ProductDetailDTO
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                ProductDescription = p.ProductDescription,
                ProductImage = p.ProductImage,
                ProductPrice = p.ProductPrice,
                ProductSize = p.ProductSize,
                ProductColor = p.ProductColor,
                ProductStatus = p.ProductStatus,
                CategoryName = p.Category.CategoryName,
                AverageRating = p.Ratings.Any() ? p.Ratings.Average(r => r.RatingValue ?? 0) : 0,
                RatingsFeedback = p.Ratings.Select(r => new RatingFeedbackDTO
                {
                    RatingId = r.RatingId,
                    UserId = r.User.UserId,
                    UserName = r.User.UserName,
                    RatingValue = r.RatingValue ?? 0,
                    DateGiven = r.DateGiven,
                    FeedbackComment = r.Feedback.FeedbackComment,
                    FeedbackImage = r.Feedback.FeedbackImage
                }).ToList()
            })
            .FirstOrDefaultAsync();

            return product;
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

        public async Task<PagedList<ProductWithRatingDTO>> GetFilteredProducts(ProductFilterDTO filter)
        {
            var query = _context.Products
                                .Include(p => p.Ratings)
                                .Select(p => new ProductWithRatingDTO
                                {
                                    ProductId = p.ProductId,
                                    ProductName = p.ProductName,
                                    ProductDescription = p.ProductDescription,
                                    ProductImage = p.ProductImage,
                                    ProductStatus = p.ProductStatus,
                                    ProductPrice = p.ProductPrice,
                                    CategoryId = p.CategoryId,
                                    ProductSize = p.ProductSize,
                                    ProductColor = p.ProductColor,
                                    AverageRating = p.Ratings.Any() ? p.Ratings.Average(r => r.RatingValue ?? 0) : 0
                                })
                                .AsQueryable();

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

            if (!string.IsNullOrEmpty(filter.SortBy))
            {
                switch (filter.SortBy.ToLower())
                {
                    case "price":
                        query = filter.Sort ? query.OrderByDescending(p => p.ProductPrice) : query.OrderBy(p => p.ProductPrice);
                        break;
                    case "name":
                        query = filter.Sort ? query.OrderByDescending(p => p.ProductName) : query.OrderBy(p => p.ProductName);
                        break;
                    case "rating":
                        query = filter.Sort ? query.OrderByDescending(p => p.AverageRating) : query.OrderBy(p => p.AverageRating);
                        break;
                    default:
                        query = query.OrderBy(p => p.ProductId);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(p => p.ProductId);
            }

            var products = await query.ToListAsync();
            return PagedList<ProductWithRatingDTO>.ToPagedList(products, filter.PageNumber, filter.PageSize);
        }
    }
}
