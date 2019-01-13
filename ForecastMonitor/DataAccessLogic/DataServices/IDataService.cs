using System.Collections.Generic;
using ForecastMonitor.Service.DataAccessLogic.DataAccessObjects;

namespace ForecastMonitor.Service.DataAccessLogic.DataServices
{
    public interface IDataService
    {
        IEnumerable<DaoInstallation> GetAllInstallations();
        IEnumerable<DaoClient> GetAllClients();
        IEnumerable<DaoUnit> GetAllUnits();
        IEnumerable<DaoPrediction> GetAllPredictions();
        IEnumerable<DaoTimeSerie> GetAllTimeSeries();
        IEnumerable<DaoModel> GetAllModels();
        IEnumerable<DaoClient> GetClients(int installationId);
        IEnumerable<DaoUnit> GetUnits(int installationId, int clientId);
        void UpsertInstallations(List<DaoInstallation> installations);
        void UpsertClients(List<DaoClient> clients);
        void UpsertUnits(List<DaoUnit> units);
        void UpsertPredictions(List<DaoPrediction> predictions);
        void UpsertTimeSeries(List<DaoTimeSerie> timeSeries);
        void UpsertModels(List<DaoModel> models);
    }
}
