using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SchoolManagementSystem.Application.DTOs.Notification;
using System.Collections.Concurrent;
using System.Text.Json;

namespace SchoolManagementSystem.Application.Services.NotificationStreamService
{
    public class NotificationStreamService : INotificationStreamService
    {
        private readonly ConcurrentDictionary<string, List<HttpResponse>> clients = new();
        private readonly ILogger<NotificationStreamService> logger;

        public NotificationStreamService(ILogger<NotificationStreamService> logger)
        {
            this.logger = logger;
        }
        public void AddClient(string userId, HttpResponse response)
        {
            var responses = clients.GetOrAdd(userId, _ => new List<HttpResponse>());
            lock (responses)
            {
                responses.Add(response);
            }
            logger.LogInformation("Client connected: UserId={UserId}, Active connections={Count}", userId, responses.Count);
        }
        public async Task NotifyUser(string userId, NotificationDto notification)
        {
            if (!clients.TryGetValue(userId, out var responses))
            {
                logger.LogDebug("No active connections for user {UserId}", userId);
                return;
            }
            var json = JsonSerializer.Serialize(notification, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            var deadConnections = new List<HttpResponse>();

            foreach (var response in responses.ToList())
            {
                var httpContext = response.HttpContext;
                if (httpContext != null && !httpContext.RequestAborted.IsCancellationRequested)
                {
                    try
                    {
                        await response.WriteAsync($"data: {json}\n\n");
                        await response.Body.FlushAsync();
                    }
                    catch (Exception ex) when (ex is ObjectDisposedException
                                               or InvalidOperationException
                                               or IOException
                                               or OperationCanceledException)
                    {
                        logger.LogDebug(ex, "Connection failed for user {UserId}", userId);
                        deadConnections.Add(response);
                    }
                }
                else
                {
                    deadConnections.Add(response);
                }
            }
            foreach (var dead in deadConnections)
            {
                responses.Remove(dead);
            }

            if (responses.Count == 0)
                RemoveClient(userId);
        }
        public void RemoveClient(string userId)
        {
            if (clients.TryRemove(userId, out _))
            {
                logger.LogInformation("Removed all connections for user {UserId}", userId);
            }
        }
    }
}