using ForecastMonitor.Service.Configuration.AppSettings.Manager;
using ForecastMonitor.Service.DomainLogic.HttpClients.ForecastSystemClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ForecastMonitor.Service.DomainLogic.ForecastSystemService
{
    public static class ForecastSystemServiceExtensions
    {
        public static IServiceCollection AddForecastSystemServices(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettings = new AppSettingsManager(configuration);
            var installations = appSettings.Installations;

            foreach (var installation in installations)
            {
                services.AddScoped<IForecastSystemService, ForecastSystemService>(serviceProvider =>
                {
                    var client = serviceProvider.GetRequiredService<IForecastSystemClient>();
                    var logger = serviceProvider.GetRequiredService<ILoggerFactory>();
                    var instance = new ForecastSystemService(installation, client, appSettings, logger);
                    return instance;
                });
            }

            return services;
        }
    }
}
