using FluentAssertions;
using ForecastMonitor.Service.Configuration.AppSettings.Manager;
using ForecastMonitor.Test.Integration.TestUtils;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace ForecastMonitor.Test.Integration.Configuration.AppSettings
{
    [TestFixture]
    public class AppSettingsManagerTests : BaseIntegrationTest
    {

        private IAppSettingsManager _sut;

        public AppSettingsManagerTests() : base(services =>
        {
            services.RemoveJobScheduler();
        })
        {
        }

        [SetUp]
        public void SetUp()
        {
            this._sut = TestServer.Host.Services.GetRequiredService<IAppSettingsManager>();
        }

        [Test]
        public void Read_HistoricalDataLookup()
        {
            // Arrange
            var expectedHistoricalDataLookupInDays = 28;

            // Act
            var actualHistoricalDataLookupInDays = _sut.HistoricalDataLookupInDays;

            // Assert
            actualHistoricalDataLookupInDays.Should().Be(expectedHistoricalDataLookupInDays);
        }

    }
}
