using System.Threading;
using System.Threading.Tasks;
using ForecastMonitor.Service.ApplicationLogic.Publishing.UnitPublishingService;
using ForecastMonitor.Service.Jobs.JobTypes.AdHoc;
using Microsoft.Extensions.DependencyInjection;

namespace ForecastMonitor.Service.Jobs
{
    public class UnitsPublishJob : AdHocJob
    {

        public UnitsPublishJob(IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {
        }

        protected override async Task DoExecuteAsync(CancellationToken cancellationToken)
        {
            using (var scope = Factory.CreateScope())
            {
                var publishingService = scope.ServiceProvider.GetRequiredService<IUnitPublishingService>();
                await publishingService.PublishUnits(cancellationToken);
            }
        }
    }
}
