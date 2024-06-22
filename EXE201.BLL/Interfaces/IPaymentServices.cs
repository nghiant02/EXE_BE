using EXE201.DAL.DTOs.PaymentDTOs;
using EXE201.DAL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EXE201.DAL.Models;
using EXE201.DAL.DTOs.PaymentDTOs.EXE201.DAL.DTOs.PaymentDTOs;
using LMSystem.Repository.Helpers;

namespace EXE201.BLL.Interfaces
{
    public interface IPaymentServices
    {
        Task<ResponeModel> AddPaymentForUser(int userId, AddPaymentDTO paymentDetails);
        Task<ResponeModel> ConfirmPayment(int paymentId);
        Task<IEnumerable<ProfitDTO>> GetProfitData(DateTime startDate, DateTime endDate);
        Task<PagedResponseDTO<PaymentHistoryDto>> GetPaymentsByUserIdAsync(int userId, PaginationParameter paginationParameter);
    }
}
