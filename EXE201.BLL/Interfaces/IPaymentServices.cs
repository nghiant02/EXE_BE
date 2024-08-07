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
using Net.payOS.Types;

namespace EXE201.BLL.Interfaces
{
    public interface IPaymentServices
    {
        Task<ResponeModel> GetPaymentForUser(int paymentId);
        Task<ResponeModel> AddPaymentForUser(int userId, AddPaymentDTO paymentDetails, string successUrl, string cancelUrl, int orderCode);
        Task<ResponeModel> ConfirmPayment(int paymentId);
        //Task<IEnumerable<ProfitDTO>> GetProfitData(DateTime startDate, DateTime endDate);
        Task<PagedResponseDTO<PaymentHistoryDto>> GetPaymentsByUserIdAsync(int userId, PaginationParameter paginationParameter);
        Task<IEnumerable<PaymentMethod>> GetAllPaymentMethods();
        Task<IEnumerable<Payment>> GetAllPayments();
        Task<PaymentMethod> CreatePaymentMethod(string paymentMethodName);
        Task<PaymentMethod> UpdatePaymentMethodName(int paymentMethodId, string paymentMethodName);
        Task<bool> DeletePaymentMethod(int paymentMethodId);
        Task<bool> DeletePayment(int paymentId);
        Task<PaymentLinkInformation> CancelPaymentLink(int paymentId);
        //Task UpdatePaymentStatusAndClearCartAsync(int paymentId, string status);
        Task<PaymentLinkInformation> GetPaymentLinkInformation(int orderId);
        Task<Payment> UpdatePaymentStatus(int paymentId, PaymentStatus paymentStatus);
        Task<ResponeModel> HandleSuccessfulPaymentAsync(int orderCode, string userId, string returnUrl);
        Task<PaymentResponseDTO> UpdatePaymentUrls(int orderCode, string returnUrl, string cancelUrl);
    }

}
