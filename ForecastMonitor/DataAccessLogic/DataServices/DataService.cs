using System.Collections.Generic;
using System.Linq;
using ForecastMonitor.Service.DataAccessLogic.DataAccessObjects;
using ForecastMonitor.Service.DataAccessLogic.DataContext;

namespace ForecastMonitor.Service.DataAccessLogic.DataServices
{
    public class DataService: IDataService
    {
        private readonly IDataContext _context;

        public DataService(IDataContext context)
        {
            _context = context;
        }

        public IEnumerable<DaoInstallation> GetAllInstallations()
        {
            return this._context.Installations;
        }

        public IEnumerable<DaoClient> GetAllClients()
        {
            return this._context.Clients;
        }

        public IEnumerable<DaoUnit> GetAllUnits()
        {
            return this._context.Units;
        }

        public IEnumerable<DaoPrediction> GetAllPredictions()
        {
            return this._context.Predictions;
        }

        public IEnumerable<DaoTimeSerie> GetAllTimeSeries()
        {
            return this._context.TimeSeries;
        }

        public IEnumerable<DaoModel> GetAllModels()
        {
            return this._context.Models;
        }

        public IEnumerable<DaoClient> GetClients(int installationId)
        {
            var clients = this._context.Clients
                .Where(client => client.InstallationId == installationId);
            return clients;
        }

        public IEnumerable<DaoUnit> GetUnits(int installationId, int clientId)
        {
            var units = this._context.Units
                .Where(unit => unit.InstallationId == installationId && unit.ClientId == clientId);
            return units;
        }

        public void UpsertInstallations(List<DaoInstallation> installations)
        {
            this._context.Installations = installations;
        }

        public void UpsertClients(List<DaoClient> clients)
        {
            this._context.Clients = clients;
        }

        public void UpsertUnits(List<DaoUnit> units)
        {
            this._context.Units = units;
        }

        public void UpsertPredictions(List<DaoPrediction> predictions)
        {
            this._context.Predictions = predictions;
        }

        public void UpsertTimeSeries(List<DaoTimeSerie> timeSeries)
        {
            this._context.TimeSeries = timeSeries;
        }

        public void UpsertModels(List<DaoModel> models)
        {
            this._context.Models = models;
        }
    }
}
