using EXE201.DAL.DTOs;
using EXE201.DAL.DTOs.ProductDTOs;
using EXE201.DAL.Interfaces;
using EXE201.DAL.Models;
using MCC.DAL.Repository.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                return new ResponeModel { Status = "Success", Message = "Order cancelled successfully", DataObject = order };
            }
            return new ResponeModel { Status = "Error", Message = "Order not found or already cancelled" };
        }

        public async Task<ResponeModel> ReturnOrderAsync(int orderId, string returnReason)
        {
            var order = await GetByIdAsync(orderId);
            if (order != null && order.OrderStatus != "Returned")
            {
                order.OrderStatus = "Returned";
                order.ReturnDate = DateTime.Now;
                // Add returnReason to a new field if necessary
                Update(order);
                await SaveChangesAsync();
                return new ResponeModel { Status = "Success", Message = "Order returned successfully", DataObject = order };
            }
            return new ResponeModel { Status = "Error", Message = "Order not found or already returned" };
        }

        public async Task<ResponeModel> ReturnItem(ReturnItemDTO returnItem)
        {
            var order = await _context.RentalOrders.FindAsync(returnItem.OrderId);
            if (order != null && order.OrderStatus != "Returned")
            {
                order.OrderStatus = "Returned";
                order.ReturnDate = DateTime.Now;
                // Assuming there is a field for ReturnReason
                order.ReturnReason = returnItem.ReturnReason; 
                _context.RentalOrders.Update(order);
                await SaveChangesAsync();
                return new ResponeModel { Status = "Success", Message = "Item returned successfully", DataObject = order };
            }
            return new ResponeModel { Status = "Error", Message = "Order not found or already returned" };
        }
    }
}
