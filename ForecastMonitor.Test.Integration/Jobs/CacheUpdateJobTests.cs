using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using ForecastMonitor.Service.ApplicationLogic.Publishing.UnitPublishingService;
using ForecastMonitor.Service.Configuration.AppSettings.Manager;
using ForecastMonitor.Service.DataAccessLogic.DataServices;
using ForecastMonitor.Service.Jobs;
using ForecastMonitor.Service.Jobs.JobTypes;
using ForecastMonitor.Service.Jobs.JobTypes.Scheduled;
using ForecastMonitor.Test.Integration.TestUtils;
using ForecastMonitor.Test.Integration.TestUtils.TestServices;
using MediatR;
using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace ForecastMonitor.Test.Integration.Jobs
{
    [TestFixture]
    public class CacheUpdateJobTests : BaseIntegrationTest
    {
        private IScheduledJob _sut;
        private static readonly Mock<IUnitPublishingService> PublishingMock = new Mock<IUnitPublishingService>();

        public CacheUpdateJobTests() : base(services => {
            services.RemoveJobScheduler()
                .RemoveForecastSystemServices()
                .AddForecastSystemServiceWorkingMock()
                .AddCacheResetService()
                .AddSingleton(PublishingMock.Object);
        })
        {
        }

        [SetUp]
        public void Setup()
        {
            var serviceProvider = TestServer.Host.Services;
            var settingsManager = serviceProvider.GetRequiredService<IAppSettingsManager>();
            var scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            this._sut = new CacheUpdateJob(settingsManager, scopeFactory, loggerFactory);
        }

        [TearDown]
        public void TearDown()
        {
            var cacheResetService = TestServer.Host.Services.GetService<IMemoryDataContextResetService>();
            cacheResetService.ResetCache();
            PublishingMock.ResetCalls();
        }

        [Test]
        public async Task Update_Cache_Works()
        {
            // Arrange
            var serviceProvider = TestServer.Host.Services;
            var dataService = serviceProvider.GetService<IDataService>();

            // Act
            await this._sut.ExecuteAsync(CancellationToken.None);

            var actualClients = dataService.GetAllClients().ToList();
            var actualUnits = dataService.GetAllUnits().ToList();

            // Assert
            actualClients.Should().NotBeNullOrEmpty();
            actualUnits.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task PublishUnits_Is_Called_On_Successful_Update()
        {
            // Arrange
            var publisher = PublishingMock;

            // Act
            await this._sut.ExecuteAsync(CancellationToken.None);
            
            // Assert
            publisher.Verify(x => x.PublishUnits(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
