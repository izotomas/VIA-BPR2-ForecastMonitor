using System.Collections.Generic;
using System.Linq;
using ForecastMonitor.Service.DataAccessLogic.DataAccessObjects;
using ForecastMonitor.Service.DataAccessLogic.DataContext;

namespace ForecastMonitor.Service.DataAccessLogic.DataServices.PredictionDataService
{
    public class PredictionDataService : IPredictionDataService
    {
        private readonly IDataContext _context;

        public PredictionDataService(IDataContext context)
        {
            _context = context;
        }
        public IEnumerable<DaoPrediction> GetPredictions()
        {
            var predictions = this._context.Predictions;
            return predictions;
        }

        public IEnumerable<DaoPrediction> GetPredictions(DaoUnit unit)
        {
            var predictions = this._context.Predictions
                .Where(_ =>
                    _.InstallationId == unit.InstallationId &&
                    _.UnitId == unit.Id)
                .OrderBy(_ => _.PredictionFor);
            return predictions;
        }

        public IEnumerable<DaoPrediction> GetPredictions(DaoModel model)
        {
            var predictions = this._context.Predictions
                .Where(_ =>
                    _.InstallationId == model.InstallationId &&
                    _.UnitId == model.UnitId &&
                    _.ModelId == model.Id)
                .OrderBy(_ => _.PredictionFor);
            return predictions;
        }
    }
}