using Newtonsoft.Json;

namespace ForecastMonitor.Service.DomainLogic.HttpClients.DataTransferObjects
{
    [JsonObject]
    public sealed class DtoClient : IDto
    {
        public int InstallationId { get; set; }
        [JsonProperty(PropertyName = "client_id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "ts_start_days_ago")]
        public int TimeSeriesStartDaysAgo { get; set; }
        [JsonProperty(PropertyName = "aux_start_days_ago")]
        public int AuxiliarySeriesStartDaysAgo { get; set; }
        [JsonProperty(PropertyName = "client_key")]
        public string Key { get; set; } 
    }
}
