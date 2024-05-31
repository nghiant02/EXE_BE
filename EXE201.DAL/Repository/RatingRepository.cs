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
    public class RatingRepository : GenericRepository<Rating>, IRatingRepository
    {
        public RatingRepository(EXE201Context context) : base(context)
        {
        }

        public async Task<IEnumerable<Rating>> GetRatings()
        {
            try
            {
                var ratingList = await _context.Ratings
                    .Include(x => x.User)
                    .Include(x => x.Product)
                    .ToListAsync();

                return ratingList;
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
