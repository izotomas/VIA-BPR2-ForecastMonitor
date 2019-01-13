using System;
using Newtonsoft.Json;

namespace ForecastMonitor.Service.DomainLogic.HttpClients.DataTransferObjects
{
    [JsonObject]
    public sealed class DtoPrediction : IDto
    {
        public int InstallationId { get; set; }
        [JsonProperty(PropertyName = "model_id")]
        public int ModelId { get; set; }
        [JsonProperty(PropertyName = "unit_key_id")]
        public int UnitKeyId { get; set; }
        [JsonProperty(PropertyName = "prediction")]
        public double Prediction { get; set; }
        [JsonProperty(PropertyName = "prediction_for_date")]
        public DateTime PredictionForDate { get; set; }
        [JsonProperty(PropertyName = "prediction_made")]
        public DateTime PredictionMade { get; set; }
        [JsonProperty(PropertyName = "prediction_id")]
        public long PredictionId { get; set; }
    }
}