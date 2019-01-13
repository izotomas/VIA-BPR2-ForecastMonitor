using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForecastMonitor.Service.AppSettings;
using ForecastMonitor.Service.Configuration.AppSettings.Manager;
using ForecastMonitor.Service.DomainLogic.HttpClients.DataTransferObjects;
using ForecastMonitor.Service.DomainLogic.HttpClients.ForecastSystemClient;
using Microsoft.Extensions.Logging;

namespace ForecastMonitor.Service.DomainLogic.ForecastSystemService
{
    public class ForecastSystemService : IForecastSystemService
    {
        private readonly Installation _installation;
        private readonly IForecastSystemClient _client;
        private readonly ILogger _logger;
        private readonly IAppSettingsManager _appSettings;

        public ForecastSystemService(Installation installation, IForecastSystemClient client, IAppSettingsManager appSettings, ILoggerFactory loggerFactory)
        {
            _installation = installation;
            _client = client;
            _logger = loggerFactory.CreateLogger<ForecastSystemService>();
            _appSettings = appSettings;
        }

        public Installation GetInstallation()
        {
            return this._installation;
        }

        public async Task<IEnumerable<DtoClient>> GetClients()
        {
            return await this._client.GetClients(this._installation);
        }

        public async Task<IEnumerable<DtoUnitKey>> GetUnits()
        {
            return await this._client.GetUnits(this._installation);
        }

        public async Task<(IEnumerable<DtoPrediction>, IEnumerable<DtoTimeSerie>)> GetMappablePredictionsAndTimeSeries(DtoUnitKey unit, DtoModelInfo model)
        {
            
            var modelPredictions = (await GetPredictions(model)).ToList();
            var lookupInterval = GetLookupInterval(modelPredictions);

            var dtoUnitPredictions = (await GetPredictions(unit, lookupInterval)).ToList();
            var dtoUnitTimeSeries = (await GetTimeSeries(unit, lookupInterval)).ToList();

            FilterPredictionsAndTimeSeriesWithMatchingDates(ref dtoUnitPredictions, ref dtoUnitTimeSeries);

            return new ValueTuple<IEnumerable<DtoPrediction>, IEnumerable<DtoTimeSerie>>
            {
                Item1 = dtoUnitPredictions,
                Item2 = dtoUnitTimeSeries
            };
        }

        public async Task<DtoModelInfo> GetLatestEvaluableModel(DtoUnitKey unit)
        {
            var timeSerie = await GetMostRecentTimeSerie(unit);
            if (timeSerie != null)
            {
                var predictions = (await this._client.GetPredictions(this._installation, timeSerie.Date, timeSerie.Date, unit.UnitKey)).ToList();
                var mostRecentPrediction = predictions.ToList().TakeLast(1).FirstOrDefault();
                if (mostRecentPrediction != null)
                {
                    var modelInfo = await this._client.GetModelInfo(this._installation, mostRecentPrediction.ModelId);
                    return modelInfo;
                }
                _logger.LogDebug($"No latest evaluable model exists: {unit.UnitKey} | has no predictions for most recent timeSerie ({timeSerie.Date.ToUniversalTime()})");
            }
            else
            {
                _logger.LogDebug($"No latest evaluable model exists: {unit.UnitKey} | has no timeSeries with non-null labels");
            }
            return default;
        }

        public async Task<IEnumerable<DtoTimeSerie>> GetTimeSeries(DtoUnitKey unit, DateTime fromDate)
        {
            var timeSeries = await this._client.GetTimeSeries(this._installation, unit.UnitKey, unit.Client.Key);
            timeSeries = timeSeries.Where(_ => _.Date >= fromDate);
            return timeSeries;
        }

        public async Task<IEnumerable<DtoPrediction>> GetPredictions(DtoUnitKey unit, DateTime fromDate)
        {
            var predictions = await this._client.GetPredictions(this._installation, fromDate, DateTime.Now, unit.UnitKey);
            return predictions;
        }

        public async Task<DtoModelInfo> TrainModel(DtoUnitKey unit)
        {
            var modelInfo = await this._client.TrainModel(this._installation, unit.Client.Key, unit.UnitKey);
            return modelInfo;
        }

        public async Task<IEnumerable<DtoModelInfo>> GetModelInfos(IEnumerable<DtoPrediction> predictions)
        {
            var modelIds = predictions.Select(prediction => prediction.ModelId);
            var modelInfos = await this._client.GetModelInfos(this._installation, modelIds);
            return modelInfos;
        }

        private async Task<DtoTimeSerie> GetMostRecentTimeSerie(DtoUnitKey unit)
        {
            var timeSeries = (await this._client.GetTimeSeries(this._installation, unit.UnitKey, unit.Client.Key, mostRecent: 1)).ToList(); 
            var mostRecent = timeSeries.FirstOrDefault(ts => !string.IsNullOrEmpty(ts.Label));
            return mostRecent;
        }

        private async Task<IEnumerable<DtoTimeSerie>> GetTimeSeries(DtoUnitKey unit, (DateTime, DateTime) lookupInterval)
        {
            var timeSeries = await this._client.GetTimeSeries(this._installation, unit.UnitKey, unit.Client.Key, lookupInterval.Item1, lookupInterval.Item2);
            return timeSeries;
        }

        private async Task<IEnumerable<DtoPrediction>> GetPredictions(DtoModelInfo model)
        {
            var predictions = await this._client.GetPredictions(this._installation, modelId: model.ModelId);
            return predictions;
        }

        private async Task<IEnumerable<DtoPrediction>> GetPredictions(DtoUnitKey unit, (DateTime, DateTime) lookupInterval)
        {
            var predictions = await this._client.GetPredictions(this._installation, lookupInterval.Item1, lookupInterval.Item2, unit.UnitKey);
            return predictions;
        }

        private void FilterPredictionsAndTimeSeriesWithMatchingDates(ref List<DtoPrediction> predictions, ref List<DtoTimeSerie> timeSeries)
        {
            timeSeries.RemoveAll(ts => string.IsNullOrEmpty(ts.Label));

            var timeSeriesDates = timeSeries.Select(ts => ts.Date);
            predictions.RemoveAll(prediction => !timeSeriesDates.Contains(prediction.PredictionForDate));

            var predictionDates = predictions.Select(prediction => prediction.PredictionForDate);
            timeSeries.RemoveAll(ts => !predictionDates.Contains(ts.Date));
        }

        private (DateTime, DateTime) GetLookupInterval(IList<DtoPrediction> predictions)
        {
            var interval = new ValueTuple<DateTime, DateTime>
            {
                Item1 = predictions.First().PredictionForDate.AddDays(-this._appSettings.HistoricalDataLookupInDays),
                Item2 = predictions.Last().PredictionForDate
            };
            return interval;
        }
    }
}
