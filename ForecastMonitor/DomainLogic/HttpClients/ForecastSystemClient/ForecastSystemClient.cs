using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ForecastMonitor.Service.AppSettings;
using ForecastMonitor.Service.DomainLogic.HttpClients.DataTransferObjects;
using ForecastMonitor.Service.DomainLogic.HttpClients.Serialization;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ForecastMonitor.Service.DomainLogic.HttpClients.ForecastSystemClient
{
    public class ForecastSystemClient : IForecastSystemClient
    {
        private const string DateTimeQueryFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'";
        private static class Endpoints
        {
            public const string Clients = "client/all";
            public const string ModelInfo = "model_info/";
            public const string ModelInfos = "model_infos";
            public const string ModelTrain = "model_training/train";
            public const string Predictions = "predictions";
            public const string TimeSeries = "timeseries";
            public const string UnitKeys = "unit_keys/all";
        }

        private readonly ILogger _logger;
        private readonly HttpClient _client;

        public ForecastSystemClient(HttpClient client, ILoggerFactory loggerFactory)
        {
            this._logger = loggerFactory.CreateLogger<HttpClient>();
            this._client = client;
        }

        public async Task<IEnumerable<DtoClient>> GetClients(Installation installation)
        {
            var result = await GetStringAsync(installation, Endpoints.Clients);
            try
            {
                var clients = JsonConvert.DeserializeObject<IEnumerable<DtoClient>>(result, new DtoConverter<DtoClient>(installation.Id));
                return clients;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, $"Failed to deserialize clients from string: {result}");
                throw;
            }
        }

        public async Task<IEnumerable<DtoUnitKey>> GetUnits(Installation installation)
        {

            var result = await GetStringAsync(installation, Endpoints.UnitKeys);
            try
            {
                var unitKeys = JsonConvert.DeserializeObject<IEnumerable<DtoUnitKey>>(result, new DtoUnitKeyConverter(installation.Id));
                return unitKeys;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, $"Failed to deserialize unitKeys from string: {result}");
                throw;
            }
        }

        public async Task<IEnumerable<DtoPrediction>> GetPredictions(Installation installation, DateTime? fromDate = null, DateTime? toDate = null, string unitKey = null, int? modelId = null)
        {
            var queryParams = new Dictionary<string, string>();
            if (unitKey != null) { queryParams["unit_key"] = unitKey; }
            if (modelId.HasValue) { queryParams["model_id"] = modelId.Value.ToString(); }
            if (fromDate.HasValue) { queryParams["from_dt"] = fromDate.Value.ToUniversalTime().ToString(DateTimeQueryFormat); }
            if (toDate.HasValue) { queryParams["to_dt"] = toDate.Value.ToUniversalTime().ToString(DateTimeQueryFormat); }

            var result = await GetStringAsync(installation, Endpoints.Predictions, queryParams);

            try
            {
                var predictions = JsonConvert.DeserializeObject<IEnumerable<DtoPrediction>>(result, new DtoConverter<DtoPrediction>(installation.Id));
                return predictions;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, $"Failed to deserialize unitKeys from string: {result}");
                throw;
            }
        }

        public async Task<IEnumerable<DtoTimeSerie>> GetTimeSeries(Installation installation, string unitKey, string clientKey, DateTime? fromDate = null, DateTime? toDate = null, int? mostRecent = null)
        {
            var queryParams = new Dictionary<string, string>
            {
                {"unit_key", unitKey},
                {"client_key", clientKey},
                {"exclude_null_labels", "true"}
            };

            if (mostRecent.HasValue) { queryParams["most_recent"] = mostRecent.Value.ToString(); }
            if (fromDate.HasValue) { queryParams["from_date"] = fromDate.Value.ToUniversalTime().ToString(DateTimeQueryFormat); }
            if (toDate.HasValue) { queryParams["to_date"] = toDate.Value.ToUniversalTime().ToString(DateTimeQueryFormat); }

            var result = await GetStringAsync(installation, Endpoints.TimeSeries, queryParams);

            try
            {
                var timeSeries = JsonConvert.DeserializeObject<IEnumerable<DtoTimeSerie>>(result, new DtoTimeSerieConverter(installation.Id));
                return timeSeries;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, $"Failed to deserialize timeSeries from string: {result}");
                throw;
            }
        }

        public async Task<DtoModelInfo> GetModelInfo(Installation installation, int modelId)
        {
            var queryParams = new Dictionary<string, string>{ {"model_id", modelId.ToString()} };
            var result = await GetStringAsync(installation, Endpoints.ModelInfo, queryParams);

            try
            {
                var modelInfo = JsonConvert.DeserializeObject<DtoModelInfo>(result, new DtoConverter<DtoModelInfo>(installation.Id));
                return modelInfo;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, $"Failed to deserialize modelInfo from string: {result}");
                throw;
            }
        }

        public async Task<IEnumerable<DtoModelInfo>> GetModelInfos(Installation installation, IEnumerable<int> modelIds)
        {
            var modelInfoParam = string.Join(",", modelIds);
            var queryParams = new Dictionary<string, string> {{"model_ids", modelInfoParam}};
            var result = await GetStringAsync(installation, Endpoints.ModelInfos, queryParams);

            try
            {
                var modelInfos =JsonConvert.DeserializeObject<IEnumerable<DtoModelInfo>>(result, new DtoConverter<DtoModelInfo>(installation.Id));
                return modelInfos;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, $"Failed to deserialize modelInfos from string: {result}");
                throw;
            }
        }

        public async Task<DtoModelInfo> TrainModel(Installation installation, string clientKey, string unitKey)
        {
            var queryParams = new Dictionary<string, string>
            {
                {"client_key", clientKey},
                { "unit_key", unitKey }
            };
            var result = await GetStringAsync(installation, Endpoints.ModelTrain, queryParams);

            try
            {
                var modelInfo = JsonConvert.DeserializeObject<DtoModelInfo>(result, new DtoConverter<DtoModelInfo>(installation.Id));
                return modelInfo;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, $"Failed to deserialize modelInfo from string: {result}");
                throw;
            }
        }


        private async Task<string> GetStringAsync(Installation installation, string endpoint, IDictionary<string, string> queryParams = null)
        {
            var url = GetFullUrl(installation.Url, endpoint, queryParams);

            try
            {
                return await this._client.GetStringAsync(url);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, $"Installation: {installation.Name} | Failed to resolve url: {url}");
                throw;
            }
        }

        private static string GetFullUrl(string baseUrl, string endpoint, IDictionary<string, string> queryParams = null)
        {
            var url = $"{baseUrl}{endpoint}";

            if (queryParams != null)
            {
                url = QueryHelpers.AddQueryString(url, queryParams);
            }

            return url;
        }
    }
}