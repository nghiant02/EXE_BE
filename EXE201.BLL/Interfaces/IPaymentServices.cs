using EXE201.DAL.DTOs.PaymentDTOs;
using EXE201.DAL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EXE201.DAL.Models;

namespace EXE201.BLL.Interfaces
{
    public interface IPaymentServices
    {
        Task<ResponeModel> EnterPaymentDetails(EnterPaymentDetailsDTO paymentDetails);
        Task<ResponeModel> ProcessPayment(ProcessPaymentDTO processPayment);
        Task< IEnumerable<Payment>> GetPaymentsByUserIdAsync(int userId);
        Task<IEnumerable<ProfitDTO>> GetProfitData(DateTime startDate, DateTime endDate);
    }
}
