using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using ForecastMonitor.Service.ApplicationLogic.DataLogic.InstallationLogic;
using ForecastMonitor.Service.ApplicationLogic.DataTransferObjects;
using ForecastMonitor.Service.ApplicationLogic.Mapping;
using ForecastMonitor.Service.DataAccessLogic.DataAccessObjects;
using ForecastMonitor.Service.DataAccessLogic.DataServices.InstallationDataService;
using Moq;
using NUnit.Framework;

namespace ForecastMonitor.Test.Unit.ApplicationLogic.DataLogic
{
    [TestFixture]
    public class InstallationLogicTests : BaseUnitTest
    {
        private InstallationLogic _sut;

        [Test]
        public void Get_Installations()
        {
            // Arrange
            Fixture.Freeze<Mock<IInstallationDataService>>()
                .Setup(cfg => cfg.GetAllInstallations())
                .Returns(new List<DaoInstallation>
                {
                    new DaoInstallation
                    {
                        Id = 1,
                        Name = "Test",
                        Url = "localhost"
                    }
                });

            var mapper = new Mapper(new MapperConfiguration(cfg => { cfg.AddProfile<DaoToDtoMappingProfile>(); }));
            Fixture.Inject<IMapper>(mapper);

            var expectedInstallations = new List<DtoInstallation>
            {
                new DtoInstallation
                {
                    Id = 1,
                    Name = "Test"
                }
            };

            _sut = Fixture.Build<InstallationLogic>().Create();

            // Act
            var actualInstallations = _sut.GetInstallations().ToList();

            // Assert
            actualInstallations.Should().NotBeEmpty();
            actualInstallations.Should().BeEquivalentTo(expectedInstallations);
        }
    }
}
