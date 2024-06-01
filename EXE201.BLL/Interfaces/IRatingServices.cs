using EXE201.DAL.DTOs.FeedbackDTOs;
using EXE201.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.BLL.Interfaces
{
    public interface IRatingServices
    {
        Task<Rating> AddRating(AddRatingDTO addRatingDTO);
        Task<IEnumerable<Rating>> GetsApplicaition();
        Task<IEnumerable<UserRatingFeedbackDTO>> GetUserRatingsAndFeedback(int userId);
        Task<IEnumerable<ProductRatingFeedbackDTO>> GetProductRatingsAndFeedback(int productId);
        Task<IEnumerable<ProductWithRatingsFeedbackDTO>> GetAllProductsWithRatingsFeedback();
    }
}
