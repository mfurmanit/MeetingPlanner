using System;
using Quartz;
using System.Threading.Tasks;
using MeetingPlanner.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MeetingPlanner.Others.Scheduling
{
    [DisallowConcurrentExecution]
    public class NotificationsJob : IJob
    {
        private readonly IServiceProvider _provider;

        public NotificationsJob(IServiceProvider provider)
        {
            _provider = provider;
        }

        public Task Execute(IJobExecutionContext context)
        {
            using (var scope = _provider.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<INotificationService>();
                service.ResolveAndSendNotifications();
            }
            return Task.CompletedTask;
        }
    }
}
