using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ForecastMonitor.Service.AppSettings;
using ForecastMonitor.Service.DomainLogic.HttpClients.DataTransferObjects;

namespace ForecastMonitor.Service.DomainLogic.ForecastSystemService
{
    public interface IForecastSystemService
    {
        Installation GetInstallation();
        Task<IEnumerable<DtoClient>> GetClients();
        Task<IEnumerable<DtoUnitKey>> GetUnits();
        Task<IEnumerable<DtoModelInfo>> GetModelInfos(IEnumerable<DtoPrediction> predictions);
        /// <summary>
        /// Gets mutually mappable Predictions and TimeSeries, within the time same interval
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<(IEnumerable<DtoPrediction>, IEnumerable<DtoTimeSerie>)> GetMappablePredictionsAndTimeSeries(DtoUnitKey unit, DtoModelInfo model);
        /// <summary>
        /// Model with prediction for the most recent TimeSerie
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        Task<DtoModelInfo> GetLatestEvaluableModel(DtoUnitKey unit);

        Task<IEnumerable<DtoTimeSerie>> GetTimeSeries(DtoUnitKey unit, DateTime fromDate);
        Task<IEnumerable<DtoPrediction>> GetPredictions(DtoUnitKey unit, DateTime fromDate);
        Task<DtoModelInfo> TrainModel(DtoUnitKey unit);
    }
}