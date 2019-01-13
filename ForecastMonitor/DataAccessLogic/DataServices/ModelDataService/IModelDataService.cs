using ForecastMonitor.Service.DataAccessLogic.DataAccessObjects;

namespace ForecastMonitor.Service.DataAccessLogic.DataServices.ModelDataService
{
    public interface IModelDataService
    {
        DaoModel GetLatestEvaluableModel(DaoUnit unit);
    }
}
