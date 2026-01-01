using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Application.Services.NotificationStreamService;
using SchoolManagementSystem.Web.Extensions;

namespace SchoolManagementSystem.Web.Controllers.StudentControllers
{
    [Route("api/student/[controller]")]
    [ApiController]
    [Authorize(Roles = "Student")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationStreamService notificationStreamService;

        public NotificationsController(INotificationStreamService notificationStreamService)
        {
            this.notificationStreamService = notificationStreamService;
        }

        [HttpGet("stream")]
        public async Task SendNotification(CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            Response.Headers.Add("Content-Type", "text/event-stream");
            Response.Headers.Add("Cache-Control", "no-cache");
            Response.Headers.Add("Connection", "keep-alive");

            await Response.StartAsync(cancellationToken);
            notificationStreamService.AddClient(userId, Response);
            try
            {
                var tcs = new TaskCompletionSource();
                HttpContext.RequestAborted.Register(() => tcs.TrySetResult());
                await tcs.Task;
            }
            finally
            {
                notificationStreamService.RemoveClient(userId);
            }
        }
    }
}