using AutoMapper;
using EXE201.BLL.Interfaces;
using EXE201.DAL.DTOs;
using EXE201.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.BLL.Services
{
    public class RentalOrderServices : IRentalOrderServices
    {
        private readonly IRentalOrderRepository _rentalOrderRepository;
        private readonly IMapper _mapper;

        public RentalOrderServices(IRentalOrderRepository rentalOrderRepository, IMapper mapper)
        {
            _rentalOrderRepository = rentalOrderRepository;
            _mapper = mapper;
        }
        public async Task<ResponeModel> CancelOrderAsync(int orderId)
        {
            return await _rentalOrderRepository.CancelOrderAsync(orderId);
        }

        public async Task<ResponeModel> ReturnOrderAsync(int orderId, string returnReason)
        {
            return await _rentalOrderRepository.ReturnOrderAsync(orderId, returnReason);
        }
    }
}
