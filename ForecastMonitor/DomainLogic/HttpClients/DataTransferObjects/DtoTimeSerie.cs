using System;
using Newtonsoft.Json;

namespace ForecastMonitor.Service.DomainLogic.HttpClients.DataTransferObjects
{
    [JsonObject]
    public sealed class DtoTimeSerie : IDto 
    {
        public int InstallationId { get; set; }
        [JsonProperty(PropertyName = "date")]
        public DateTime Date { get; set; }
        [JsonProperty(PropertyName = "unit_key_id")]
        public int UnitKeyId { get; set; }
        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }
        [JsonProperty(PropertyName = "unit_key")]
        public DtoUnitKey UnitKey { get; set; }
    }
}
