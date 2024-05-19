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
    public class InventoryRepository : GenericRepository<Inventory>, IInventoryRepository
    {
        public InventoryRepository(EXE201Context context) : base(context)
        {
        }

        public async Task<IEnumerable<Inventory>> inventories()
        {
            try
            {
                var check = await _context.Inventories.Include(x => x.Product)
                    .ThenInclude(p => p.Category)
                    .OrderByDescending(x => x.InventoryId)
                    .ToListAsync();
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
