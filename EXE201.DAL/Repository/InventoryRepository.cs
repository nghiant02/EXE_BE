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

        public async Task<(int, int, IEnumerable<Inventory>)> Inventories(int inventoryId, int pageNumber, int pageSize)
        {
            try
            {
                var totalRecord = await _context.Inventories.Where(x => x.InventoryId == inventoryId).CountAsync();
                var totalPage = (int)Math.Ceiling((double)totalRecord / pageSize);
                var inventories = await _context.Inventories
                    .Where(x => x.InventoryId == inventoryId)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                return (totalRecord, totalPage, inventories);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteInventory(int inventoryId)
        {
            try
            {
                var inventory = await _context.Inventories.FindAsync(inventoryId);
                if (inventory == null)
                {
                    return false;
                }

                _context.Inventories.Remove(inventory);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}