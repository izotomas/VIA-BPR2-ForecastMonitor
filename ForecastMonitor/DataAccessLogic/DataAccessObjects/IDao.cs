using System;

namespace ForecastMonitor.Service.DataAccessLogic.DataAccessObjects
{
    public interface IDao
    {
        DateTime TimeStamp { get; set; }
        bool PrimaryKeyEquals(object obj);
    }
}
