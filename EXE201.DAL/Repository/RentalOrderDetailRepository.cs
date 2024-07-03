using EXE201.DAL.DTOs;
using EXE201.DAL.DTOs.RentalOrderDTOs;
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
    public class RentalOrderDetailRepository : GenericRepository<RentalOrderDetail>, IRentalOrderDetailRepository
    {
        public RentalOrderDetailRepository(EXE201Context context) : base(context)
        {
        }

        public async Task<RentalOrderDetail> GetRentalOrderDetail(int id)
        {
            try
            {
                var check = await _context.RentalOrderDetails.Where(x => x.OrderDetailsId == id)
                    .Include(x => x.Order).ThenInclude(x => x.User)
                    .Include(x => x.Product)
                    .OrderByDescending(x => x.RentalStart)
                    .FirstOrDefaultAsync();
                if (check != null)
                {
                    return check;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<ViewRentalOrderDetail>> GetRentalOrderDetailByUserId(int id)
        {
            try
            {
                var check = await _context.RentalOrderDetails.Where(x => x.Order.UserId == id)
                    .Include(x => x.Order).ThenInclude(x => x.User).ThenInclude(payment => payment.Payments)
                    .Include(x => x.Product).ThenInclude(i => i.ProductImages)
                    .ThenInclude(productImage => productImage.Image)
                    .Include(rentalOrderDetail => rentalOrderDetail.Order)
                    .ThenInclude(rentalOrder => rentalOrder.Payments).ThenInclude(payment => payment.PaymentMethod)
                    .OrderByDescending(x => x.RentalStart)
                    .ToListAsync();
                if (check == null)
                {
                    throw new Exception($"No rental order detail found for Order ID {id}");
                }

                var listOrderDetails = new List<ViewRentalOrderDetail>();
                foreach (var rentalOrder in check)
                {
                    var viewOrderDetail = new ViewRentalOrderDetail
                    {
                        ProductImage = rentalOrder.Product.ProductImages.First().Image.ImageUrl,
                        ProductName = rentalOrder.Product.ProductName,
                        ProductQuantity = rentalOrder.Quantity,
                        RentalStart = rentalOrder.RentalStart,
                        RentalEnd = rentalOrder.RentalEnd,
                        Status = rentalOrder.Order.OrderStatus,
                        OrderCode = rentalOrder.Order.OrderCode,
                        Username = rentalOrder.Order.User.UserName,
                        Address = rentalOrder.Order.User.Address,
                        Phone = rentalOrder.Order.User.Phone,
                        PaymentType = rentalOrder.Order.Payments.First().PaymentMethod.PaymentMethodName
                    };

                    listOrderDetails.Add(viewOrderDetail);
                }

                return listOrderDetails;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<PagedResponseDTO<RentalOrderDetailResponseDTO>> GetPagedRentalOrderDetailsByUserId(int userId,
            int pageNumber, int pageSize)
        {
            var query = _context.RentalOrderDetails
                .Include(x => x.Product).ThenInclude(p => p.ProductImages).ThenInclude(pi => pi.Image)
                .Include(x => x.Order)
                .Include(t => t.Order).ThenInclude(q => q.Payments)
                .Where(x => x.Order.UserId == userId);

            var totalCount = await query.CountAsync();

            var rentalOrderDetails = await query
                .OrderByDescending(x => x.RentalStart)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new RentalOrderDetailResponseDTO()
                {
                    ProductName = x.Product.ProductName,
                    ProductImage = x.Product.ProductImages.FirstOrDefault() != null
                        ? x.Product.ProductImages.FirstOrDefault().Image.ImageUrl
                        : string.Empty,
                    RentalStart = x.RentalStart,
                    RentalEnd = x.RentalEnd,
                    Status = x.RentalEnd < DateTime.Now ? "Expired" : "Active",
                    OrderCode = x.Order.OrderCode,
                    PaymentTime = x.Order.Payments.First().PaymentTime
                })
                .ToListAsync();

            return new PagedResponseDTO<RentalOrderDetailResponseDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = rentalOrderDetails
            };
        }

        public async Task<RentalOrderDetail> UpdateRentalDetail(RentalOrderDetail rentalOrderDetail)
        {
            try
            {
                _context.RentalOrderDetails.Update(rentalOrderDetail);
                await _context.SaveChangesAsync();
                return rentalOrderDetail;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}