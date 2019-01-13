using System;
using System.Collections.Generic;
using ForecastMonitor.Service.DomainLogic.DomainModel;
using Newtonsoft.Json;

namespace ForecastMonitor.Service.ApplicationLogic.DataTransferObjects
{
    [JsonObject]
    public class DtoUnitPlot
    {
        [JsonProperty(PropertyName = "unit_id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName ="name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "installation_id")]
        public int InstallationId { get; set; }
        [JsonProperty(PropertyName = "unit_key")]
        public string UnitKey { get; set; }
        [JsonProperty(PropertyName = "historical")]
        public IEnumerable<GraphDataPoint<DateTime, double>> Historical { get; set; }
        [JsonProperty(PropertyName = "predictions")]
        public IEnumerable<GraphDataPoint<DateTime, double>> Predictions { get; set; }
    }
}
