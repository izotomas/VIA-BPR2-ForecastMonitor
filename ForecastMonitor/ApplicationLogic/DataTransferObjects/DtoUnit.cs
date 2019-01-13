using System;
using Newtonsoft.Json;

namespace ForecastMonitor.Service.ApplicationLogic.DataTransferObjects
{
    [JsonObject]
    public class DtoUnit : IDto
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "last_update")]
        public DateTime LastUpdate { get; set; }
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "client_id")]
        public int ClientId { get; set; }
        [JsonProperty(PropertyName = "mae")]
        public double Mae { get; set; }
        [JsonProperty(PropertyName = "performance")]
        public string Performance { get; set; }
    }
}
