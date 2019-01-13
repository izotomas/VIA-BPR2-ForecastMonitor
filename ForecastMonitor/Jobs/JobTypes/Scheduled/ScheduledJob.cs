using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ForecastMonitor.Service.Jobs.JobTypes.Scheduled
{
    public abstract class ScheduledJob : Job, IScheduledJob
    {
        public abstract string Schedule { get; protected set; }

        protected ScheduledJob(IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {
        }
    }
}
