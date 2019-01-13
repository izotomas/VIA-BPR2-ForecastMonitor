using System.Collections.Generic;
using ForecastMonitor.Service.DataAccessLogic.DataAccessObjects;

namespace ForecastMonitor.Service.DataAccessLogic.DataServices.ClientDataService
{
    public interface IClientDataService
    {
        IEnumerable<DaoClient> GetClients(int installationId);
        IEnumerable<DaoClient> GetAllClients();
    }
}