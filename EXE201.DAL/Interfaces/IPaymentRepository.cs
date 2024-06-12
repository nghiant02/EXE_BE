using EXE201.DAL.DTOs.PaymentDTOs;
using EXE201.DAL.DTOs;
using EXE201.DAL.Models;
using MCC.DAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EXE201.DAL.DTOs.PaymentDTOs.EXE201.DAL.DTOs.PaymentDTOs;

namespace EXE201.DAL.Interfaces
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<ResponeModel> AddPaymentForUser(int userId, AddPaymentDTO paymentDetails);
        Task<ResponeModel> ConfirmPayment(int paymentId);
        //Task<PaymentMethod> GetPaymentMethodByName(string name);
        //Task<ResponeModel> EnterPaymentDetails(EnterPaymentDetailsDTO paymentDetails);
        //Task<ResponeModel> ProcessPayment(ProcessPaymentDTO processPayment);
        Task<IEnumerable<Payment>> GetPaymentHistoryByUserIdAsync(int userId);
        Task<IEnumerable<ProfitDTO>> GetProfitData(DateTime startDate, DateTime endDate);
    }
}
