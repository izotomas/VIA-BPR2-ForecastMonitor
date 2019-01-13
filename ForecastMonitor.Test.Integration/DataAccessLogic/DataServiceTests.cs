using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using ForecastMonitor.Service.DataAccessLogic.DataAccessObjects;
using ForecastMonitor.Service.DataAccessLogic.DataServices;
using ForecastMonitor.Test.Integration.TestUtils;
using ForecastMonitor.Test.Integration.TestUtils.TestServices;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace ForecastMonitor.Test.Integration.DataAccessLogic
{
    [TestFixture]
    public class DataServiceTests : BaseIntegrationTest
    {
        private const int DataEntryExpirationInSec = 1;
        private IDataService _sut;

        public DataServiceTests() : base(services => {
            services.AddCacheConfiguration(DataEntryExpirationInSec)
                .AddCacheResetService()
                .RemoveJobScheduler();
        })
        {
        }

        [SetUp]
        public void SetUp()
        {
            this._sut = TestServer.Host.Services.GetService<IDataService>();
        }

        [TearDown]
        public void TearDown()
        {
            var cacheResetService = TestServer.Host.Services.GetService<IMemoryDataContextResetService>();
            cacheResetService.ResetCache();
        }

        [Test]
        public void Insert_And_Get_New_Data()
        {
            // Arrange
            var expectedInstallations = Fixture.Build<DaoInstallation>()
                .With(x => x.TimeStamp, DateTime.Now)
                .CreateMany()
                .ToList();

            var expectedClients = Fixture.Build<DaoClient>()
                .With(x => x.TimeStamp, DateTime.Now)
                .CreateMany()
                .ToList();

            var expectedUnits = Fixture.Build<DaoUnit>()
                .With(x => x.TimeStamp, DateTime.Now)
                .CreateMany()
                .ToList();

            // Act
            this._sut.UpsertInstallations(expectedInstallations);
            var actualInstallations = this._sut.GetAllInstallations();

            this._sut.UpsertClients(expectedClients);
            var actualClients = this._sut.GetAllClients();
            
            this._sut.UpsertUnits(expectedUnits);
            var actualUnits = this._sut.GetAllUnits();

            // Assert
            expectedInstallations.Should().BeEquivalentTo(actualInstallations);
            expectedClients.Should().BeEquivalentTo(actualClients);
            expectedUnits.Should().BeEquivalentTo(actualUnits);
        }

        [Test]
        public void Remove_Expired_Data()
        {
            // Arrange
            var expiredInstallation = Fixture.Build<DaoInstallation>()
                .With(x => x.TimeStamp, DateTime.Today.AddSeconds( - DataEntryExpirationInSec))
                .Create();

            var newInstallation = Fixture.Build<DaoInstallation>()
                .With(x => x.TimeStamp, DateTime.Now)
                .Create();

            // Act
            this._sut.UpsertInstallations(new List<DaoInstallation>
            {
                expiredInstallation,
                newInstallation
            });

            var actualInstallations = this._sut.GetAllInstallations().ToList();

            // Assert
            actualInstallations.Should().NotBeEmpty();
            actualInstallations.Should().AllBeEquivalentTo(newInstallation);
        }

        [Test]
        public void Update_Expired_Data()
        {
            // Arrange
            var oldUnit = Fixture.Build<DaoUnit>()
                .With(x => x.Id, 1)
                .With(x => x.ClientId, 1)
                .With(x => x.InstallationId, 1)
                .With(x => x.TimeStamp, DateTime.Now)
                .Create();

            var newUnit = Fixture.Build<DaoUnit>()
                .With(x => x.Id, 1)
                .With(x => x.ClientId, 1)
                .With(x => x.InstallationId, 1)
                .With(x => x.TimeStamp, DateTime.Now)
                .Create();

            // Act
            this._sut.UpsertUnits(new List<DaoUnit>{oldUnit});
            this._sut.UpsertUnits(new List<DaoUnit>{newUnit});

            var actualUnits = this._sut.GetAllUnits().ToList();

            // Assert
            actualUnits.Count.Should().Be(1);
            actualUnits.Should().AllBeEquivalentTo(newUnit);
            actualUnits.Should().NotContain(oldUnit);
        }
    }
}
