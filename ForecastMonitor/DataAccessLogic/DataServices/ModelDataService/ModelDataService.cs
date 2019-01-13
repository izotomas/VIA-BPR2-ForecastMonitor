using System.Linq;
using ForecastMonitor.Service.DataAccessLogic.DataAccessObjects;
using ForecastMonitor.Service.DataAccessLogic.DataContext;

namespace ForecastMonitor.Service.DataAccessLogic.DataServices.ModelDataService
{
    public class ModelDataService : IModelDataService
    {
        private readonly IDataContext _context;

        public ModelDataService(IDataContext context)
        {
            _context = context;
        }

        public DaoModel GetLatestEvaluableModel(DaoUnit unit)
        {
            var model = this._context.Models.FirstOrDefault(_ =>
                _.InstallationId == unit.InstallationId &&
                _.UnitId == unit.Id &&
                _.IsLatestEvaluable);
            return model;
        }
    }
}