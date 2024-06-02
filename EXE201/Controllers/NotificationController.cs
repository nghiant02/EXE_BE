using EXE201.BLL.Interfaces;
using EXE201.DAL.DTOs.NotificationDTOs;
using Microsoft.AspNetCore.Mvc;

namespace EXE201.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : Controller
    {
        private readonly INotificationServices _notificationService;

        public NotificationController(INotificationServices notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            var notifications = await _notificationService.GetAllNotificationsAsync();
            return Ok(notifications);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotification(int id)
        {
            var notification = await _notificationService.GetNotificationByIdAsync(id);
            if (notification == null)
            {
                return NotFound();
            }
            return Ok(notification);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] NotificationAddDto notificationDto)
        {
            await _notificationService.AddNotificationAsync(notificationDto);
            return CreatedAtAction(nameof(GetNotification), new { id = notificationDto.UserId }, notificationDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNotification(int id, [FromBody] NotificationEditDto notificationDto)
        {
            if (id != notificationDto.NotificationId)
            {
                return BadRequest();
            }
            await _notificationService.UpdateNotificationAsync(notificationDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            await _notificationService.DeleteNotificationAsync(id);
            return NoContent();
        }
    }
}
