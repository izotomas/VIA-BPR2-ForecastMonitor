using System;
using ForecastMonitor.Service.DomainLogic.HttpClients.DataTransferObjects;
using Newtonsoft.Json.Converters;

namespace ForecastMonitor.Service.DomainLogic.HttpClients.Serialization
{
    public class DtoUnitKeyConverter : CustomCreationConverter<DtoUnitKey>
    {
        private readonly int _installationId;

        public DtoUnitKeyConverter(int installationId)
        {
            _installationId = installationId;
        }

        public override DtoUnitKey Create(Type objectType)
        {
            return new DtoUnitKey
            {
                InstallationId = this._installationId,
                Client = new DtoClient
                {
                    InstallationId = this._installationId
                }
            };
        }
    }
}
