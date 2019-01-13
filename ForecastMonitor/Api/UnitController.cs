using System.Collections.Generic;
using System.Threading.Tasks;
using ForecastMonitor.Service.ApplicationLogic.DataLogic.UnitLogic;
using ForecastMonitor.Service.ApplicationLogic.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ForecastMonitor.Service.Api
{
    [Route("")]
    [ApiController]
    public class UnitController : BaseController
    {

        private readonly IUnitLogic _unit;
        public UnitController(IUnitLogic unit, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            this._unit = unit;
        }

        [HttpGet]
        [Route("units")]
        public IEnumerable<DtoUnit> GetUnits([FromQuery] int installationId, [FromQuery] int clientId)
        {
            Logger.LogDebug($"Calling {nameof(GetUnits)} with {nameof(installationId)}: {installationId} | {nameof(clientId)}: {clientId}");
            return HttpGet(this._unit.GetUnits(installationId, clientId));
        }

        [HttpGet]
        [Route("unit/plot")]
        public async Task<DtoUnitPlot> GetUnitPlot([FromQuery] int installationId, [FromQuery] int unitId, [FromQuery] int? weeksAgo = null)
        {
            Logger.LogDebug($"Calling {nameof(GetUnitPlot)} with {nameof(installationId)}: {installationId} | {nameof(unitId)}: {unitId} | {nameof(weeksAgo)}: {weeksAgo}");
            return HttpGet(await this._unit.GetUnitPlotAsync(installationId, unitId, weeksAgo));
        }

        [HttpGet]
        [Route("unit/train")]
        public async Task TrainUnit([FromQuery] int installationId, [FromQuery] int unitId)
        {
            Logger.LogDebug($"Calling {nameof(TrainUnit)} with {nameof(installationId)}: {installationId}");
            await this._unit.TrainUnitAsync(installationId, unitId);
        }
    }
}
