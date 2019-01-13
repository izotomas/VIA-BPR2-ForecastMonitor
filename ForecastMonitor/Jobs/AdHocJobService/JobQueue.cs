using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using ForecastMonitor.Service.Jobs.JobTypes;
using ForecastMonitor.Service.Jobs.JobTypes.AdHoc;
using MediatR;

namespace ForecastMonitor.Service.Jobs.AdHocJobService
{
    public class JobQueue : IJobQueue
    {
        private readonly SemaphoreSlim _signal = new SemaphoreSlim(0);
        private readonly ConcurrentQueue<IAdHocJob> _jobs = new ConcurrentQueue<IAdHocJob>();

        public void Enqueue(IAdHocJob job)
        {
            if (job == null)
            {
                throw new ArgumentNullException(nameof(job));
            }

            _jobs.Enqueue(job);
            _signal.Release();
        }

        public async Task<IAdHocJob> DequeAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _jobs.TryDequeue(out var job);
            return job;
        }
    }
}
