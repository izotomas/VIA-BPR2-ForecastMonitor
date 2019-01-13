using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ForecastMonitor.Service.Jobs.AdHocJobService;
using ForecastMonitor.Service.Jobs.JobTypes;
using ForecastMonitor.Service.Jobs.JobTypes.AdHoc;
using ForecastMonitor.Service.Jobs.Notifications;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ForecastMonitor.Service.Jobs.JobNotificationHandler
{
    public class NotificationHandler : INotificationHandler<JobCompletedNotification>
    {
        private readonly IAdHocJobService _adHocJobService;
        private readonly ILogger _logger;
        private readonly List<IAdHocJob> _jobs;

        public NotificationHandler(IAdHocJobService adHocJobService, IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            _adHocJobService = adHocJobService;
            _logger = loggerFactory.CreateLogger<IJob>();
            _jobs = serviceProvider.GetServices<IAdHocJob>().ToList();
        }

        public async Task Handle(JobCompletedNotification notification, CancellationToken cancellationToken)
        {
            var type = notification.JobType;
            _logger.LogDebug($"Job finished: {type.Name}");

            if (type == typeof(CacheUpdateJob))
            {
                var performanceCalculationJob = this._jobs.FirstOrDefault(x => x.GetType() == typeof(PerformanceCalculationJob));
                await this._adHocJobService.AddJob(performanceCalculationJob);
            }
            else if (type == typeof(PerformanceCalculationJob))
            {
                var unitPublishingJob = this._jobs.FirstOrDefault(x => x.GetType() == typeof(UnitsPublishJob));
                await this._adHocJobService.AddJob(unitPublishingJob);
            }
        }
    }
}
