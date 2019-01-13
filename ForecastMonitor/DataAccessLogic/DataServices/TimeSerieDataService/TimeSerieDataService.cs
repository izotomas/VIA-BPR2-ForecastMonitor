using System.Collections.Generic;
using System.Linq;
using ForecastMonitor.Service.DataAccessLogic.DataAccessObjects;
using ForecastMonitor.Service.DataAccessLogic.DataContext;

namespace ForecastMonitor.Service.DataAccessLogic.DataServices.TimeSerieDataService
{
    public class TimeSerieDataService : ITimeSerieDataService
    {
        private readonly IDataContext _context;

        public TimeSerieDataService(IDataContext context)
        {
            _context = context;
        }

        public IEnumerable<DaoTimeSerie> GetTimeSeries()
        {
            var timeSeries = this._context.TimeSeries;
            return timeSeries;
        }

        public IEnumerable<DaoTimeSerie> GetTimeSeries(DaoUnit unit)
        {
            var timeSeries = this._context.TimeSeries
                .Where(_ =>
                    _.InstallationId == unit.InstallationId &&
                    _.UnitId == unit.Id);
            return timeSeries;
        }

        public IEnumerable<DaoTimeSerie> GetTimeSeries(IEnumerable<DaoPrediction> predictions)
        {
            var timeSeries = predictions.Select(prediction =>
                this._context.TimeSeries.FirstOrDefault(timeSerie =>
                    timeSerie.InstallationId == prediction.InstallationId &&
                    timeSerie.UnitId == prediction.UnitId &&
                    timeSerie.Date == prediction.PredictionFor)
                );
            return timeSeries;
        }
    }
}