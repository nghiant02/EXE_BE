using EXE201.DAL.DTOs;
using EXE201.DAL.DTOs.ProductDTOs;
using EXE201.DAL.Models;
using MCC.DAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.DAL.Interfaces
{
    public interface IRentalOrderRepository : IGenericRepository<RentalOrder>
    {
        Task<ResponeModel> CancelOrderAsync(int orderId);
        Task<ResponeModel> ReturnOrderAsync(int orderId, string returnReason);
        Task<ResponeModel> ReturnItem(ReturnItemDTO returnItem);
    }
}
