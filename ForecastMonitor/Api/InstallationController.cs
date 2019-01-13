using System.Collections.Generic;
using ForecastMonitor.Service.ApplicationLogic.DataLogic.InstallationLogic;
using ForecastMonitor.Service.ApplicationLogic.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ForecastMonitor.Service.Api
{
    [Route("")]
    [ApiController]
    public class InstallationController : BaseController
    {
        private readonly IInstallationLogic _installation;

        public InstallationController(IInstallationLogic installation, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            _installation = installation;
        }

        [HttpGet]
        [Route("installations")]
        public IEnumerable<DtoInstallation> GetInstallations()
        {
            Logger.LogDebug($"Calling {nameof(GetInstallations)}");
            return HttpGet(this._installation.GetInstallations());
        }
    }
}
