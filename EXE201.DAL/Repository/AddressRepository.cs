using EXE201.DAL.Interfaces;
using EXE201.DAL.Models;
using MCC.DAL.Repository.Implements;
using MCC.DAL.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.DAL.Repository
{
    public class AddressRepository : GenericRepository<Address>, IAddressRepository
    {
        public AddressRepository(EXE201Context context) : base(context)
        {
        }

        public async Task<IEnumerable<Address>> GetAddressByUserIdAsync(int userId)
        {
            return await _dbSet.Where(x => x.UserID == userId).ToListAsync();
        }

        public async Task<IEnumerable<Address>> GetAllAddressAsync()
        {
            return await _dbSet.ToListAsync();
        }
    }
}
