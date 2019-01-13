using System.Collections.Generic;
using ForecastMonitor.Service.ApplicationLogic.DataLogic.ClientLogic;
using ForecastMonitor.Service.ApplicationLogic.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ForecastMonitor.Service.Api
{
    [Route("")]
    [ApiController]
    public class ClientController : BaseController
    {
        private readonly IClientLogic _client;

        public ClientController(IClientLogic client, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            _client = client;
        }

        [HttpGet]
        [Route("clients")]
        public IEnumerable<DtoClient> GetClients([FromQuery] int installationId)
        {
            Logger.LogDebug($"Calling {nameof(GetClients)} with {nameof(installationId)}: {installationId}");
            return HttpGet(this._client.GetClients(installationId));
        }
    }
}
