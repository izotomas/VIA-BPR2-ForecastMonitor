using System;
using ForecastMonitor.Service.DomainLogic.HttpClients.DataTransferObjects;
using Newtonsoft.Json.Converters;

namespace ForecastMonitor.Service.DomainLogic.HttpClients.Serialization
{
    public class DtoConverter<T> : CustomCreationConverter<T> where T : IDto, new()
    {
        private readonly int _installationId;

        public DtoConverter(int installationId)
        {
            this._installationId = installationId;
        }
        public override T Create(Type objectType)
        {
            return new T
            {
                InstallationId = this._installationId
            };
        }
    }
}
