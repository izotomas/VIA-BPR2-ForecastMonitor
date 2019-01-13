using System;
using System.Threading.Tasks;
using ForecastMonitor.Service.Jobs.AdHocJobService;
using ForecastMonitor.Service.Jobs.JobTypes.Scheduled;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ForecastMonitor.Service.Jobs
{
    public static class JobServicesExtensions
    {
        public static IServiceCollection AddSchedulerJobService(this IServiceCollection services, EventHandler<UnobservedTaskExceptionEventArgs> unobservedTaskExceptionHandler)
        {
            return services.AddSingleton<IHostedService, ScheduledJobService.ScheduledJobService>(serviceProvider =>
            {
                var jobs = serviceProvider.GetServices<IScheduledJob>();
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                var instance = new ScheduledJobService.ScheduledJobService(jobs, loggerFactory);
                instance.UnobservedTaskException += unobservedTaskExceptionHandler;
                return instance;
            });
        }

        public static IServiceCollection AddAdHocJobService(this IServiceCollection services)
        {
            services.AddSingleton<IJobQueue, JobQueue>();
            services.AddHostedService<AdHocJobService.AdHocJobService>();
            services.AddSingleton<IAdHocJobService, AdHocJobService.AdHocJobService>();
            return services;
        }
    }
}
