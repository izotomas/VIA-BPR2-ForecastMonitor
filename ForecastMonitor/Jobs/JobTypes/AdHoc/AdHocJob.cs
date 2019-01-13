using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ForecastMonitor.Service.Jobs.JobTypes.AdHoc
{
    public abstract class AdHocJob : Job, IAdHocJob
    {
        protected AdHocJob(IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {
        }
    }
}
