using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ForecastMonitor.Service.ApplicationLogic.DataTransferObjects;
using ForecastMonitor.Service.Configuration.AppSettings.Manager;
using ForecastMonitor.Service.DataAccessLogic.DataServices.UnitDataService;
using ForecastMonitor.Service.DomainLogic.DomainModel;
using ForecastMonitor.Service.DomainLogic.ForecastSystemService;
using ForecastMonitor.Service.DomainLogic.HttpClients.DataTransferObjects;

namespace ForecastMonitor.Service.ApplicationLogic.DataLogic.UnitLogic
{
    public class UnitLogic : IUnitLogic
    {
        private readonly IUnitDataService _dataService;
        private readonly IEnumerable<IForecastSystemService> _forecastServices;
        private readonly IAppSettingsManager _appSettings;
        private readonly IMapper _mapper;

        public UnitLogic(IUnitDataService dataService, IEnumerable<IForecastSystemService> forecastServices, IAppSettingsManager appSettings, IMapper mapper)
        {
            this._dataService = dataService;
            this._forecastServices = forecastServices;
            this._appSettings = appSettings;
            this._mapper = mapper;
        }

        public IEnumerable<DtoUnit> GetUnits(int installationId, int clientId)
        {
            var dao = this._dataService.GetUnits(installationId, clientId);
            var dto = dao.Select(this._mapper.Map<DtoUnit>);
            return dto;
        }

        public async Task<DtoUnitPlot> GetUnitPlotAsync(int installationId, int unitId, int? weeksAgo = null)
        {
            var unit = this._dataService.GetUnit(installationId, unitId);
            var fromDate = DateTime.Today.AddDays(-7 * (weeksAgo ?? _appSettings.UnitPlotDefaultRangeInWeeks));

            if (unit != null)
            {
                var unitKey = this._mapper.Map<DtoUnitKey>(unit);
                var forecastService = this._forecastServices.First(_ => _.GetInstallation().Id == unitKey.InstallationId);
                var predictions = await forecastService.GetPredictions(unitKey, fromDate);
                var timeSeries = await forecastService.GetTimeSeries(unitKey, fromDate);

                var predictionGraphDataPoints = predictions.Select(_mapper.Map<GraphDataPoint<DateTime, double>>);
                var timeSeriesGraphDataPoints = timeSeries.Select(_mapper.Map<GraphDataPoint<DateTime, double>>);

                var plot = new DtoUnitPlot
                {
                    Id = unit.Id,
                    Name = unit.Name,
                    InstallationId = unit.InstallationId,
                    UnitKey = unit.Key,
                    Historical = timeSeriesGraphDataPoints, 
                    Predictions = predictionGraphDataPoints
                };
                return plot;
            }
            return default;
        }

        public async Task TrainUnitAsync(int installationId, int unitId)
        {
            var daoUnit = this._dataService.GetUnit(installationId, unitId);
            if (daoUnit != null)
            {
                var dtoUnitKey = this._mapper.Map<DtoUnitKey>(daoUnit);
                var forecastService = this._forecastServices.First(_ => _.GetInstallation().Id == installationId);
                await forecastService.TrainModel(dtoUnitKey);
            }
        }
    }
}