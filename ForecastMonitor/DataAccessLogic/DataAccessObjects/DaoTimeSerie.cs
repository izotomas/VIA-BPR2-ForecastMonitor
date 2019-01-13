using System;

namespace ForecastMonitor.Service.DataAccessLogic.DataAccessObjects
{
    public class DaoTimeSerie : IDao
    {
        public DateTime TimeStamp { get; set; }
        public int UnitId { get; set; }
        public int InstallationId { get; set; }
        public DateTime Date { get; set; }
        public double Value { get; set; }

        public bool PrimaryKeyEquals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            var other = (DaoTimeSerie)obj;
            return Date.Equals(other.Date) &&
                   UnitId == other.UnitId &&
                   InstallationId == other.InstallationId;
        }
    }
}
