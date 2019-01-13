using System.Collections.Generic;
using ForecastMonitor.Service.DataAccessLogic.DataAccessObjects;

namespace ForecastMonitor.Service.DataAccessLogic.DataServices.PredictionDataService
{
    public interface IPredictionDataService
    {
        IEnumerable<DaoPrediction> GetPredictions();
        IEnumerable<DaoPrediction> GetPredictions(DaoUnit unit);
        IEnumerable<DaoPrediction> GetPredictions(DaoModel model);
    }
}