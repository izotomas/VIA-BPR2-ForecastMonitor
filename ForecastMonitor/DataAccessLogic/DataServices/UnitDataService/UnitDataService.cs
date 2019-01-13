using System.Collections.Generic;
using System.Linq;
using ForecastMonitor.Service.DataAccessLogic.DataAccessObjects;
using ForecastMonitor.Service.DataAccessLogic.DataContext;

namespace ForecastMonitor.Service.DataAccessLogic.DataServices.UnitDataService
{
    public class UnitDataService : IUnitDataService
    {
        private readonly IDataContext _context;

        public UnitDataService(IDataContext context)
        {
            _context = context;
        }

        public IEnumerable<DaoUnit> GetAllUnits()
        {
            var units = this._context.Units;
            return units;
        }

        public IEnumerable<DaoUnit> GetUnits(int installationId, int clientId)
        {
            var units = this._context.Units
                .Where(_ => _.InstallationId == installationId && _.ClientId == clientId);
            return units;
        }

        public DaoUnit GetUnit(int installationId, int unitId)
        {
            var unit = this._context.Units
                .FirstOrDefault(_ => _.InstallationId == installationId && _.Id == unitId);
            return unit;
        }
    }
}