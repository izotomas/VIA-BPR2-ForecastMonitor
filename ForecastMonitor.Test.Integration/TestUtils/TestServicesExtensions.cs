using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using ForecastMonitor.Service.AppSettings;
using ForecastMonitor.Service.DataAccessLogic.DataContext;
using ForecastMonitor.Service.DomainLogic.ForecastSystemService;
using ForecastMonitor.Service.DomainLogic.HttpClients.DataTransferObjects;
using ForecastMonitor.Service.DomainLogic.HttpClients.ForecastSystemClient;
using ForecastMonitor.Service.Jobs.ScheduledJobService;
using ForecastMonitor.Test.Integration.TestUtils.TestServices;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace ForecastMonitor.Test.Integration.TestUtils
{
    public static class TestServicesExtensions
    {
        private const int DefaultInstallationCount = 1;
        private const int DefaultClientCount = 2;
        private const int DefaultUnitKeyCount = 8;

        /// <summary>
        /// Prevents Job scheduler from running
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection RemoveJobScheduler(this IServiceCollection services)
        {
            var scheduler = services.FirstOrDefault(descriptor => descriptor.ImplementationFactory?.GetType() == typeof(Func<IServiceProvider, ScheduledJobService>));
            services.Remove(scheduler);
            return services;
        }

        public static IServiceCollection ReplaceService<TImplementation>(this IServiceCollection services, TImplementation service) 
            where TImplementation : class
        {
            var serviceToReplace = services.FirstOrDefault(descriptor => descriptor.ImplementationType == typeof(TImplementation));
            services.Remove(serviceToReplace);
            services.AddSingleton(service);
            return services;
        }

        public static IServiceCollection ReplaceServices<TServiceType>(this IServiceCollection services, TServiceType service)
            where TServiceType : class
        {
            services.RemoveAll(typeof(TServiceType));
            services.AddSingleton(service);
            return services;
        }

        public static IServiceCollection RemoveForecastSystemServices(this IServiceCollection services)
        {
           return services.RemoveAll(typeof(IForecastSystemService));
        }

        public static IServiceCollection AddCacheConfiguration(this IServiceCollection services, int cacheEntryExpirationInSec)
        {
            return services.AddScoped<IDataContext, MemoryDataContext>(serviceProvider => new MemoryDataContext(serviceProvider.GetService<IMemoryCache>(), cacheEntryExpirationInSec));
        }

        public static IServiceCollection AddForecastSystemServiceWorkingMock(this IServiceCollection services)
        {
            return services.AddSingleton(Mock.Of<IForecastSystemService>(service =>
                service.GetInstallation() == TestDataGenerator.CreateInstallations(1).First() &&
                service.GetClients() == Task.FromResult(TestDataGenerator.CreateClients(1, 1)) &&
                service.GetUnits() == Task.FromResult(TestDataGenerator.CreateUnits(1, 1, 1)) &&
                service.GetMappablePredictionsAndTimeSeries(It.IsAny<DtoUnitKey>(), It.IsAny<DtoModelInfo>()) == 
                    Task.FromResult(TestDataGenerator.GetFixture().Create<(IEnumerable<DtoPrediction>, IEnumerable<DtoTimeSerie>)>()) &&
                service.GetModelInfos(It.IsAny<IEnumerable<DtoPrediction>>()) == Task.FromResult(TestDataGenerator.GetFixture().CreateMany<DtoModelInfo>())
            ));
        }

        /// <summary>
        /// Overrides a default HttpClient to a mock with generated data
        /// </summary>
        /// <param name="services"></param>
        /// <param name="installationsCount"></param>
        /// <param name="clientsPerInstallationCount"></param>
        /// <param name="unitKeysPerClientCount"></param>
        /// <returns></returns>
        public static IServiceCollection AddForecastSystemClientMock(this IServiceCollection services, 
            int installationsCount = DefaultInstallationCount,
            int clientsPerInstallationCount = DefaultClientCount,
            int unitKeysPerClientCount = DefaultUnitKeyCount)
        {

            return services.AddSingleton(Mock.Of<IForecastSystemClient>(client =>
                client.GetClients(It.IsAny<Installation>()) == Task.FromResult(TestDataGenerator.CreateClients(installationsCount, clientsPerInstallationCount)) &&
                client.GetUnits(It.IsAny<Installation>()) == Task.FromResult(TestDataGenerator.CreateUnits(installationsCount, clientsPerInstallationCount, unitKeysPerClientCount))
            ));
        }

        public static IServiceCollection AddCacheResetService(this IServiceCollection services)
        {
            return services.AddScoped<IMemoryDataContextResetService, MemoryDataContextResetService>();
        }
    }
}
