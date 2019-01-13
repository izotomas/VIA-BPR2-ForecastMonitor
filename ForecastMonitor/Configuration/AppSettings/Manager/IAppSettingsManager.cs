using System.Collections.Generic;
using ForecastMonitor.Service.AppSettings;

namespace ForecastMonitor.Service.Configuration.AppSettings.Manager
{
    public interface IAppSettingsManager
    {
        IEnumerable<Installation> Installations { get; }
        JobScheduling JobScheduling { get; }
        int HistoricalDataLookupInDays { get; }
        int UnitPlotDefaultRangeInWeeks { get; }
    }
}
