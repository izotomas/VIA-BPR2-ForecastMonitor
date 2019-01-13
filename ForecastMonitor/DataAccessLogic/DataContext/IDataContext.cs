using System.Collections.Generic;
using ForecastMonitor.Service.DataAccessLogic.DataAccessObjects;

namespace ForecastMonitor.Service.DataAccessLogic.DataContext
{
    public interface IDataContext
    {
        List<DaoInstallation> Installations { get; set; }
        List<DaoClient> Clients { get; set; }
        List<DaoUnit> Units { get; set; }
        List<DaoPrediction> Predictions { get; set; }
        List<DaoModel> Models { get; set; }
        List<DaoTimeSerie> TimeSeries { get; set; }
    }
}
