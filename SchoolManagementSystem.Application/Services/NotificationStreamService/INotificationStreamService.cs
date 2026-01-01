using Microsoft.AspNetCore.Http;
using SchoolManagementSystem.Application.DTOs.Notification;

namespace SchoolManagementSystem.Application.Services.NotificationStreamService
{
    public interface INotificationStreamService
    {
        void AddClient(string userId, HttpResponse response);
        Task NotifyUser(string userId, NotificationDto notification);
        void RemoveClient(string userId);
    }
}