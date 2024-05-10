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
    public class AddressServices : IAddressServices
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;

        public AddressServices(IAddressRepository addressRepository, IMapper mapper)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Address>> GetAddressByUserIdAsync(int userId)
        {
            return await _addressRepository.GetAddressByUserIdAsync(userId);
        }

        public async Task<IEnumerable<Address>> GetAllAddressAsync()
        {
            return await _addressRepository.GetAllAddressAsync();
        }
    }
}
