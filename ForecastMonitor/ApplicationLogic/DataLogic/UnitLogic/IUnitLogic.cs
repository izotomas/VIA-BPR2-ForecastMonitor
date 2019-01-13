using System.Collections.Generic;
using System.Threading.Tasks;
using ForecastMonitor.Service.ApplicationLogic.DataTransferObjects;

namespace ForecastMonitor.Service.ApplicationLogic.DataLogic.UnitLogic
{
    public interface IUnitLogic
    {
        IEnumerable<DtoUnit> GetUnits(int installationId, int clientId);
        Task<DtoUnitPlot> GetUnitPlotAsync(int installationId, int unitId, int? weeksAgo = null);
        Task TrainUnitAsync(int installationId, int unitId);
    }
}
