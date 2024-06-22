using EXE201.DAL.DTOs;
using EXE201.DAL.DTOs.ProductDTOs;
using EXE201.DAL.DTOs.RentalOrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.BLL.Interfaces
{
    public interface IRentalOrderServices
    {
        Task<ResponeModel> CancelOrderAsync(int orderId);
        Task<ResponeModel> ReturnOrderAsync(int orderId, string returnReason);
        Task<ResponeModel> ReturnItem(ReturnItemDTO returnItem);
        Task<OrderStatusDTO> GetOrderStatus(int orderId);
        Task<PagedResponseDTO<RentalOrderResponseDTO>> GetRentalOrdersByUserId(int userId, int PageNumber, int PageSize);


    }
}
