using System.Collections.Generic;
using AutoFixture;
using AutoFixture.AutoMoq;
using ForecastMonitor.Service.AppSettings;
using ClientDto = ForecastMonitor.Service.DomainLogic.HttpClients.DataTransferObjects;

namespace ForecastMonitor.Test.Integration.TestUtils
{
    public static class TestDataGenerator
    {
        private static readonly IFixture Fixture = new Fixture().Customize(new AutoMoqCustomization());

        public static IFixture GetFixture()
        {
            return Fixture;
        }

        public static IEnumerable<Installation> CreateInstallations(int installationsCount)
        {
            for (var i = 0; i < installationsCount; i++)
            {
                yield return Fixture.Build<Installation>()
                    .With(x => x.Id, i + 1)
                    .Create();
            }
        }

        public static IEnumerable<ClientDto.DtoClient> CreateClients(int installationsCount, int clientsPerInstallationCount)
        {
            for (var i = 0; i < installationsCount; i++)
            for (var j = 0; j < clientsPerInstallationCount; j++)
            {
                yield return Fixture.Build<ClientDto.DtoClient>()
                    .With(x => x.InstallationId, i + 1)
                    .With(x => x.Id, j + 1)
                    .Create();
            }
        }

        public static IEnumerable<ClientDto.DtoUnitKey> CreateUnits(int installationsCount, int clientsPerInstallationCount, int unitKeysPerClientCount)
        {
            for (var i = 0; i < installationsCount; i++)
            for (var j = 0; j < clientsPerInstallationCount; j++)
            for (var k = 0; k < unitKeysPerClientCount; k++)
            {
                yield return Fixture.Build<ClientDto.DtoUnitKey>()
                    .With(x => x.InstallationId, i + 1)
                    .With(x => x.ClientId, j + 1)
                    .With(x => x.Id, k + 1)
                    .Create();
            }
        }
    }
}
