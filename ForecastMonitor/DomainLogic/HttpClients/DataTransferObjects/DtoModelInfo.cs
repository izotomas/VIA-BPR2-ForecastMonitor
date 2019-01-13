using System;
using Newtonsoft.Json;

namespace ForecastMonitor.Service.DomainLogic.HttpClients.DataTransferObjects
{
    [JsonObject]
    public sealed class DtoModelInfo : IDto
    {
        public int InstallationId { get; set; }
        [JsonProperty(PropertyName = "model_id")]
        public int ModelId { get; set; }
        [JsonProperty(PropertyName = "finished_training_on")]
        public DateTime FinishedTrainingOn { get; set; }
        [JsonProperty(PropertyName = "unit_key_id")]
        public int UnitKeyId { get; set; }
    }
}
