using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ForecastMonitor.Service.AppSettings;
using ForecastMonitor.Service.DomainLogic.HttpClients.DataTransferObjects;

namespace ForecastMonitor.Service.DomainLogic.HttpClients.ForecastSystemClient
{
    public interface IForecastSystemClient
    {
        Task<IEnumerable<DtoClient>> GetClients(Installation installation);
        Task<IEnumerable<DtoUnitKey>> GetUnits(Installation installation);
        Task<IEnumerable<DtoPrediction>> GetPredictions(Installation installation, DateTime? fromDate = null, DateTime? toDate = null, string unitKey = null, int? modelId = null);
        Task<IEnumerable<DtoTimeSerie>> GetTimeSeries(Installation installation, string unitKey, string clientKey, DateTime? fromDate = null, DateTime? toDate = null, int? mostRecent = null);
        Task<DtoModelInfo> GetModelInfo(Installation installation, int modelId);
        Task<IEnumerable<DtoModelInfo>> GetModelInfos(Installation installation, IEnumerable<int> modelIds);
        Task<DtoModelInfo> TrainModel(Installation installation, string clientKey, string unitKey);
    }
}
