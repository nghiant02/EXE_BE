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
    public class RentalOrderDetailRepository : GenericRepository<RentalOrderDetail>, IRentalOrderDetailRepository
    {
        public RentalOrderDetailRepository(EXE201Context context) : base(context)
        {
        }

        public async Task<RentalOrderDetail> GetRentalOrderDetail(int id)
        {
            try
            {
                var check = await _context.RentalOrderDetails.Where(x => x.OrderDetailsId == id)
                    .Include(x => x.Order).ThenInclude(x => x.User)
                    .Include(x => x.Product)
                    .OrderByDescending(x => x.RentalStart)
                    .FirstOrDefaultAsync();
                if (check != null)
                {
                    return check;
                }
                return null;
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
