using AutoMapper;
using EXE201.BLL.Interfaces;
using EXE201.DAL.DTOs.PaymentDTOs;
using EXE201.DAL.DTOs;
using EXE201.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EXE201.DAL.Models;

namespace EXE201.BLL.Services
{
    public class PaymentServices : IPaymentServices
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;

        public PaymentServices(IPaymentRepository paymentRepository, IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
        }

        public async Task<ResponeModel> EnterPaymentDetails(EnterPaymentDetailsDTO paymentDetails)
        {
            return await _paymentRepository.EnterPaymentDetails(paymentDetails);
        }

        public async Task<ResponeModel> ProcessPayment(ProcessPaymentDTO processPayment)
        {
            return await _paymentRepository.ProcessPayment(processPayment);
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByUserIdAsync(int userId)
        {
            return await _paymentRepository.GetPaymentHistoryByUserIdAsync(userId);
        }

        public async Task<IEnumerable<ProfitDTO>> GetProfitData(DateTime startDate, DateTime endDate)
        {
            return await _paymentRepository.GetProfitData(startDate, endDate);
        }
    }
}
