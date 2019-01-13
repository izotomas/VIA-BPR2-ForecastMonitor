using System.Threading;
using System.Threading.Tasks;
using ForecastMonitor.Service.DataAccessLogic.DataServices.ModelDataService;
using ForecastMonitor.Service.DataAccessLogic.DataServices.UnitDataService;
using ForecastMonitor.Service.DomainLogic.Statistics.PerformanceCalculationLogic;
using ForecastMonitor.Service.Jobs.JobTypes;
using ForecastMonitor.Service.Jobs.JobTypes.AdHoc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ForecastMonitor.Service.Jobs
{
    public class PerformanceCalculationJob : AdHocJob
    {
        private readonly ILogger _logger;
        public PerformanceCalculationJob(IServiceScopeFactory scopeFactory, ILoggerFactory loggerFactory) : base(scopeFactory)
        {
            this._logger = loggerFactory.CreateLogger<IJob>();
        }

        protected override async Task DoExecuteAsync(CancellationToken cancellationToken)
        {
            await Task.Run(DoPerformanceCalculation, cancellationToken).ConfigureAwait(false);
        }

        private void DoPerformanceCalculation()
        {
            using (var scope = Factory.CreateScope())
            {
                var performanceLogic = scope.ServiceProvider.GetRequiredService<IPerformanceCalculationLogic>();
                var unitDataService = scope.ServiceProvider.GetRequiredService<IUnitDataService>();
                var modelDataService = scope.ServiceProvider.GetRequiredService<IModelDataService>();
                
                var units = unitDataService.GetAllUnits();
                foreach (var unit in units)
                {
                    var model = modelDataService.GetLatestEvaluableModel(unit);
                    if (model != null)
                    {
                        var mae = performanceLogic.MAE(model);
                        model.Mae = mae;
                        unit.Mae = mae;

                        var zScore = performanceLogic.ZScore(unit);
                        unit.ZScore = zScore;

                        this._logger.LogDebug($"{unit.Key} | MAE: {unit.Mae} | Z-score: {unit.ZScore}");
                    }
                    else
                    {
                        this._logger.LogInformation($"{unit.Key}: not evaluable");
                    }
                }
            }
        }
    }
}
