using Newtonsoft.Json;

namespace ForecastMonitor.Service.DomainLogic.HttpClients.DataTransferObjects
{
    [JsonObject]
    public sealed class DtoUnitKey: IDto
    {
        public int InstallationId { get; set; }
        [JsonProperty(PropertyName = "unit_key_id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "client_id")]
        public int ClientId { get; set; }
        [JsonProperty(PropertyName = "client")]
        public DtoClient Client { get; set; }
        [JsonProperty(PropertyName = "unit_key")]
        public string UnitKey { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
