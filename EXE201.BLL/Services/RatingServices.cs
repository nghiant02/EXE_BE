﻿using AutoMapper;
using EXE201.BLL.Interfaces;
using EXE201.DAL.DTOs.FeedbackDTOs;
using EXE201.DAL.Interfaces;
using EXE201.DAL.Models;
using EXE201.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.BLL.Services
{
    public class RatingServices : IRatingServices
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IMapper _mapper;

        public RatingServices(IRatingRepository ratingRepository, IMapper mapper)
        {
            _ratingRepository = ratingRepository;
            _mapper = mapper;
        }

        public async Task<Rating> AddRating(AddRatingDTO addRatingDTO)
        {
            try
            {
                var rating = _mapper.Map<Rating>(addRatingDTO);
                await _ratingRepository.AddAsync(rating);
                await _ratingRepository.SaveChangesAsync();
                return rating;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<IEnumerable<ProductRatingFeedbackDTO>> GetProductRatingsAndFeedback(int productId)
        {
            return await _ratingRepository.GetProductRatingsAndFeedback(productId);
        }

        public async Task<IEnumerable<Rating>> GetsApplicaition()
        {
            return await _ratingRepository.GetRatings();
        }

        public async Task<IEnumerable<UserRatingFeedbackDTO>> GetUserRatingsAndFeedback(int userId)
        {
            return await _ratingRepository.GetUserRatingsAndFeedback(userId);
        }

        public async Task<IEnumerable<ProductWithRatingsFeedbackDTO>> GetAllProductsWithRatingsFeedback()
        {
            return await _ratingRepository.GetAllProductsWithRatingsFeedback();
        }
    }
}
