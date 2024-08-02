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
using Net.payOS;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;
using EXE201.DAL.Repository;

namespace EXE201.BLL.Services
{
    public class PaymentServices : IPaymentServices
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IRentalOrderRepository _rentalOrderRepository;
        private readonly IRentalOrderDetailRepository _rentalOrderDetailRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;
        private readonly PayOSPaymentService _payOSPaymentService;

        public PaymentServices(IPaymentRepository paymentRepository, IRentalOrderRepository rentalOrderRepository, IRentalOrderDetailRepository rentalOrderDetailRepository, ICartRepository cartRepository, IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _rentalOrderRepository = rentalOrderRepository;
            _rentalOrderDetailRepository = rentalOrderDetailRepository;
            _cartRepository = cartRepository;
            _mapper = mapper;
            _payOSPaymentService = new PayOSPaymentService(
                "53e19b3d-c859-4415-b259-da5f00c609a7",
                "6272f3ad-8346-40cc-81ce-86fee5eb38fd",
                "e0130d2b3f563e10138c4ac0ca00ed32242cf6cbcfdbafd737b51391a28ea3ea"
            );
        }
        private static Random random = new Random();

        public async Task<ResponeModel> AddPaymentForUser(int userId, AddPaymentDTO paymentDetails)
        {
            var paymentResult = await _paymentRepository.AddPaymentForUser(userId, paymentDetails);
            if (paymentResult.Status == "Error")
            {
                return paymentResult;
            }

            var paymentData = paymentResult.DataObject as Payment;
            var paymentLink = paymentData.PaymentLink; // Initialize with existing link

            // Generate a random 5-character numeric ID
            var randomPaymentId = GenerateRandomNumericId();

            // Check if payment link already exists
            if (string.IsNullOrEmpty(paymentLink))
            {
                var cartItems = paymentData.User?.Carts?.Select(c => new ItemData(
                    name: c.Product.ProductName,
                    quantity: c.Quantity ?? 1,
                    price: (int)(c.Product.ProductPrice ?? 0)
                )).ToList() ?? new List<ItemData>();

                const int bankAccountMethodId = 1;
                if (paymentDetails.PaymentMethodId == bankAccountMethodId)
                {
                    var paymentPayload = new PaymentData(
                        orderCode: long.Parse(randomPaymentId),  // Convert the generated random ID to long
                        amount: (int)(paymentData.PaymentAmount ?? 0),
                        description: "Thanh toán đơn hàng",
                        items: cartItems,
                        cancelUrl: $"https://voguary.id.vn/Checkout?handler=PaymentCallback&success=false&paymentId={paymentData.PaymentId}",
                        returnUrl: $"https://voguary.id.vn/Checkout?handler=PaymentCallback&success=true&paymentId={paymentData.PaymentId}"
                    );

                    var createPaymentResult = await _payOSPaymentService.CreatePaymentLink(paymentPayload);
                    paymentLink = createPaymentResult.checkoutUrl;

                    // Update payment with new link
                    paymentData.PaymentLink = paymentLink;
                    await _paymentRepository.UpdatePayment(paymentData);
                }
            }

            // Create a new rental order
            var rentalOrder = new RentalOrder
            {
                UserId = userId,
                OrderStatus = "Pending", // Set the initial status as "Pending"
                OrderTotal = paymentData.PaymentAmount,
                OrderCode = randomPaymentId, // Use the random ID as the order code
                PaymentId = paymentData.PaymentId // Set the PaymentId in RentalOrder
            };

            await _rentalOrderRepository.AddAsync(rentalOrder);
            await _rentalOrderRepository.SaveChangesAsync();

            // Update the Payment to reference the new RentalOrder's ID
            paymentData.OrderId = rentalOrder.OrderId;
            await _paymentRepository.UpdatePayment(paymentData);

            // Add rental order details for each cart item
            foreach (var cartItem in paymentData.User.Carts)
            {
                var rentalOrderDetail = new RentalOrderDetail
                {
                    OrderId = rentalOrder.OrderId,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    RentalStart = DateTime.UtcNow, // Assuming rental starts immediately
                    RentalEnd = DateTime.UtcNow.AddDays(7) // Example: rental ends in 7 days
                };
                await _rentalOrderDetailRepository.AddAsync(rentalOrderDetail);
            }

            // Save changes to rental order details
            await _rentalOrderDetailRepository.SaveChangesAsync();

            var paymentResponse = new PaymentResponseDTO
            {
                PaymentId = paymentData.PaymentId,
                OrderId = rentalOrder.OrderId, // Set the OrderId to the new rental order
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


        public async Task UpdatePaymentStatusAndClearCartAsync(int paymentId, string status)
        {
            var payment = await _paymentRepository.GetByIdAsync(paymentId);
            if (payment != null)
            {
                payment.PaymentStatus = status;
                await _paymentRepository.SaveChangesAsync();

                if (status == "Completed")
                {
                    var userCarts = await _cartRepository.GetCartsByUserId((int)payment.UserId);
                    foreach (var cart in userCarts)
                    {
                        // Update inventory for each product in the cart
                        var inventory = await _inventoryRepository.GetInventoryByProductId((int)cart.ProductId);
                        if (inventory != null)
                        {
                            inventory.QuantityAvailable -= cart.Quantity ?? 0; // Subtract the purchased quantity
                            await _inventoryRepository.UpdateAsync(inventory);
                        }

                        // Delete the cart item after updating inventory
                        await _cartRepository.DeleteCartById(cart.CartId);
                    }

                    // Save all changes to the inventory
                    await _inventoryRepository.SaveChangesAsync();
                }
            }
        }


        // Generate a random 5-character numeric ID
        public static string GenerateRandomNumericId(int length = 5)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
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

        public async Task<PaymentLinkInformation> CancelPaymentLink(int paymentId)
        {
            return await _payOSPaymentService.CancelPaymentLink(paymentId);
        }
    }

}
