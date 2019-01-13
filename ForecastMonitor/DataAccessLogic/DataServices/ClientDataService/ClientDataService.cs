using System.Collections.Generic;
using System.Linq;
using ForecastMonitor.Service.DataAccessLogic.DataAccessObjects;
using ForecastMonitor.Service.DataAccessLogic.DataContext;

namespace ForecastMonitor.Service.DataAccessLogic.DataServices.ClientDataService
{
    public class ClientDataService : IClientDataService
    {
        private readonly IDataContext _context;

        public ClientDataService(IDataContext context)
        {
            _context = context;
        }

        public IEnumerable<DaoClient> GetClients(int installationId)
        {
            var clients = this._context.Clients
                .Where(_ => _.InstallationId == installationId);
            return clients;
        }

        public IEnumerable<DaoClient> GetAllClients()
        {
            var clients = this._context.Clients;
            return clients;
        }
    }
}