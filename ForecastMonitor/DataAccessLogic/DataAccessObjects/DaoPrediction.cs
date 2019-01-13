using System;

namespace ForecastMonitor.Service.DataAccessLogic.DataAccessObjects
{
    public class DaoPrediction : IDao
    {
        public DateTime TimeStamp { get; set; }
        public long Id { get; set; }
        public int InstallationId { get; set; }
        public int UnitId { get; set; }
        public int ModelId { get; set; }
        public DateTime PredictionFor { get; set; }
        public DateTime PredictionMade { get; set; }
        public double Prediction { get; set; }

        public bool PrimaryKeyEquals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            var other = (DaoPrediction)obj;
            return Id == other.Id &&
                   ModelId == other.ModelId &&
                   UnitId == other.UnitId &&
                   InstallationId == other.InstallationId;
        }
    }
}
