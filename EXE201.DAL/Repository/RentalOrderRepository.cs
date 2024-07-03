using EXE201.DAL.DTOs;
using EXE201.DAL.DTOs.ProductDTOs;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EXE201.DAL.Repository
{
    public class RentalOrderRepository : GenericRepository<RentalOrder>, IRentalOrderRepository
    {
        public RentalOrderRepository(EXE201Context context) : base(context)
        {
        }

        public async Task<ResponeModel> CancelOrderAsync(int orderId)
        {
            var order = await GetByIdAsync(orderId);
            if (order != null && order.OrderStatus != "Cancelled")
            {
                order.OrderStatus = "Cancelled";
                Update(order);
                await SaveChangesAsync();
                return new ResponeModel
                    { Status = "Success", Message = "Order cancelled successfully", DataObject = order };
            }

            return new ResponeModel { Status = "Error", Message = "Order not found or already cancelled" };
        }

        //public async Task<ResponeModel> ReturnOrderAsync(int orderId, string returnReason)
        //{
        //    var order = await GetByIdAsync(orderId);
        //    if (order != null && order.OrderStatus != "Returned")
        //    {
        //        order.OrderStatus = "Returned";
        //        order.ReturnDate = DateTime.Now;
        //        // Add returnReason to a new field if necessary
        //        Update(order);
        //        await SaveChangesAsync();
        //        return new ResponeModel
        //            { Status = "Success", Message = "Order returned successfully", DataObject = order };
        //    }

        //    return new ResponeModel { Status = "Error", Message = "Order not found or already returned" };
        //}

        //public async Task<ResponeModel> ReturnItem(ReturnItemDTO returnItem)
        //{
        //    var order = await _context.RentalOrders.FindAsync(returnItem.OrderId);
        //    if (order != null && order.OrderStatus != "Returned")
        //    {
        //        order.OrderStatus = "Returned";
        //        order.ReturnDate = DateTime.Now;
        //        // Assuming there is a field for ReturnReason
        //        order.ReturnReason = returnItem.ReturnReason;
        //        _context.RentalOrders.Update(order);
        //        await SaveChangesAsync();
        //        return new ResponeModel
        //            { Status = "Success", Message = "Item returned successfully", DataObject = order };
        //    }

        //    return new ResponeModel { Status = "Error", Message = "Order not found or already returned" };
        //}

        //public async Task<OrderStatusDTO> GetOrderStatus(int orderId)
        //{
        //    var order = await _context.RentalOrders
        //        .Where(o => o.OrderId == orderId)
        //        .Select(o => new OrderStatusDTO
        //        {
        //            OrderId = o.OrderId,
        //            OrderStatus = o.OrderStatus,
        //            OrderDate = o.DueDate,
        //            ReturnDate = o.ReturnDate,
        //            ReturnReason = o.ReturnReason
        //        })
        //        .FirstOrDefaultAsync();

        //    return order;
        //}

        //public async Task<PagedResponseDTO<RentalOrderResponseDTO>> GetRentalOrdersByUserId(int userId, int PageNumber,
        //    int PageSize)
        //{
        //    var rentalOrders = _context.RentalOrders
        //        .Where(o => o.UserId == userId)
        //        .Select(o => new RentalOrderResponseDTO
        //        {
        //            OrderId = o.OrderId,
        //            OrderStatus = o.OrderStatus,
        //            DatePlaced = o.DatePlaced,
        //            DueDate = o.DueDate,
        //            ReturnDate = o.ReturnDate,
        //            OrderTotal = o.OrderTotal,
        //            PointsEarned = o.PointsEarned,
        //        })
        //        .AsQueryable();

        //    var totalCount = await rentalOrders.CountAsync();
        //    var products = await rentalOrders.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToListAsync();

        //    return new PagedResponseDTO<RentalOrderResponseDTO>
        //    {
        //        PageNumber = PageNumber,
        //        PageSize = PageSize,
        //        TotalCount = totalCount,
        //        Items = products
        //    };
        //}

        //public async Task<RentalOrderResponseDTO> GetRentalByUserId(int userId)
        //{
        //    var rentalOrder = await _context.RentalOrders
        //        .Where(r => r.UserId == userId)
        //        .Include(d => d.RentalOrderDetails).ThenInclude(p => p.Product)
        //        .Select(r => new RentalOrderResponseDTO
        //        {
        //            OrderId = r.OrderId,
        //            OrderStatus = r.OrderStatus,
        //            DatePlaced = r.DatePlaced,
        //            DueDate = r.DatePlaced,
        //            OrderTotal = r.OrderTotal,
        //            PointsEarned = r.PointsEarned,
        //            ReturnDate = r.ReturnDate,
        //            ProductImage = r.RentalOrderDetails.First().Product.ProductImages.First().Image.ImageUrl
        //        }).FirstOrDefaultAsync();
        //    return rentalOrder;
        //}
        
        public async Task<RentalOrder> UpdateRental(RentalOrder rentalOrder)
        {
            try
            {
                _context.RentalOrders.Update(rentalOrder);
                await _context.SaveChangesAsync();
                return rentalOrder;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<(int, int, IEnumerable<ViewRentalOrderDto>)> RentalOrdersByStatus(string status, int pageNumber, int pageSize)
        {
            try
            {
                var totalRecord = await _context.RentalOrders.Where(x => x.OrderStatus == status).CountAsync();
                var totalPage = (int)Math.Ceiling((double)totalRecord / pageSize);

                var rentalOrders = await _context.RentalOrders
                    .Where(x => x.OrderStatus == status)
                    .Include(d => d.RentalOrderDetails).ThenInclude(p => p.Product)
                    .Include(u => u.User)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                var viewRentalOrders = new List<ViewRentalOrderDto>();
                foreach (var rentalOrder in rentalOrders)
                {

                    var viewRentalOrderResponseDto =  new ViewRentalOrderDto()
                    {
                        OrderId = rentalOrder.OrderId,
                        OrderStatus = rentalOrder.OrderStatus,
                        DatePlaced = rentalOrder.RentalOrderDetails.First().RentalStart,
                        DueDate = rentalOrder.RentalOrderDetails.First().DueDate,
                        ReturnDate = rentalOrder.RentalOrderDetails.First().RentalEnd,
                        MoneyReturned = rentalOrder.OrderTotal,
                        ProductName = rentalOrder.RentalOrderDetails.First().Product.ProductName,
                        Username = rentalOrder.User.UserName
                    };
                    
                    viewRentalOrders.Add(viewRentalOrderResponseDto);
                }  

               
                return (totalRecord, totalPage, viewRentalOrders);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        public async Task<(int, int, IEnumerable<ViewRentalOrderDto>)> RentalOrders(int pageNumber, int pageSize)
        {
            try
            {
                var totalRecord = await _context.RentalOrders.CountAsync();
                var totalPage = (int)Math.Ceiling((double)totalRecord / pageSize);

                var rentalOrders = await _context.RentalOrders
                    .Include(d => d.RentalOrderDetails).ThenInclude(p => p.Product)
                    .Include(u => u.User)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                var viewRentalOrders = new List<ViewRentalOrderDto>();
                foreach (var rentalOrder in rentalOrders)
                {

                    var viewRentalOrderResponseDto =  new ViewRentalOrderDto()
                    {
                        OrderId = rentalOrder.OrderId,
                        OrderStatus = rentalOrder.OrderStatus,
                        DatePlaced = rentalOrder.RentalOrderDetails.First().RentalStart,
                        DueDate = rentalOrder.RentalOrderDetails.First().DueDate,
                        ReturnDate = rentalOrder.RentalOrderDetails.First().RentalEnd,
                        MoneyReturned = rentalOrder.OrderTotal,
                        ProductName = rentalOrder.RentalOrderDetails.First().Product.ProductName,
                        Username = rentalOrder.User.UserName
                    };
                    
                    viewRentalOrders.Add(viewRentalOrderResponseDto);
                }  

               
                return (totalRecord, totalPage, viewRentalOrders);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}