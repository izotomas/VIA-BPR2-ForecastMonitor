using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace ForecastMonitor.Service.Jobs.ScheduledJobService
{
    public abstract class BaseHostedService : IHostedService
    {
        private Task _executingJob;
        private CancellationTokenSource _cts;

        protected abstract Task ExecuteAsync(CancellationToken cancellationToken);

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Create a linked token so we can trigger cancellation outside of this token's cancellation
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            // Store the task we're executing
            _executingJob = ExecuteAsync(_cts.Token);

            // If the task is completed then return it, otherwise it's running
            return _executingJob.IsCompleted ? _executingJob : Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_executingJob == null)
            {
                return;
            }

            _cts.Cancel();

            // Wait until the task completes or the stop token triggers
            await Task.WhenAny(_executingJob, Task.Delay(-1, cancellationToken));

            // Throw if cancellation triggered
            cancellationToken.ThrowIfCancellationRequested();
        }
    }
}
