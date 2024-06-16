using EXE201.DAL.DTOs;
using EXE201.DAL.DTOs.PaymentDTOs;
using EXE201.DAL.DTOs.PaymentDTOs.EXE201.DAL.DTOs.PaymentDTOs;
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
        private readonly EXE201Context _context;

        public PaymentRepository(EXE201Context context) : base(context)
        {
            _context = context;
        }
        public async Task<ResponeModel> AddPaymentForUser(int userId, AddPaymentDTO paymentDetails)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return new ResponeModel { Status = "Error", Message = "User not found" };
            }

            var cartItems = await _context.Carts
                .Where(c => c.UserId == userId)
                .Include(c => c.Product)
                .ToListAsync();

            if (!cartItems.Any())
            {
                return new ResponeModel { Status = "Error", Message = "No items in cart" };
            }

            var paymentAmount = cartItems.Sum(c => (decimal?)(c.Quantity * c.Product.ProductPrice ?? 0));

            var payment = new Payment
            {
                UserId = userId,
                PaymentAmount = paymentAmount,
                FullName = paymentDetails.FullName ?? user.FullName,
                Phone = paymentDetails.Phone ?? user.Phone,
                Address = paymentDetails.Address ?? user.Address,
                PaymentStatus = "Pending",
                PaymentTime = DateTime.UtcNow,
                PaymentMethodId = paymentDetails.PaymentMethodId
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            // Ensure PaymentMethod is included
            var paymentWithMethod = await _context.Payments
                .Include(p => p.PaymentMethod)  // Ensure PaymentMethod is included
                .Include(p => p.User)  // Include User for cart details
                .ThenInclude(u => u.Carts)
                .ThenInclude(c => c.Product)
                .Where(p => p.PaymentId == payment.PaymentId)
                .FirstOrDefaultAsync();

            if (paymentWithMethod == null)
            {
                return new ResponeModel { Status = "Error", Message = "Payment retrieval failed after creation" };
            }

            return new ResponeModel { Status = "Success", Message = "Payment created successfully", DataObject = paymentWithMethod };
        }

        public async Task ClearUserCart(int userId)
        {
            var cartItems = await _context.Carts
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (cartItems.Any())
            {
                _context.Carts.RemoveRange(cartItems);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ResponeModel> ConfirmPayment(int paymentId)
        {
            var payment = await _context.Payments
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.PaymentId == paymentId);

            if (payment == null)
            {
                return new ResponeModel { Status = "Error", Message = "Payment not found" };
            }

            if (payment.PaymentStatus == "Completed")
            {
                return new ResponeModel { Status = "Error", Message = "Payment is already completed" };
            }

            payment.PaymentStatus = "Completed";
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();

            await ClearUserCart((int)payment.UserId);

            return new ResponeModel { Status = "Success", Message = "Payment confirmed and cart cleared successfully" };
        }

        public async Task<IEnumerable<Payment>> GetPaymentHistoryByUserIdAsync(int userId)
        {
            return await _dbSet
                .Where(p => p.UserId == userId)
                .Select(p => new Payment
                {
                    PaymentId = p.PaymentId,
                    OrderId = p.OrderId,
                    UserId = p.UserId,
                    PaymentAmount = p.PaymentAmount,
                    PaymentMethod = p.PaymentMethod,
                    PaymentStatus = p.PaymentStatus
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ProfitDTO>> GetProfitData(DateTime startDate, DateTime endDate)
        {
            var profits = await _context.Payments
                .Where(p => p.PaymentStatus == "Completed" && p.Order.DatePlaced >= startDate && p.Order.DatePlaced <= endDate)
                .GroupBy(p => p.Order.DatePlaced.Value.Date)
                .Select(g => new ProfitDTO
                {
                    Date = g.Key,
                    TotalProfit = g.Sum(p => p.PaymentAmount ?? 0)
                })
                .ToListAsync();

            return profits;
        }
    }
}