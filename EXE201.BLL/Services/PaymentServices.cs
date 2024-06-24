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
using EXE201.DAL.DTOs.PaymentDTOs.EXE201.DAL.DTOs.PaymentDTOs;
using Net.payOS.Types;
using LMSystem.Repository.Helpers;

namespace EXE201.BLL.Services
{
    public class PaymentServices : IPaymentServices
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;
        private readonly PayOSPaymentService _payOSPaymentService;

        public PaymentServices(IPaymentRepository paymentRepository, IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
            _payOSPaymentService = new PayOSPaymentService(
                "53e19b3d-c859-4415-b259-da5f00c609a7",
                "6272f3ad-8346-40cc-81ce-86fee5eb38fd",
                "e0130d2b3f563e10138c4ac0ca00ed32242cf6cbcfdbafd737b51391a28ea3ea"
            );
        }

        public async Task<ResponeModel> AddPaymentForUser(int userId, AddPaymentDTO paymentDetails)
        {
            var paymentResult = await _paymentRepository.AddPaymentForUser(userId, paymentDetails);
            if (paymentResult.Status == "Error")
            {
                return paymentResult;
            }

            var paymentData = paymentResult.DataObject as Payment;
            var paymentLink = (string)null;

            // Fetch the user's cart items
            var cartItems = paymentData.User?.Carts?.Select(c => new ItemData(
                name: c.Product.ProductName,
                quantity: c.Quantity ?? 1,
                price: (int)(c.Product.ProductPrice ?? 0)
            )).ToList() ?? new List<ItemData>();

            // ID for BankAccount
            const int bankAccountMethodId = 1;
            if (paymentDetails.PaymentMethodId == bankAccountMethodId)
            {
                var paymentPayload = new PaymentData(
                    orderCode: paymentData.PaymentId,
                    amount: (int)(paymentData.PaymentAmount ?? 0),
                    description: "Thanh toán đơn hàng",
                    items: cartItems,  // Use cart items here
                    cancelUrl: "https://voguary.id.vn/cart",
                    returnUrl: "https://voguary.id.vn/orderTracking"
                );

                var createPaymentResult = await _payOSPaymentService.CreatePaymentLink(paymentPayload);
                paymentLink = createPaymentResult.checkoutUrl;
            }

            var paymentResponse = new PaymentResponseDTO
            {
                PaymentId = paymentData.PaymentId,
                OrderId = paymentData.OrderId,
                UserId = paymentData.UserId ?? 0,
                PaymentAmount = paymentData.PaymentAmount,
                PaymentMethodName = paymentData.PaymentMethod?.PaymentMethodName,
                PaymentStatus = paymentData.PaymentStatus,
                FullName = paymentData.FullName,
                Phone = paymentData.Phone,
                Address = paymentData.Address,
                PaymentTime = paymentData.PaymentTime ?? DateTime.UtcNow,
                PaymentLink = paymentLink,
                Carts = paymentData.User?.Carts?.Select(c => new CartResponseDTO
                {
                    CartId = c.CartId,
                    UserId = c.UserId ?? 0,
                    ProductId = c.ProductId ?? 0,
                    Quantity = c.Quantity ?? 0,
                    Product = new ProductResponseDTO
                    {
                        ProductId = c.Product.ProductId,
                        ProductName = c.Product.ProductName,
                        ProductPrice = (decimal)(c.Product.ProductPrice ?? 0),
                        CategoryId = (int)c.Product.CategoryId
                    }
                }).ToList()
            };

            return new ResponeModel { Status = "Success", Message = "Payment created successfully", DataObject = paymentResponse };
        }


        public async Task<ResponeModel> ConfirmPayment(int paymentId)
        {
            var paymentResult = await _paymentRepository.ConfirmPayment(paymentId);
            return paymentResult;
        }

        public async Task<PagedResponseDTO<PaymentHistoryDto>> GetPaymentsByUserIdAsync(int userId, PaginationParameter paginationParameter)
        {
            return await _paymentRepository.GetPaymentHistoryByUserIdAsync(userId, paginationParameter);
        }

        public async Task<IEnumerable<ProfitDTO>> GetProfitData(DateTime startDate, DateTime endDate)
        {
            return await _paymentRepository.GetProfitData(startDate, endDate);
        }

        public async Task<IEnumerable<PaymentMethod>> GetAllPaymentMethods()
        {
            return await _paymentRepository.GetAllPaymentMethods();
        }

        public async Task<IEnumerable<Payment>> GetAllPayments()
        {
            return await _paymentRepository.GetAllPayments();
        }

        public async Task<PaymentMethod> CreatePaymentMethod(string paymentMethodName)
        {
            return await _paymentRepository.CreatePaymentMethod(paymentMethodName);
        }

        public async Task<PaymentMethod> UpdatePaymentMethodName(int paymentMethodId, string paymentMethodName)
        {
            return await _paymentRepository.UpdatePaymentMethodName(paymentMethodId, paymentMethodName);
        }

        public async Task<bool> DeletePaymentMethod(int paymentMethodId)
        {
            return await _paymentRepository.DeletePaymentMethod(paymentMethodId);
        }

        public async Task<bool> DeletePayment(int paymentId)
        {
            return await _paymentRepository.DeletePayment(paymentId);
        }
    }
}
