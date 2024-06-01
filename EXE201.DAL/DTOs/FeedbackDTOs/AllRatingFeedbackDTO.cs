using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.DAL.DTOs.FeedbackDTOs
{
    public class RatingFeedbackDTO
    {
        public int RatingId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int RatingValue { get; set; }
        public DateTime? DateGiven { get; set; }
        public string FeedbackComment { get; set; }
        public string FeedbackImage { get; set; }
    }

    public class ProductWithRatingsFeedbackDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductImage { get; set; }
        public double? ProductPrice { get; set; }
        public string ProductSize { get; set; }
        public string ProductColor { get; set; }
        public string ProductStatus { get; set; }
        public string CategoryName { get; set; }
        public double AverageRating { get; set; }
        public List<RatingFeedbackDTO> RatingsFeedback { get; set; }
    }
}
