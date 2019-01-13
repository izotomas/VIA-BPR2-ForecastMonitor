using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using ForecastMonitor.Service.Configuration.AppSettings.Manager;
using ForecastMonitor.Service.DomainLogic.HttpClients.ForecastSystemClient;
using ForecastMonitor.Shared;
using ForecastMonitor.Test.Integration.TestUtils;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using WireMock.Server;

namespace ForecastMonitor.Test.Integration.DomainLogic.HttpClients
{
    [TestFixture]
    public class ForecastSystemClientTests : BaseIntegrationTest
    {
        private const int StubPort = 6061;
        private FluentMockServer _forecastSystemStub;

        private IForecastSystemClient _sut;
        public ForecastSystemClientTests() : base(services => { services.RemoveJobScheduler(); })
        {
        }

        [SetUp]
        public void SetUp()
        {
            _sut = TestServer.Host.Services.GetRequiredService<IForecastSystemClient>();
            _forecastSystemStub = FluentMockServer.Start(StubPort);
        }

        [TearDown]
        public void TearDown()
        {
            _forecastSystemStub.Dispose();
        }

        [Test]
        public void Get_Historical_Predictions()
        {
            // Arrange
            _forecastSystemStub.ConfigurePredictionsRequest();
            var manager = TestServer.Host.Services.GetRequiredService<IAppSettingsManager>();
            var installation = manager.Installations.ToList().First();

            // Act
            var predictions = _sut.GetPredictions(installation, DateTime.Now, DateTime.Now).Result.ToList();
            
            // Assert
            predictions.Should().NotBeNullOrEmpty();
            predictions.Should().OnlyContain(prediction => prediction.InstallationId == installation.Id);
        }

        [Test]
        public void Get_UnitKeys()
        {
            // Arrange
            _forecastSystemStub.ConfigureClientWithFixedData();
            var manager = TestServer.Host.Services.GetRequiredService<IAppSettingsManager>();
            var installation = manager.Installations.ToList().First();

            // Act
            var units = _sut.GetUnits(installation).Result.ToList();

            // Assert
            units.Should().NotBeNullOrEmpty();
            units.Select(unit => unit.Client.InstallationId).Should().AllBeEquivalentTo(installation.Id);
        }

        [Test]
        public void Get_TimeSeries()
        {
            // Arrange
            _forecastSystemStub.ConfigureTimeSeriesRequest();
            var manager = TestServer.Host.Services.GetRequiredService<IAppSettingsManager>();
            var installation = manager.Installations.ToList().First();

            var expectedUnitKey = "0452B832-85BC-4457-84BF-738B509DDE45_PatientsAtDepartment";
            var expectedClientKey = "PatientCapacity";

            // Act
            var timeSeries = _sut.GetTimeSeries(installation, expectedUnitKey, expectedClientKey, DateTime.Now, DateTime.Now, 1).Result.ToList();

            // Assert
            timeSeries.Should().ContainSingle(ts => ts.UnitKey.UnitKey.Equals(expectedUnitKey) && ts.UnitKey.Client.Key.Equals(expectedClientKey));
        }

        [Test]
        public async Task Retry_Policy_Ensures_Successful_Request()
        {
            // Arrange
            var rand = new Random();
            var triesAfterServerRespondsWithinPolicyLimit = rand.Next(1, 5);
            _forecastSystemStub.ConfigureClientUnresponsiveWithFixedData(triesAfterServerRespondsWithinPolicyLimit);
            var manager = TestServer.Host.Services.GetRequiredService<IAppSettingsManager>();
            var installation = manager.Installations.ToList().First();

            // Act
            var clients = await _sut.GetClients(installation);

            // Assert
            clients.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void Client_Fails_After_Retry_Policy_Expires()
        {
            // Arrange
            var triesAfterServerRespondsOutOfPolicyLimit = 6;
            _forecastSystemStub.ConfigureClientUnresponsiveWithFixedData(triesAfterServerRespondsOutOfPolicyLimit);
            var manager = TestServer.Host.Services.GetRequiredService<IAppSettingsManager>();
            var installation = manager.Installations.ToList().First();

            // Act
            Func<Task> getThatShouldExpire = async() => { await _sut.GetClients(installation); };

            // Assert
            getThatShouldExpire.Should().Throw<Exception>();
        }
    }
}
