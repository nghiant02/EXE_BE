using EXE201.DAL.DTOs.NotificationDTOs;
using EXE201.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.BLL.Interfaces
{
    public interface INotificationServices
    {
        Task<IEnumerable<NotificationDto>> GetAllNotificationsAsync();
        Task<NotificationDto> GetNotificationByIdAsync(int id);
        Task AddNotificationAsync(NotificationAddDto notificationDto);
        Task UpdateNotificationAsync(NotificationEditDto notificationDto);
        Task DeleteNotificationAsync(int id);
    }
}
