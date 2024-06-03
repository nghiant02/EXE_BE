using EXE201.DAL.DTOs;
using EXE201.DAL.DTOs.PaymentDTOs;
using EXE201.DAL.Helpers;
using EXE201.DAL.Interfaces;
using EXE201.DAL.Models;
using MCC.DAL.Repository.Implements;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        private readonly IConfiguration _configuration;
        private readonly ILogger<PaymentRepository> _logger;



        public PaymentRepository(EXE201Context context, IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<PaymentRepository> logger) : base(context)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
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

            string paymentInformation = await GeneratePaymentInformationAsync(paymentDetails.PaymentMethod, payment.PaymentId, paymentDetails.Amount);

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

        private async Task<string> GeneratePaymentInformationAsync(string paymentMethod, int paymentId, decimal amount)
        {
            if (paymentMethod == "VNPay")
            {
                return GenerateVNPayInformation(paymentId, amount);
            }
            else if (paymentMethod == "MoMo")
            {
                return await GenerateMoMoInformationAsync(paymentId, amount);
            }
            else if (paymentMethod == "BankTransfer")
            {
                return "Bank transfer details: Account number: 123456789, Bank: ABC Bank";
            }

            return string.Empty;
        }

        private string GenerateVNPayInformation(int paymentId, decimal amount)
        {
            var vnPayLibrary = new VnPayLibrary();
            var vnpUrl = _configuration["VNPay:BaseUrl"];
            var vnpTmnCode = _configuration["VNPay:TmnCode"];
            var vnpHashSecret = _configuration["VNPay:HashSecret"];

            vnPayLibrary.AddRequestData("vnp_Version", "2.1.0");
            vnPayLibrary.AddRequestData("vnp_Command", "pay");
            vnPayLibrary.AddRequestData("vnp_TmnCode", vnpTmnCode);
            vnPayLibrary.AddRequestData("vnp_Amount", ((int)(amount * 100)).ToString());
            vnPayLibrary.AddRequestData("vnp_CurrCode", "VND");
            vnPayLibrary.AddRequestData("vnp_TxnRef", paymentId.ToString());
            vnPayLibrary.AddRequestData("vnp_OrderInfo", "Payment for Order #" + paymentId);
            vnPayLibrary.AddRequestData("vnp_Locale", "vn");
            vnPayLibrary.AddRequestData("vnp_ReturnUrl", _configuration["VNPay:ReturnUrl"]);
            vnPayLibrary.AddRequestData("vnp_IpAddr", "127.0.0.1");
            vnPayLibrary.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnPayLibrary.AddRequestData("vnp_ExpireDate", DateTime.Now.AddMinutes(15).ToString("yyyyMMddHHmmss"));

            return vnPayLibrary.CreateRequestUrl(vnpUrl, vnpHashSecret);
        }


        private async Task<string> GenerateMoMoInformationAsync(int paymentId, decimal amount)
        {
            //var httpClient = _httpClientFactory.CreateClient();

            //// MoMo API endpoint and required parameters
            //var requestUri = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
            //var parameters = new Dictionary<string, string>
            //{
            //    { "partnerCode", "YOUR_PARTNER_CODE" },
            //    { "accessKey", "YOUR_ACCESS_KEY" },
            //    { "requestId", Guid.NewGuid().ToString() },
            //    { "amount", amount.ToString() },
            //    { "orderId", paymentId.ToString() },
            //    { "orderInfo", "Payment for Order #" + paymentId },
            //    { "returnUrl", "https://your-return-url" },
            //    { "notifyUrl", "https://your-notify-url" },
            //    { "extraData", "" }
            //};

            //// Generate signature
            //var rawData = $"partnerCode={parameters["partnerCode"]}&accessKey={parameters["accessKey"]}&requestId={parameters["requestId"]}&amount={parameters["amount"]}&orderId={parameters["orderId"]}&orderInfo={parameters["orderInfo"]}&returnUrl={parameters["returnUrl"]}&notifyUrl={parameters["notifyUrl"]}&extraData={parameters["extraData"]}";
            //var signature = GenerateMoMoSignature(rawData, "YOUR_SECRET_KEY");
            //parameters.Add("signature", signature);

            //var content = new FormUrlEncodedContent(parameters);
            //var response = await httpClient.PostAsync(requestUri, content);
            //var responseContent = await response.Content.ReadAsStringAsync();

            //// Extract the payUrl from the response
            //var responseJson = Newtonsoft.Json.Linq.JObject.Parse(responseContent);
            //return responseJson["payUrl"].ToString();
            return null;
        }

        private string GenerateMoMoSignature(string rawData, string secretKey)
        {
            var hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));
            var hash = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
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
