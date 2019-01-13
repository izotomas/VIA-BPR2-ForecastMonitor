using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ForecastMonitor.Service.Jobs.JobTypes.Scheduled;
using Microsoft.Extensions.Logging;
using NCrontab;

namespace ForecastMonitor.Service.Jobs.ScheduledJobService
{
    public class ScheduledJobService : BaseHostedService
    {
        public event EventHandler<UnobservedTaskExceptionEventArgs> UnobservedTaskException;

        private const int JobLookupTimePeriodInMin = 1;

        private readonly List<JobWrapper> _scheduledJobs = new List<JobWrapper>();
        private readonly ILogger _logger;

        public ScheduledJobService(IEnumerable<IScheduledJob> scheduledJobs, ILoggerFactory loggerFactory)
        {
            this._logger = loggerFactory.CreateLogger<ScheduledJobService>();
            var referenceTime = DateTime.UtcNow;

            foreach (var scheduledJob in scheduledJobs)
            {
                _scheduledJobs.Add(new JobWrapper
                {
                    Schedule = CrontabSchedule.Parse(scheduledJob.Schedule),
                    Job = scheduledJob,
                    NextRunTime = referenceTime
                });
            }

            this._logger.LogDebug($"Scheduler starting with {this._scheduledJobs.Count} scheduled jobs.");
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await ExecuteOnceAsync(cancellationToken);

                await Task.Delay(TimeSpan.FromMinutes(JobLookupTimePeriodInMin), cancellationToken);
            }
        }

        private async Task ExecuteOnceAsync(CancellationToken cancellationToken)
        {
            var taskFactory = new TaskFactory(TaskScheduler.Current);
            var referenceTime = DateTime.UtcNow;

            var jobsThatShouldRun = _scheduledJobs.Where(t => t.ShouldRun(referenceTime)).ToList();

            foreach (var jobThatShouldRun in jobsThatShouldRun)
            {
                jobThatShouldRun.Increment();

                await taskFactory.StartNew(
                    async () =>
                    {
                        try
                        {
                            await jobThatShouldRun.Job.ExecuteAsync(cancellationToken);
                        }
                        catch (Exception ex)
                        {
                            var args = new UnobservedTaskExceptionEventArgs(
                                ex as AggregateException ?? new AggregateException(ex));

                            this._logger.LogError(ex, $"Job execution failed: {jobThatShouldRun.Job.GetType().FullName}");
                            UnobservedTaskException?.Invoke(this, args);

                            if (!args.Observed)
                            {
                                throw;
                            }
                        }
                    },
                    cancellationToken);
            }
        }

        private class JobWrapper
        {
            public CrontabSchedule Schedule { get; set; }
            public IScheduledJob Job { get; set; }
            public DateTime LastRunTime { get; set; }
            public DateTime NextRunTime { get; set; }

            public void Increment()
            {
                LastRunTime = NextRunTime;
                NextRunTime = Schedule.GetNextOccurrence(NextRunTime);
            }

            public bool ShouldRun(DateTime currentTime)
            {
                return NextRunTime < currentTime && LastRunTime != NextRunTime;
            }
        }

    }
}
