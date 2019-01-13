using System;
using Newtonsoft.Json;

namespace ForecastMonitor.Service.ApplicationLogic.DataTransferObjects
{
    [JsonObject]
    public class DtoInstallation: IDto
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "last_update")]
        public DateTime LastUpdate { get; set; }
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; } 
    }
}
