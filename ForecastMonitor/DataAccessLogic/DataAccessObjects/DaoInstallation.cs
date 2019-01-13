using System;

namespace ForecastMonitor.Service.DataAccessLogic.DataAccessObjects
{
    public class DaoInstallation : IDao
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }

        public bool PrimaryKeyEquals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            var other = (DaoInstallation) obj;
            return Id == other.Id;
        }
    }
}
