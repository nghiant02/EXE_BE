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
    public class MembershipRepository : GenericRepository<Membership>, IMembershipRepository
    {
        public MembershipRepository(EXE201Context context) : base(context)
        {
        }

        public async Task<IEnumerable<Membership>> GetAll()
        {
            try
            {
                var membership = await _context.Memberships
                    .Include(x => x.User)
                    .Include(x => x.MembershipType)
                    .ToListAsync();
                return membership;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
