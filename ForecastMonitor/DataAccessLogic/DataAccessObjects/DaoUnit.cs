using System;

namespace ForecastMonitor.Service.DataAccessLogic.DataAccessObjects
{
    public class DaoUnit : IDao
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int InstallationId { get; set; }
        public string Key { get; set; }
        public string ClientKey { get; set; }
        public DateTime TimeStamp { get; set; }
        public double Mae { get; set; }
        public double ZScore { get; set; }

        public bool PrimaryKeyEquals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            var other = (DaoUnit) obj;
            return Id == other.Id &&
                   ClientId == other.ClientId &&
                   InstallationId == other.InstallationId;
        }
    }
}
