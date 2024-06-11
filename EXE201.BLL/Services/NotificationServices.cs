using AutoMapper;
using EXE201.BLL.Interfaces;
using EXE201.DAL.DTOs.NotificationDTOs;
using EXE201.DAL.Interfaces;
using EXE201.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.BLL.Services
{
    public class NotificationServices : INotificationServices
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;

        public NotificationServices(INotificationRepository notificationRepository, IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NotificationDto>> GetAllNotificationsAsync()
        {
            var notifications = await _notificationRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<NotificationDto>>(notifications);
        }

        public async Task<NotificationDto> GetNotificationByIdAsync(int id)
        {
            var notification = await _notificationRepository.GetByIdAsync(id);
            return _mapper.Map<NotificationDto>(notification);
        }

        public async Task AddNotificationAsync(NotificationAddDto notificationDto)
        {
            var notification = _mapper.Map<Notification>(notificationDto);
            await _notificationRepository.AddAsync(notification);
            await _notificationRepository.SaveChangesAsync();
        }

        public async Task UpdateNotificationAsync(NotificationEditDto notificationDto)
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationDto.NotificationId);
            if (notification != null)
            {
                _mapper.Map(notificationDto, notification);
                _notificationRepository.Update(notification);
                await _notificationRepository.SaveChangesAsync();
            }
        }

        public async Task DeleteNotificationAsync(int id)
        {
            var notification = await _notificationRepository.GetByIdAsync(id);
            if (notification != null)
            {
                await _notificationRepository.Delete(notification);
                await _notificationRepository.SaveChangesAsync();
            }
        }
    }
}
