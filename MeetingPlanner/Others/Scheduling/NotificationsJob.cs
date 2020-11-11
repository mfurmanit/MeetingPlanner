using System;
using Quartz;
using System.Threading.Tasks;
using MeetingPlanner.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MeetingPlanner.Others.Scheduling
{
    [DisallowConcurrentExecution]
    public class NotificationsJob : IJob
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<NotificationsJob> _logger;

        public NotificationsJob(IServiceProvider provider, ILogger<NotificationsJob> logger)
        {
            _provider = provider;
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Executing notifications job.");
            using var scope = _provider.CreateScope();
            var service = scope.ServiceProvider.GetService<INotificationService>();
            service.ResolveAndSendNotifications();
            return Task.CompletedTask;
        }
    }
}
