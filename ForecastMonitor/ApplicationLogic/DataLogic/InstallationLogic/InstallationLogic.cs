using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ForecastMonitor.Service.ApplicationLogic.DataTransferObjects;
using ForecastMonitor.Service.DataAccessLogic.DataServices.InstallationDataService;

namespace ForecastMonitor.Service.ApplicationLogic.DataLogic.InstallationLogic
{
    public class InstallationLogic : IInstallationLogic
    {
        private readonly IInstallationDataService _dataService;
        private readonly IMapper _mapper;

        public InstallationLogic(IInstallationDataService dataService, IMapper mapper)
        {
            _dataService = dataService;
            _mapper = mapper;
        }

        public IEnumerable<DtoInstallation> GetInstallations()
        {
            var daoInstallations = _dataService.GetAllInstallations();
            var dtoInstallations = daoInstallations.Select(this._mapper.Map<DtoInstallation>);
            return dtoInstallations;
        }
    }
}