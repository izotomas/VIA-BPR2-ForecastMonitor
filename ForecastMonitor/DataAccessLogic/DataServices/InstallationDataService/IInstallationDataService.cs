using System.Collections.Generic;
using ForecastMonitor.Service.DataAccessLogic.DataAccessObjects;

namespace ForecastMonitor.Service.DataAccessLogic.DataServices.InstallationDataService
{
    public interface IInstallationDataService
    {
        IEnumerable<DaoInstallation> GetAllInstallations();
        DaoInstallation GetInstallation(int installationId);
    }
}