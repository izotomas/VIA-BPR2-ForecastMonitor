using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ForecastMonitor.Service.Configuration.AppSettings.Manager;
using ForecastMonitor.Service.DataAccessLogic.DataAccessObjects;
using ForecastMonitor.Service.DataAccessLogic.DataServices;
using ForecastMonitor.Service.DomainLogic.ForecastSystemService;
using ForecastMonitor.Service.DomainLogic.HttpClients.DataTransferObjects;
using ForecastMonitor.Service.Jobs.JobTypes;
using ForecastMonitor.Service.Jobs.JobTypes.Scheduled;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ForecastMonitor.Service.Jobs
{
    public class CacheUpdateJob : ScheduledJob
    {
        private readonly ILogger _logger;
        public sealed override string Schedule { get; protected set; }


        public CacheUpdateJob(IAppSettingsManager appSettings, IServiceScopeFactory scopeFactory, ILoggerFactory loggerFactory) : base(scopeFactory)
        {
            this._logger = loggerFactory.CreateLogger<IJob>();
            this.Schedule = appSettings.JobScheduling.CacheUpdate;
        }


        protected override async Task DoExecuteAsync(CancellationToken cancellationToken)
        {
            this._logger.LogDebug("Executing cache update job");

            using (var scope = this.Factory.CreateScope())
            {
                var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
                var dataService = scope.ServiceProvider.GetRequiredService<IDataService>();
                var forecastSystemServices = scope.ServiceProvider.GetServices<IForecastSystemService>().ToList();

                var allDaoInstallations = new List<DaoInstallation>();
                var allDaoClients = new List<DaoClient>();
                var allDaoUnits = new List<DaoUnit>();
                var allDaoModels = new List<DaoModel>();
                var allDaoPredictions = new List<DaoPrediction>();
                var allDaoTimeSeries = new List<DaoTimeSerie>();

                foreach (var forecastService in forecastSystemServices)
                {
                    try
                    {
                        var dtoClients = await forecastService.GetClients();
                        var dtoUnits = (await forecastService.GetUnits()).ToList();

                        var daoInstallation = mapper.Map<DaoInstallation>(forecastService.GetInstallation());
                        var daoClients = dtoClients.Select(mapper.Map<DaoClient>);
                        var daoUnits = dtoUnits.Select(mapper.Map<DaoUnit>);
                        
                        foreach (var unit in dtoUnits)
                        {
                            var model = await forecastService.GetLatestEvaluableModel(unit);
                            if (model != null)
                            {
                                var dtoPredictionsAndTimeSeries = await forecastService.GetMappablePredictionsAndTimeSeries(unit, model);
                                
                                var daoModel = mapper.Map<DtoModelInfo, DaoModel>(model, opts => opts.AfterMap((dto, dao) => dao.IsLatestEvaluable = true));
                                var daoUnitPredictions = dtoPredictionsAndTimeSeries.Item1.Select(mapper.Map<DaoPrediction>);
                                var daoUnitTimeSeries = dtoPredictionsAndTimeSeries.Item2.Select(mapper.Map<DaoTimeSerie>);

                                allDaoModels.Add(daoModel);
                                allDaoPredictions.AddRange(daoUnitPredictions);
                                allDaoTimeSeries.AddRange(daoUnitTimeSeries);
                            }
                        }

                        allDaoInstallations.Add(daoInstallation);
                        allDaoClients.AddRange(daoClients);
                        allDaoUnits.AddRange(daoUnits);
                    }
                    catch (Exception ex)
                    {
                        this._logger.LogError(ex, $"Failed to receive data from installation: { forecastService.GetInstallation()}");
                    }
                }

                dataService.UpsertInstallations(allDaoInstallations);
                dataService.UpsertClients(allDaoClients);
                dataService.UpsertUnits(allDaoUnits);
                dataService.UpsertModels(allDaoModels);
                dataService.UpsertPredictions(allDaoPredictions);
                dataService.UpsertTimeSeries(allDaoTimeSeries);
            }
        }
    }
}
