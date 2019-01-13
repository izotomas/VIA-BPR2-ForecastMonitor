using System.Collections.Generic;
using System.Linq;
using ForecastMonitor.Service.DataAccessLogic.DataAccessObjects;
using ForecastMonitor.Service.DataAccessLogic.DataContext;

namespace ForecastMonitor.Service.DataAccessLogic.DataServices.InstallationDataService
{
    public class InstallationDataService : IInstallationDataService
    {
        private readonly IDataContext _context;

        public InstallationDataService(IDataContext context)
        {
            _context = context;
        }

        public IEnumerable<DaoInstallation> GetAllInstallations()
        {
            var installations = this._context.Installations;
            return installations;
        }

        public DaoInstallation GetInstallation(int installationId)
        {
            var installation = this._context.Installations
                .FirstOrDefault(_ => _.Id == installationId);
            return installation;
        }
    }
}