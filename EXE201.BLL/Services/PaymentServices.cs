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

        public PaymentServices(IPaymentRepository paymentRepository, IRentalOrderRepository rentalOrderRepository, IRentalOrderDetailRepository rentalOrderDetailRepository, ICartRepository cartRepository, IInventoryRepository inventoryRepository, IMapper mapper)
        {
            _inventoryRepository = inventoryRepository;

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

        public async Task<ResponeModel> GetPaymentForUser(int paymentId)
        {
            // Fetch the payment details using the paymentId
            var payment = await _paymentRepository.GetPaymentById(paymentId);
            if (payment == null)
            {
                return new ResponeModel { Status = "Error", Message = "Payment not found" };
            }

            // Create and return the response with the payment details
            var paymentResponse = new PaymentResponseDTO
            {
                PaymentId = payment.PaymentId,
                UserId = payment.UserId ?? 0,
                PaymentAmount = payment.PaymentAmount,
                PaymentMethodName = payment.PaymentMethod?.PaymentMethodName,
                PaymentStatus = payment.PaymentStatus,
                FullName = payment.FullName,
                Phone = payment.Phone,
                Address = payment.Address,
                PaymentTime = payment.PaymentTime ?? DateTime.UtcNow,
                PaymentLink = payment.PaymentLink,
                Carts = payment.User?.Carts?.Select(c => new CartResponseDTO
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

            return new ResponeModel
            {
                Status = "Success",
                Message = "Payment details retrieved successfully",
                DataObject = paymentResponse
            };
        }


        private static Random random = new Random();
        public static string GenerateRandomNumericId(int length = 5)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        public async Task<ResponeModel> AddPaymentForUser(int userId, AddPaymentDTO paymentDetails, string successUrl, string cancelUrl, int orderCode)
        {
            var paymentResult = await _paymentRepository.AddPaymentForUser(userId, paymentDetails);
            if (paymentResult.Status == "Error")
            {
                return paymentResult;
            }

            var paymentData = paymentResult.DataObject as Payment;
            var paymentLink = paymentData.PaymentLink;
            //var randomPaymentId = GenerateRandomNumericId();

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
                    // Adjust the PaymentData constructor to match its parameters
                    var paymentPayload = new PaymentData(
                        orderCode: orderCode,  // Use the provided paymentId
                        amount: (int)(paymentData.PaymentAmount ?? 0),
                        description: "Thanh toán đơn hàng",
                        items: cartItems,
                        cancelUrl: cancelUrl,
                        returnUrl: successUrl
                    // Assuming these are optional, provide null or appropriate default
                    );

                    var createPaymentResult = await _payOSPaymentService.CreatePaymentLink(paymentPayload);
                    paymentLink = createPaymentResult.checkoutUrl;

                    // Update payment with new link
                    paymentData.PaymentLink = paymentLink;
                    await _paymentRepository.UpdatePayment(paymentData);
                }
            }

            var paymentResponse = new PaymentResponseDTO
            {
                PaymentId = paymentData.PaymentId,
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

            return new ResponeModel
            {
                Status = "Success",
                Message = "Payment created successfully",
                DataObject = paymentResponse,
            };
        }




        public async Task<ResponeModel> HandleSuccessfulPaymentAsync(int orderCode, string userId, string returnUrl)
        {
            // Update payment status to Success
            await UpdatePaymentStatus(orderCode, PaymentStatus.Completed);

            // Get the user's carts
            var userCarts = await _cartRepository.GetCartsByUserId(int.Parse(userId));

            if (userCarts == null || !userCarts.Any())
            {
                return new ResponeModel { Status = "Error", Message = "No items in cart" };
            }

            // Calculate order total using a more explicit conversion approach
            decimal orderTotal = userCarts.Sum(c =>
            {
                var productPrice = c.Product?.ProductPrice;
                decimal priceAsDecimal = productPrice.HasValue ? (decimal)productPrice.Value : 0m;
                return (c.Quantity ?? 0) * priceAsDecimal;
            });

            // Create a new rental order
            var rentalOrder = new RentalOrder
            {
                UserId = int.Parse(userId),
                OrderStatus = "Chờ xác nhận",
                OrderTotal = orderTotal,
                OrderCode = orderCode.ToString(),
                PaymentId = orderCode // Assuming the orderCode is the payment ID
            };

            // Add rental order to the database
            await _rentalOrderRepository.AddAsync(rentalOrder);
            await _rentalOrderRepository.SaveChangesAsync(); // Save to generate OrderId

            // Update the Payment table with the new OrderId
            var payment = await _paymentRepository.GetPaymentById(orderCode);
            if (payment != null)
            {
                payment.OrderId = rentalOrder.OrderId;
                await _paymentRepository.UpdatePayment(payment);
                await _paymentRepository.SaveChangesAsync();
            }

            // Add rental order details and update inventory
            foreach (var cart in userCarts)
            {
                if (cart == null || cart.ProductId == null || cart.Quantity == null)
                {
                    continue; // Skip any cart items with null Product or Quantity
                }

                // Add rental order detail
                var rentalOrderDetail = new RentalOrderDetail
                {
                    OrderId = rentalOrder.OrderId,
                    ProductId = cart.ProductId,
                    Quantity = cart.Quantity,
                    RentalStart = DateTime.UtcNow,
                    RentalEnd = DateTime.UtcNow.AddDays(7), // Example: 7-day rental period
                };
                await _rentalOrderDetailRepository.AddAsync(rentalOrderDetail);

                // Update inventory
                var inventory = await _inventoryRepository.GetInventoryByProductId((int)cart.ProductId);
                if (inventory != null)
                {
                    inventory.QuantityAvailable -= cart.Quantity ?? 0;
                    await _inventoryRepository.UpdateAsync(inventory);
                }
                else
                {
                    // Log or handle the case where inventory is not found
                    // Depending on the requirement, you can continue processing the remaining items
                    Console.WriteLine($"Warning: Inventory not found for Product ID: {cart.ProductId}");
                    continue; // Continue processing other cart items
                }

                // Delete the cart item after updating inventory
                await _cartRepository.DeleteCartById(cart.CartId);
            }

            // Save all changes
            await _rentalOrderDetailRepository.SaveChangesAsync();
            await _inventoryRepository.SaveChangesAsync();

            return new ResponeModel { Status = "Success", Message = "Payment processed and rental order created successfully" };
        }

        


        public async Task<PaymentResponseDTO> UpdatePaymentUrls(int paymentId, string returnUrl, string cancelUrl)
        {
            var payment = await _paymentRepository.GetPaymentById(paymentId);
            if (payment == null)
            {
                return null; // or handle error
            }

            // Directly set the URLs (assuming you can store them in the context of handling)
            // If you cannot modify the model, ensure that these URLs are handled appropriately where needed

            // Map updated payment to PaymentResponseDTO
            var paymentResponse = new PaymentResponseDTO
            {
                PaymentId = payment.PaymentId,
                UserId = payment.UserId ?? 0,
                PaymentAmount = payment.PaymentAmount,
                PaymentMethodName = payment.PaymentMethod?.PaymentMethodName,
                PaymentStatus = payment.PaymentStatus,
                FullName = payment.FullName,
                Phone = payment.Phone,
                Address = payment.Address,
                PaymentTime = payment.PaymentTime ?? DateTime.UtcNow,
                PaymentLink = payment.PaymentLink,
                ReturnUrl = returnUrl,
                CancelUrl = cancelUrl
            };

            return paymentResponse;
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

        public async Task<PaymentLinkInformation> GetPaymentLinkInformation(int orderId)
        {
            return await _payOSPaymentService.GetPaymentLinkInformation(orderId);
        }

        public async Task<Payment> UpdatePaymentStatus(int paymentId, PaymentStatus paymentStatus)
        {
            var result = await _paymentRepository.UpdatePaymentStatus(paymentId, paymentStatus);
            return result; // Return the Payment object directly
        }

    }

}
