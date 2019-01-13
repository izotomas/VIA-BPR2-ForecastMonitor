using System;
using System.Threading;
using System.Threading.Tasks;
using ForecastMonitor.Service.Jobs.JobTypes.AdHoc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ForecastMonitor.Service.Jobs.AdHocJobService
{
    public class AdHocJobService : BackgroundService, IAdHocJobService
    {
        private readonly ILogger _logger;
        private readonly IJobQueue _jobQueue;

        public AdHocJobService(IJobQueue jobQueue, ILoggerFactory loggerFactory)
        {
            _jobQueue = jobQueue;
            _logger = loggerFactory.CreateLogger<AdHocJobService>();
        }
        public Task AddJob(IAdHocJob job)
        {
            _jobQueue.Enqueue(job);
            return Task.CompletedTask;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            this._logger.LogInformation($"{nameof(AdHocJobService)} is starting.");
            while (!cancellationToken.IsCancellationRequested)
            {
                var job = await this._jobQueue.DequeAsync(cancellationToken);
                try
                {
                    this._logger.LogInformation($"{nameof(AdHocJobService)} is executing {job.GetType()}.");
                    await job.ExecuteAsync(cancellationToken);
                }
                catch (Exception e)
                {
                    this._logger.LogError(e, $"Error occured during execution of job: {job.GetType()}.");
                }
            }
            this._logger.LogInformation($"{nameof(AdHocJobService)} is stopping.");
        }
    }
}
