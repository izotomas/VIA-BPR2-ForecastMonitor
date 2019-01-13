using System.Threading;
using System.Threading.Tasks;
using ForecastMonitor.Service.Jobs.Notifications;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ForecastMonitor.Service.Jobs.JobTypes
{
    public abstract class Job : IJob 
    {
        protected readonly IServiceScopeFactory Factory;
        protected Job(IServiceScopeFactory scopeFactory)
        {
            Factory = scopeFactory;
        }

        protected abstract Task DoExecuteAsync(CancellationToken cancellationToken);

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await DoExecuteAsync(cancellationToken);
            using (var scope = this.Factory.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                await mediator.Publish(new JobCompletedNotification(GetType()), cancellationToken);
            }
        }
    }
}
