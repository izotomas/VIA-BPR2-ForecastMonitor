using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ForecastMonitor.Service.ApplicationLogic.DataTransferObjects;
using ForecastMonitor.Service.ApplicationLogic.Publishing.Hubs;
using ForecastMonitor.Service.DataAccessLogic.DataServices.UnitDataService;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace ForecastMonitor.Service.ApplicationLogic.Publishing.UnitPublishingService
{
    public class UnitPublishingService : IUnitPublishingService
    {
        private readonly IUnitDataService _dataService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly HubLifetimeManager<UnitHub> _hubManager;

        public UnitPublishingService(IUnitDataService dataService, IMapper mapper, HubLifetimeManager<UnitHub> hubManager, ILoggerFactory loggerFactory)
        {
            this._dataService = dataService;
            this._mapper = mapper;
            this._logger = loggerFactory.CreateLogger<UnitPublishingService>();
            this._hubManager = hubManager;
        }
        public async Task PublishUnits(CancellationToken cancellationToken)
        {
            var daoUnits = this._dataService.GetAllUnits();
            var dtoUnits = daoUnits.Select(this._mapper.Map<DtoUnit>).ToList();
            this._logger.LogDebug($"Publishing {dtoUnits.Count} units");
            await _hubManager.SendAllAsync("UnitsUpdate", new object[] { dtoUnits }, cancellationToken);
        }
    }
}