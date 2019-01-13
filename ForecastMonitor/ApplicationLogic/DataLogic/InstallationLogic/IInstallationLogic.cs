using System.Collections.Generic;
using ForecastMonitor.Service.ApplicationLogic.DataTransferObjects;

namespace ForecastMonitor.Service.ApplicationLogic.DataLogic.InstallationLogic
{
    public interface IInstallationLogic
    {
        IEnumerable<DtoInstallation> GetInstallations();
    }
}