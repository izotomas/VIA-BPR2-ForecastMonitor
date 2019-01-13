using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ForecastMonitor.Service.ApplicationLogic.DataTransferObjects;
using ForecastMonitor.Service.DataAccessLogic.DataContext;
using ForecastMonitor.Service.DataAccessLogic.DataServices.ClientDataService;

namespace ForecastMonitor.Service.ApplicationLogic.DataLogic.ClientLogic
{
    public class ClientLogic : IClientLogic
    {
        private readonly IClientDataService _dataService;
        private readonly IMapper _mapper;

        public ClientLogic(IClientDataService dataService, IMapper mapper)
        {
            _dataService = dataService;
            _mapper = mapper;
        }

        public IEnumerable<DtoClient> GetClients(int installationId)
        {
            var daoClients = this._dataService.GetClients(installationId);
            var dtoClients = daoClients.Select(_mapper.Map<DtoClient>);
            return dtoClients;
        }
    }
}