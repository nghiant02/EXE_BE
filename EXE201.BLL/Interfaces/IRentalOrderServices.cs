using EXE201.DAL.DTOs;
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
    }
}
