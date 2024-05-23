using EXE201.DAL.DTOs;
using EXE201.DAL.DTOs.PaymentDTOs;
using EXE201.DAL.Interfaces;
using EXE201.DAL.Models;
using MCC.DAL.Repository.Implements;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.DAL.Repository
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(EXE201Context context) : base(context)
        {
        }
        public async Task<ResponeModel> EnterPaymentDetails(EnterPaymentDetailsDTO paymentDetails)
        {
            var order = await _context.RentalOrders.FindAsync(paymentDetails.OrderId);
            if (order != null)
            {
                var payment = new Payment
                {
                    OrderId = paymentDetails.OrderId,
                    Amount = paymentDetails.Amount,
                    PaymentMethod = "Pending",
                    PaymentStatus = "Pending"
                };
                await _context.Payments.AddAsync(payment);
                await SaveChangesAsync();
                return new ResponeModel { Status = "Success", Message = "Payment details entered successfully", DataObject = payment };
            }
            return new ResponeModel { Status = "Error", Message = "Order not found" };
        }

        public async Task<ResponeModel> ProcessPayment(ProcessPaymentDTO processPayment)
        {
            var payment = await _context.Payments
                .FirstOrDefaultAsync(p => p.OrderId == processPayment.OrderId && p.PaymentStatus == "Pending");
            if (payment != null)
            {
                payment.PaymentMethod = processPayment.PaymentMethod;
                payment.PaymentStatus = processPayment.Confirm ? "Confirmed" : "Pending";
                _context.Payments.Update(payment);

                if (processPayment.Confirm)
                {
                    var order = await _context.RentalOrders.FindAsync(processPayment.OrderId);
                    if (order != null)
                    {
                        order.OrderStatus = "Paid";
                        _context.RentalOrders.Update(order);
                    }
                }

                await SaveChangesAsync();
                return new ResponeModel { Status = "Success", Message = "Payment processed successfully", DataObject = payment };
            }
            return new ResponeModel { Status = "Error", Message = "Pending payment not found" };
        }

        public async Task<IEnumerable<Payment>> GetPaymentHistoryByUserIdAsync(int userId)
        {
            return await _dbSet
                .Where(p => p.PaymentUserId == userId)
                .Select(p => new Payment
                {
                    PaymentId = p.PaymentId,
                    OrderId = p.OrderId,
                    PaymentUserId = p.PaymentUserId,
                    Amount = p.Amount,
                    PaymentMethod = p.PaymentMethod,
                    PaymentStatus = p.PaymentStatus
                })
                .ToListAsync();
        }
    }
}
