using EXE201.DAL.DTOs.PaymentDTOs;
using EXE201.DAL.DTOs;
using EXE201.DAL.Models;
using MCC.DAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.DAL.Interfaces
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<ResponeModel> EnterPaymentDetails(EnterPaymentDetailsDTO paymentDetails);
        Task<ResponeModel> ProcessPayment(ProcessPaymentDTO processPayment);
    }
}
