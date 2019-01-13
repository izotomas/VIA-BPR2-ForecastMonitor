using System.Collections.Generic;
using ForecastMonitor.Service.DataAccessLogic.DataAccessObjects;

namespace ForecastMonitor.Service.DataAccessLogic.DataServices.TimeSerieDataService
{
    public interface ITimeSerieDataService
    {
        IEnumerable<DaoTimeSerie> GetTimeSeries();
        IEnumerable<DaoTimeSerie> GetTimeSeries(DaoUnit unit);
        IEnumerable<DaoTimeSerie> GetTimeSeries(IEnumerable<DaoPrediction> predictions);
    }
}