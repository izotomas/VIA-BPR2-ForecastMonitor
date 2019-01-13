using System.Collections.Generic;
using ForecastMonitor.Service.DataAccessLogic.DataAccessObjects;

namespace ForecastMonitor.Service.DataAccessLogic.DataServices.UnitDataService
{
    public interface IUnitDataService
    {
        IEnumerable<DaoUnit> GetAllUnits();
        IEnumerable<DaoUnit> GetUnits(int installationId, int clientId);
        DaoUnit GetUnit(int installationId, int unitId);
    }
}