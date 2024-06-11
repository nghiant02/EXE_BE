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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
                // Check if CategoryId exists
                var categoryExists = await _context.Categories.AnyAsync(c => c.CategoryId == addProduct.CategoryId);
                if (!categoryExists)
                {
                    return new ResponeModel { Status = "Error", Message = "Invalid CategoryId" };
                }

                var product = new Product
                {
                    ProductName = addProduct.Name,
                    ProductTitle = addProduct.Title,
                    ProductDescription = addProduct.Description,
                    ProductStatus = "Available",
                    ProductPrice = addProduct.Price,
                    CategoryId = addProduct.CategoryId
                };

                // Handle Product Images
                var imageEntities = await _context.Images
                    .Where(i => addProduct.ProductImage.Contains(i.ImageUrl))
                    .ToListAsync();

                var newImageUrls = addProduct.ProductImage.Except(imageEntities.Select(i => i.ImageUrl)).ToList();
                var newImages = newImageUrls.Select(url => new Image { ImageUrl = url }).ToList();

                if (newImages.Any())
                {
                    _context.Images.AddRange(newImages);
                    await _context.SaveChangesAsync();
                    imageEntities.AddRange(newImages);
                }

                product.ProductImages = imageEntities.Select(img => new ProductImage { ImageId = img.ImageId, Product = product }).ToList();

                // Handle Product Colors
                var colorEntities = await _context.Colors
                    .Where(c => addProduct.ProductColor.Contains(c.ColorName))
                    .ToListAsync();

                var newColorNames = addProduct.ProductColor.Except(colorEntities.Select(c => c.ColorName)).ToList();
                var newColors = newColorNames.Select(name => new Color { ColorName = name }).ToList();

                if (newColors.Any())
                {
                    _context.Colors.AddRange(newColors);
                    await _context.SaveChangesAsync();
                    colorEntities.AddRange(newColors);
                }

                product.ProductColors = colorEntities.Select(color => new ProductColor { ColorId = color.ColorId, Product = product }).ToList();

                // Handle Product Sizes
                var sizeEntities = await _context.Sizes
                    .Where(s => addProduct.ProductSize.Contains(s.SizeName))
                    .ToListAsync();

                var newSizeNames = addProduct.ProductSize.Except(sizeEntities.Select(s => s.SizeName)).ToList();
                var newSizes = newSizeNames.Select(name => new Size { SizeName = name }).ToList();

                if (newSizes.Any())
                {
                    _context.Sizes.AddRange(newSizes);
                    await _context.SaveChangesAsync();
                    sizeEntities.AddRange(newSizes);
                }

                product.ProductSizes = sizeEntities.Select(size => new ProductSize { SizeId = size.SizeId, Product = product }).ToList();

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

        //public async Task<ProductDetailDTO> GetById(int id)
        //{
        //    var product = await _context.Products
        //    .Include(p => p.ProductDetails)
        //    .Include(p => p.Ratings)
        //        .ThenInclude(r => r.User)
        //    .Include(p => p.Ratings)
        //        .ThenInclude(r => r.Feedback)
        //    .Include(p => p.Category)
        //    .Where(p => p.ProductId == id)
        //    .Select(p => new ProductDetailDTO
        //    {
        //        ProductId = p.ProductId,
        //        ProductName = p.ProductName,
        //        ProductDescription = p.ProductDescription,
        //        //ProductImage = p.ProductImage,
        //        ProductPrice = p.ProductPrice,
        //        ProductSize = p.ProductDetails.Select(s => s.Size.SizeName),
        //        ProductColor = p.ProductDetails.Select(c => c.Color.ColorName),
        //        ProductStatus = p.ProductStatus,
        //        CategoryName = p.Category.CategoryName,
        //        AverageRating = p.Ratings.Any() ? p.Ratings.Average(r => r.RatingValue ?? 0) : 0,
        //        RatingsFeedback = p.Ratings.Select(r => new RatingFeedbackDTO
        //        {
        //            RatingId = r.RatingId,
        //            UserId = r.User.UserId,
        //            UserName = r.User.UserName,
        //            RatingValue = r.RatingValue ?? 0,
        //            DateGiven = r.DateGiven,
        //            FeedbackComment = r.Feedback.FeedbackComment,
        //            FeedbackImage = r.Feedback.FeedbackImage
        //        }).ToList()
        //    })
        //    .FirstOrDefaultAsync();

        public async Task<ProductDetailDTO> GetById(int id)
        {
            var product = await _context.Products
                                .Include(p => p.ProductColors)
                                .Include(p => p.ProductSizes)
                                .Include(p => p.ProductImages)
                                .Include(p => p.ProductDetails)
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
                ProductTitle = p.ProductTitle,
                ProductDescription = p.ProductDescription,
                ProductImage = p.ProductImages.Select(p => p.Image.ImageUrl).ToList(),
                ProductPrice = p.ProductPrice,
                ProductSize = p.ProductSizes.Select(p => p.Size.SizeName).ToList(),
                ProductColor = p.ProductColors.Select(p => p.Color.ColorName).ToList(),
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

        //    return product;
        //}

        public async Task<ResponeModel> UpdateProduct(UpdateProductDTO updateProductDTO)
        {
            try
            {
                var product = await _context.Products
                    .Include(p => p.ProductImages)
                    .ThenInclude(pi => pi.Image)
                    .Include(p => p.ProductColors)
                    .ThenInclude(pc => pc.Color)
                    .Include(p => p.ProductSizes)
                    .ThenInclude(ps => ps.Size)
                    .FirstOrDefaultAsync(p => p.ProductId == updateProductDTO.ProductId);

                if (product == null)
                {
                    return new ResponeModel { Status = "Error", Message = "Product not found" };
                }

                // Update product properties if provided
                if (!string.IsNullOrEmpty(updateProductDTO.Name))
                {
                    product.ProductName = updateProductDTO.Name;
                }

                if (!string.IsNullOrEmpty(updateProductDTO.ProductTitle))
                {
                    product.ProductTitle = updateProductDTO.ProductTitle;
                }

                if (!string.IsNullOrEmpty(updateProductDTO.Description))
                {
                    product.ProductDescription = updateProductDTO.Description;
                }

                if (updateProductDTO.Price.HasValue)
                {
                    product.ProductPrice = updateProductDTO.Price;
                }

                if (updateProductDTO.CategoryId.HasValue)
                {
                    product.CategoryId = updateProductDTO.CategoryId;
                }

                // Update Product Images if provided
                if (updateProductDTO.ProductImage != null && updateProductDTO.ProductImage.Any())
                {
                    product.ProductImages.Clear();

                    var imageEntities = await _context.Images
                        .Where(i => updateProductDTO.ProductImage.Contains(i.ImageUrl))
                        .ToListAsync();

                    var newImageUrls = updateProductDTO.ProductImage.Except(imageEntities.Select(i => i.ImageUrl)).ToList();
                    var newImages = newImageUrls.Select(url => new Image { ImageUrl = url }).ToList();

                    if (newImages.Any())
                    {
                        _context.Images.AddRange(newImages);
                        await _context.SaveChangesAsync();
                        imageEntities.AddRange(newImages);
                    }

                    product.ProductImages = imageEntities.Select(img => new ProductImage { ImageId = img.ImageId, Product = product }).ToList();
                }

                // Update Product Colors if provided
                if (updateProductDTO.ProductColor != null && updateProductDTO.ProductColor.Any())
                {
                    product.ProductColors.Clear();

                    var colorEntities = await _context.Colors
                        .Where(c => updateProductDTO.ProductColor.Contains(c.ColorName))
                        .ToListAsync();

                    var newColorNames = updateProductDTO.ProductColor.Except(colorEntities.Select(c => c.ColorName)).ToList();
                    var newColors = newColorNames.Select(name => new Color { ColorName = name }).ToList();

                    if (newColors.Any())
                    {
                        _context.Colors.AddRange(newColors);
                        await _context.SaveChangesAsync();
                        colorEntities.AddRange(newColors);
                    }

                    product.ProductColors = colorEntities.Select(color => new ProductColor { ColorId = color.ColorId, Product = product }).ToList();
                }

                // Update Product Sizes if provided
                if (updateProductDTO.ProductSize != null && updateProductDTO.ProductSize.Any())
                {
                    product.ProductSizes.Clear();

                    var sizeEntities = await _context.Sizes
                        .Where(s => updateProductDTO.ProductSize.Contains(s.SizeName))
                        .ToListAsync();

                    var newSizeNames = updateProductDTO.ProductSize.Except(sizeEntities.Select(s => s.SizeName)).ToList();
                    var newSizes = newSizeNames.Select(name => new Size { SizeName = name }).ToList();

                    if (newSizes.Any())
                    {
                        _context.Sizes.AddRange(newSizes);
                        await _context.SaveChangesAsync();
                        sizeEntities.AddRange(newSizes);
                    }

                    product.ProductSizes = sizeEntities.Select(size => new ProductSize { SizeId = size.SizeId, Product = product }).ToList();
                }

                _context.Products.Update(product);
                await _context.SaveChangesAsync();

                return new ResponeModel { Status = "Success", Message = "Product updated successfully", DataObject = product };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while updating the product" };
            }
        }

        //public async Task<PagedResponseDTO<ProductListDTO>> GetFilteredProducts(ProductFilterDTO filter)
        //{
        //    var query = _context.Products
        //                        .Include(p => p.Ratings)
        //                        .Select(p => new ProductWithRatingDTO
        //                        {
        //                            ProductId = p.ProductId,
        //                            ProductName = p.ProductName,
        //                            ProductDescription = p.ProductDescription,
        //                            //ProductImage = p.ProductImage,
        //                            ProductStatus = p.ProductStatus,
        //                            ProductPrice = p.ProductPrice,
        //                            CategoryId = p.CategoryId,
        //                            //ProductSize = p.ProductSize,
        //                            //ProductColor = p.ProductColor,
        //                            AverageRating = p.Ratings.Any() ? p.Ratings.Average(r => r.RatingValue ?? 0) : 0
        //                        })
        //                        .AsQueryable();
        //        .Include(p => p.ProductDetails)
        //            .ThenInclude(pd => pd.Color)
        //        .Include(p => p.ProductDetails)
        //            .ThenInclude(pd => pd.Size)
        //        .Include(p => p.Ratings)
        //        .Include(p => p.Category)
        //        .Select(p => new ProductListDTO
        //        {
        //            ProductId = p.ProductId,
        //            ProductName = p.ProductName,
        //            ProductDescription = p.ProductDescription,
        //            ProductImage = p.ProductImage,
        //            ProductStatus = p.ProductStatus,
        //            ProductPrice = p.ProductPrice,
        //            Category = p.Category.CategoryName,
        //            ProductSize = p.ProductDetails.Select(pd => pd.Size.SizeName).ToList(),
        //            ProductColor = p.ProductDetails.Select(pd => pd.Color.ColorName).ToList(),
        //            AverageRating = p.Ratings.Any() ? p.Ratings.Average(r => r.RatingValue ?? 0) : 0
        //        })
        //        .AsQueryable();

        public async Task<PagedResponseDTO<ProductListDTO>> GetFilteredProducts(ProductFilterDTO filter)
        {
            var query = _context.Products
                                .Include(p => p.ProductColors)
                                .Include(p => p.ProductSizes)
                                .Include(p => p.ProductImages)
                                .Include(p => p.ProductDetails)
                                .Include(p => p.Ratings)
                                .Include(p => p.Category)
                .Select(p => new ProductListDTO
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    ProductTitle = p.ProductTitle,
                    ProductDescription = p.ProductDescription,
                    ProductImage = p.ProductImages.Select(pi => pi.Image.ImageUrl).ToList(),
                    ProductStatus = p.ProductStatus,
                    ProductPrice = p.ProductPrice,
                    Category = p.Category.CategoryName,
                    ProductSize = p.ProductSizes.Select(ps => ps.Size.SizeName).ToList(),
                    ProductColor = p.ProductColors.Select(pc => pc.Color.ColorName).ToList(),
                    AverageRating = p.Ratings.Any() ? p.Ratings.Average(r => r.RatingValue ?? 0) : 0,
                    ColorCount = p.ProductColors.Select(pc => pc.Color.ColorName).Distinct().Count() // Count of unique colors
                })
                .AsQueryable();

        //    // Apply filters
        //    if (!string.IsNullOrEmpty(filter.Search))
        //    {
        //        query = query.Where(p => p.ProductName.Contains(filter.Search) || p.ProductDescription.Contains(filter.Search));
        //    }

        //    if (filter.Colors != null && filter.Colors.Any())
        //    {
        //        query = query.Where(p => p.ProductColor.Any(color => filter.Colors.Contains(color)));
        //    }

        //    if (filter.Sizes != null && filter.Sizes.Any())
        //    {
        //        query = query.Where(p => p.ProductSize.Any(size => filter.Sizes.Contains(size)));
        //    }

        //    if (!string.IsNullOrEmpty(filter.Category))
        //    {
        //        query = query.Where(p => p.Category.Equals(filter.Category));
        //    }
            if (filter.Category != null && filter.Category.Any())
            {
                query = query.Where(p => p.Category == filter.Category.FirstOrDefault());
            }

        //    if (filter.MinPrice.HasValue)
        //    {
        //        query = query.Where(p => p.ProductPrice >= filter.MinPrice);
        //    }

        //    if (filter.MaxPrice.HasValue)
        //    {
        //        query = query.Where(p => p.ProductPrice <= filter.MaxPrice);
        //    }

        //    // Apply sorting
        //    if (!string.IsNullOrEmpty(filter.SortBy))
        //    {
        //        switch (filter.SortBy.ToLower())
        //        {
        //            case "price":
        //                query = filter.Sort ? query.OrderByDescending(p => p.ProductPrice) : query.OrderBy(p => p.ProductPrice);
        //                break;
        //            case "name":
        //                query = filter.Sort ? query.OrderByDescending(p => p.ProductName) : query.OrderBy(p => p.ProductName);
        //                break;
        //            case "rating":
        //                query = filter.Sort ? query.OrderByDescending(p => p.AverageRating) : query.OrderBy(p => p.AverageRating);
        //                break;
        //            default:
        //                query = query.OrderBy(p => p.ProductId);
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        query = query.OrderBy(p => p.ProductId);
        //    }

        //    var totalCount = await query.CountAsync();
        //    var products = await query.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

        //    return new PagedResponseDTO<ProductListDTO>
        //    {
        //        PageNumber = filter.PageNumber,
        //        PageSize = filter.PageSize,
        //        TotalCount = totalCount,
        //        Items = products
        //    };
        //}


        public async Task<IEnumerable<ProductRecommendationDTO>> GetHotProducts(int topN)
        {
            var products = await _context.Products
                .Include(p => p.RentalOrderDetails)
                .Include(p => p.Category)
                .Include(p => p.Ratings)
                .Select(p => new ProductRecommendationDTO
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    ProductDescription = p.ProductDescription,
                    ProductImage = p.ProductImages.Select(p => p.Image.ImageUrl).ToList(),
                    ProductPrice = p.ProductPrice,
                    ProductSize = p.ProductSizes.Select(p => p.Size.SizeName).ToList(),
                    ProductColor = p.ProductColors.Select(p => p.Color.ColorName).ToList(),
                    ProductStatus = p.ProductStatus,
                    CategoryName = p.Category.CategoryName,
                    AverageRating = p.Ratings.Any() ? p.Ratings.Average(r => r.RatingValue ?? 0) : 0,
                    NumberOfPurchases = p.RentalOrderDetails.Count
                })
                .OrderByDescending(p => p.NumberOfPurchases)
                .Take(topN)
                .ToListAsync();

            return products;
        }

        public async Task<IEnumerable<ProductRecommendationDTO>> GetNewProducts(int topN)
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Ratings)
                .Select(p => new ProductRecommendationDTO
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    ProductDescription = p.ProductDescription,
                    ProductImage = p.ProductImages.Select(p => p.Image.ImageUrl).ToList(),
                    ProductPrice = p.ProductPrice,
                    ProductSize = p.ProductSizes.Select(p => p.Size.SizeName).ToList(),
                    ProductColor = p.ProductColors.Select(p => p.Color.ColorName).ToList(),
                    ProductStatus = p.ProductStatus,
                    CategoryName = p.Category.CategoryName,
                    AverageRating = p.Ratings.Any() ? p.Ratings.Average(r => r.RatingValue ?? 0) : 0,
                    CreatedAt = p.CreatedAt
                })
                .OrderByDescending(p => p.CreatedAt)
                .Take(topN)
                .ToListAsync();

            return products;
        }

        public async Task<IEnumerable<ProductRecommendationDTO>> GetHighlyRatedProducts(int topN)
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Ratings)
                .Select(p => new ProductRecommendationDTO
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    ProductDescription = p.ProductDescription,
                    ProductImage = p.ProductImages.Select(p => p.Image.ImageUrl).ToList(),
                    ProductPrice = p.ProductPrice,
                    ProductSize = p.ProductSizes.Select(p => p.Size.SizeName).ToList(),
                    ProductColor = p.ProductColors.Select(p => p.Color.ColorName).ToList(),
                    ProductStatus = p.ProductStatus,
                    CategoryName = p.Category.CategoryName,
                    AverageRating = p.Ratings.Any() ? p.Ratings.Average(r => r.RatingValue ?? 0) : 0
                })
                .OrderByDescending(p => p.AverageRating)
                .Take(topN)
                .ToListAsync();

            return products;
        }

        public async Task<PagedResponseDTO<ProductListRecommendByCategoryDTO>> GetProductRecommendationsByCategory(int productId, ProductPagingRecommendByCategoryDTO filter)
        {
            // Get the category of the given product
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null || product.CategoryId == null)
            {
                return new PagedResponseDTO<ProductListRecommendByCategoryDTO>
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    TotalCount = 0,
                    Items = new List<ProductListRecommendByCategoryDTO>()
                };
            }

            // Get products of the same category excluding the current product
            var query = _context.Products
                .Where(p => p.CategoryId == product.CategoryId && p.ProductId != productId)
                .Select(p => new ProductListRecommendByCategoryDTO
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    ProductTitle = p.ProductTitle,
                    ProductDescription = p.ProductDescription,
                    ProductImage = p.ProductImages.Select(pi => pi.Image.ImageUrl).ToList(),
                    ProductPrice = p.ProductPrice,
                    AverageRating = p.Ratings.Any() ? p.Ratings.Average(r => r.RatingValue ?? 0) : 0
                })
                .AsQueryable();

            var totalCount = await query.CountAsync();
            var products = await query.Skip((filter.PageNumber - 1) * filter.PageSize)
                                      .Take(filter.PageSize)
                                      .ToListAsync();

            return new PagedResponseDTO<ProductListRecommendByCategoryDTO>
            {
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalCount = totalCount,
                Items = products
            };
        }

        public async Task<IEnumerable<ProductSuggestionDTO>> GetProductSuggestions(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return new List<ProductSuggestionDTO>();
            }

            var suggestions = await _context.Products
                .Where(p => p.ProductName.Contains(searchTerm))
                .Select(p => new ProductSuggestionDTO
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    ProductImage = p.ProductImages.Select(pi => pi.Image.ImageUrl).FirstOrDefault(),
                    ProductPrice = p.ProductPrice,
                    AverageRating = p.Ratings.Any() ? p.Ratings.Average(r => r.RatingValue ?? 0) : 0
                })
                .ToListAsync();

            return suggestions;
        }

    }
}
