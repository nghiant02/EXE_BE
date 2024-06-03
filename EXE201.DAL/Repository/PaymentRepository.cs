using EXE201.DAL.DTOs;
using EXE201.DAL.DTOs.PaymentDTOs;
using EXE201.DAL.Interfaces;
using EXE201.DAL.Models;
using MCC.DAL.Repository.Implements;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.DAL.Repository
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        private readonly EXE201Context _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public PaymentRepository(EXE201Context context, IHttpClientFactory httpClientFactory) : base(context)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ResponeModel> EnterPaymentDetails(EnterPaymentDetailsDTO paymentDetails)
        {
            var order = await _context.RentalOrders.FindAsync(paymentDetails.OrderId);
            if (order == null)
            {
                return new ResponeModel { Status = "Error", Message = "Order not found" };
            }

            var payment = new Payment
            {
                OrderId = paymentDetails.OrderId,
                UserId = paymentDetails.UserId,
                PaymentAmount = paymentDetails.Amount,
                PaymentMethod = paymentDetails.PaymentMethod,
                PaymentStatus = "Pending"
            };

            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();

            string paymentInformation = await GeneratePaymentInformationAsync(paymentDetails.PaymentMethod, payment.PaymentId);

            return new ResponeModel
            {
                Status = "Success",
                Message = "Payment details entered successfully",
                DataObject = new PaymentResponseDTO { PaymentId = payment.PaymentId, PaymentInformation = paymentInformation }
            };
        }

        public async Task<ResponeModel> ProcessPayment(ProcessPaymentDTO processPayment)
        {
            var payment = await _context.Payments
                .FirstOrDefaultAsync(p => p.PaymentId == processPayment.PaymentId && p.PaymentStatus == "Pending");
            if (payment == null)
            {
                return new ResponeModel { Status = "Error", Message = "Pending payment not found" };
            }

            payment.PaymentStatus = processPayment.Confirm ? "Confirmed" : "Pending";
            _context.Payments.Update(payment);

            if (processPayment.Confirm)
            {
                var order = await _context.RentalOrders.FindAsync(payment.OrderId);
                if (order != null)
                {
                    order.OrderStatus = "Paid";
                    _context.RentalOrders.Update(order);
                }
            }

            await _context.SaveChangesAsync();
            return new ResponeModel { Status = "Success", Message = "Payment processed successfully", DataObject = payment };
        }

        private async Task<string> GeneratePaymentInformationAsync(string paymentMethod, int paymentId)
        {
            if (paymentMethod == "VNPay")
            {
                return await GenerateVNPayInformationAsync(paymentId);
            }
            else if (paymentMethod == "BankTransfer")
            {
                return "Bank transfer details: Account number: 123456789, Bank: ABC Bank";
            }

            return string.Empty;
        }

        private async Task<string> GenerateVNPayInformationAsync(int paymentId)
        {
            var httpClient = _httpClientFactory.CreateClient();

            var requestUri = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
            var parameters = new Dictionary<string, string>
        {
            { "vnp_Version", "2.1.0" },
            { "vnp_Command", "pay" },
            { "vnp_TmnCode", "E2SEF40W" },
            { "vnp_Amount", "100000" }, // Example amount
            { "vnp_CurrCode", "VND" },
            { "vnp_TxnRef", paymentId.ToString() },
            { "vnp_OrderInfo", "Payment for Order #" + paymentId },
            { "vnp_Locale", "vn" },
            { "vnp_ReturnUrl", "https://voguary.id.vn/" },
            { "vnp_IpAddr", "127.0.0.1" },
            { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") }
        };

            var secureHash = GenerateVNPaySecureHash(parameters, "0NXFXNAGIO1DHF58X4J4RJQ3GMLZXN4H");
            parameters.Add("vnp_SecureHash", secureHash);

            var query = new FormUrlEncodedContent(parameters).ReadAsStringAsync().Result;
            return $"{requestUri}?{query}";
        }

        private string GenerateVNPaySecureHash(Dictionary<string, string> parameters, string secretKey)
        {
            var sortedParameters = parameters.OrderBy(kvp => kvp.Key).ToList();
            var data = string.Join("&", sortedParameters.Select(kvp => $"{kvp.Key}={kvp.Value}"));
            var hash = new HMACSHA512(Encoding.UTF8.GetBytes(secretKey)).ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
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
