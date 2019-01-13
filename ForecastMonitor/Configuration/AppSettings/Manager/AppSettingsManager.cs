using System.Collections.Generic;
using ForecastMonitor.Service.AppSettings;
using Microsoft.Extensions.Configuration;

namespace ForecastMonitor.Service.Configuration.AppSettings.Manager
{
    public class AppSettingsManager: IAppSettingsManager
    {
        private readonly IConfiguration _configuration;

        public AppSettingsManager(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public int HistoricalDataLookupInDays => Get<int>(nameof(HistoricalDataLookupInDays));
        public int UnitPlotDefaultRangeInWeeks => Get<int>(nameof(UnitPlotDefaultRangeInWeeks));
        public IEnumerable<Installation> Installations => GetInstallations();
        public JobScheduling JobScheduling => Get<JobScheduling>(nameof(JobScheduling));

        private T Get<T>(string key)
        {
            var section = this._configuration.GetSection(key);
            return section.Get<T>();
        }

        private IEnumerable<Installation> GetInstallations()
        {
            var installations = Get<List<Installation>>(nameof(Installations));
            for(var i = 0; i < installations.Count; i++)
            {
                installations[i].Id = i + 1;
            }
            return installations;
        }
    }
}
