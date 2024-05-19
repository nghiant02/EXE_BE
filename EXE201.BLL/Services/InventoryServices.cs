using AutoMapper;
using EXE201.BLL.Interfaces;
using EXE201.DAL.Interfaces;
using EXE201.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.BLL.Services
{
    public class InventoryServices : IInventoryServices
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IMapper _mapper;

        public InventoryServices(IInventoryRepository inventoryRepository, IMapper mapper)
        {
            _inventoryRepository = inventoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Inventory>> GetInventories()
        {
            var listInventories = await _inventoryRepository.inventories();
            return listInventories;
        }
    }
}
