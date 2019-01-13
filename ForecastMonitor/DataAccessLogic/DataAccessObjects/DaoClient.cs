using System;

namespace ForecastMonitor.Service.DataAccessLogic.DataAccessObjects
{
    public class DaoClient : IDao
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int InstallationId { get; set; }
        public DateTime TimeStamp { get; set; }

        public bool PrimaryKeyEquals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            var other = (DaoClient) obj;
            return Id == other.Id &&
                   InstallationId == other.InstallationId;
        }
    }
}
