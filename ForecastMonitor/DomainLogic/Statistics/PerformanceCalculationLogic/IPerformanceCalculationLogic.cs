using ForecastMonitor.Service.DataAccessLogic.DataAccessObjects;

namespace ForecastMonitor.Service.DomainLogic.Statistics.PerformanceCalculationLogic
{
    public interface IPerformanceCalculationLogic
    {
        double MAE(DaoModel model);
        double ZScore(DaoUnit unit);
    }
}