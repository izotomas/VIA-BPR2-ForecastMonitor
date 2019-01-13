using System.Collections.Generic;
using ForecastMonitor.Service.ApplicationLogic.DataTransferObjects;

namespace ForecastMonitor.Service.ApplicationLogic.DataLogic.ClientLogic
{
    public interface IClientLogic
    {
        IEnumerable<DtoClient> GetClients(int installationId);
    }
}