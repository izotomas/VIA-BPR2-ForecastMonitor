using System;

namespace ForecastMonitor.Service.DataAccessLogic.DataAccessObjects
{

    public class DaoModel : IDao
    {
        public DateTime TimeStamp { get; set; }
        public int Id { get; set; }
        public int InstallationId { get; set; }
        public int UnitId { get; set; }
        public DateTime TrainedOn { get; set; }
        public double? Mae { get; set; }
        public bool IsLatestEvaluable { get; set; }

        public bool PrimaryKeyEquals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            var other = (DaoModel)obj;

            return InstallationId == other.InstallationId &&
                   UnitId == other.Id &&
                   // to keep only one latest evaluable model per unit 
                   ((TrainedOn.Equals(other.TrainedOn) && Id == other.Id) || (IsLatestEvaluable && other.IsLatestEvaluable));
        }
    }
}
