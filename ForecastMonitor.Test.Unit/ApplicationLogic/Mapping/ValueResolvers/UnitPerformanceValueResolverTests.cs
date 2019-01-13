using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using AutoFixture.NUnit3;
using AutoMapper;
using FluentAssertions;
using ForecastMonitor.Service.ApplicationLogic.DataTransferObjects;
using ForecastMonitor.Service.ApplicationLogic.Mapping;
using ForecastMonitor.Service.DataAccessLogic.DataAccessObjects;
using NUnit.Framework;

namespace ForecastMonitor.Test.Unit.ApplicationLogic.Mapping.ValueResolvers
{
    [TestFixture]
    public class UnitPerformanceValueResolverTests : BaseUnitTest
    {
        private IMapper _sut;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            this._sut = new MapperConfiguration(cfg => cfg.AddProfile(new DaoToDtoMappingProfile()))
                .CreateMapper();
            this._sut.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [Test, AutoData, Description("PP5-1")]
        public void ZScore_To_Colored_Performance_Indicator_Test(IList<DaoUnit> units)
        {
            // Arrange
            var expectedColors = units.Select(_ =>
            {
                if (_.ZScore < 1.5) return "Green";
                if (_.ZScore < 3) return "Yellow";
                if (_.ZScore >= 3) return "Red";
                return "Grey";
            });

            // Act
            var dtoUnits = units.Select(_sut.Map<DtoUnit>);
            var actualColors = dtoUnits.Select(_ => _.Performance);

            // Assert
            actualColors.Should().BeEquivalentTo(expectedColors);
        }
    }
}
