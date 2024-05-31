using EXE201.BLL.Interfaces;
using EXE201.DAL.DTOs.FeedbackDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EXE201.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IRatingServices _ratingServices;
        public RatingController(IRatingServices ratingServices)
        {
            _ratingServices = ratingServices;
        }

        [HttpGet("GetAllRatings")]
        public async Task<IActionResult> Get()
        {
            var result = await _ratingServices.GetsApplicaition();
            return Ok(result);
        }

        [HttpPost("AddRating")]
        public async Task<IActionResult> addRating(AddRatingDTO addRatingDTO)
        {
            var result = await _ratingServices.AddRating(addRatingDTO);
            return Ok(result);
        }
    }
}
