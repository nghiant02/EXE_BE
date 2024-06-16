﻿using EXE201.DAL.DTOs.FeedbackDTOs;
using EXE201.DAL.DTOs.ProductDTOs;
using EXE201.DAL.Models;
using MCC.DAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.DAL.Interfaces
{
    public interface IRatingRepository : IGenericRepository<Rating>
    {
        Task<IEnumerable<Rating>> GetRatings();
        Task<IEnumerable<UserRatingFeedbackDTO>> GetUserRatingsAndFeedback(int userId);
        Task<ProductRatingFeedbackResponseDTO> GetProductRatingsAndFeedback(int productId, int? ratingFilter);
        Task<IEnumerable<ProductDetailDTO>> GetAllProductsWithRatingsFeedback();
    }
}
