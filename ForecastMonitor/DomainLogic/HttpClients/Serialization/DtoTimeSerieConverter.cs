using System;
using ForecastMonitor.Service.DomainLogic.HttpClients.DataTransferObjects;
using Newtonsoft.Json.Converters;

namespace ForecastMonitor.Service.DomainLogic.HttpClients.Serialization
{
    public class DtoTimeSerieConverter : CustomCreationConverter<DtoTimeSerie>
    {
        private readonly int _installationId;

        public DtoTimeSerieConverter(int installationId)
        {
            this._installationId = installationId;
        }

        public override DtoTimeSerie Create(Type objectType)
        {
            return new DtoTimeSerie
            {
                InstallationId = this._installationId,
                UnitKey = new DtoUnitKey
                {
                    InstallationId = this._installationId,
                    Client = new DtoClient
                    {
                        InstallationId = this._installationId
                    }
                }
            };
        }
    }
}
