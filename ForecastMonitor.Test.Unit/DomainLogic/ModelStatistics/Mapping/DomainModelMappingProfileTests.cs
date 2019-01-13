using System;
using System.Linq;
using AutoFixture.NUnit3;
using AutoMapper;
using FluentAssertions;
using ForecastMonitor.Service.DataAccessLogic.DataAccessObjects;
using ForecastMonitor.Service.DomainLogic.DomainModel;
using ForecastMonitor.Service.DomainLogic.HttpClients.DataTransferObjects;
using ForecastMonitor.Service.DomainLogic.Mapping;
using MathNet.Numerics.LinearAlgebra;
using NUnit.Framework;

namespace ForecastMonitor.Test.Unit.DomainLogic.ModelStatistics.Mapping
{
    [TestFixture]
    public class DomainModelMappingProfileTests : BaseUnitTest
    {
        private IMapper _sut;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            this._sut = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new DomainModelMappingProfile());
                })
                .CreateMapper();
            this._sut.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [Test, AutoData]
        public void Tuple_Conversion_Test((DaoPrediction, DaoTimeSerie) tuple)
        {
            // Arrange
            var expected = new ValueTuple<double, double>(tuple.Item1.Prediction, tuple.Item2.Value);

            // Act
            var actual = _sut.Map<(double, double)>(tuple);

            // Assert
            actual.Should().Be(expected);
        }

        [Test, AutoData]
        public void Tuple_Array_Conversion_Test((DaoPrediction, DaoTimeSerie)[] tuples)
        {
            // Arrange
            var expected = tuples.Select(_ => new ValueTuple<double, double>(_.Item1.Prediction, _.Item2.Value)).ToArray();

            // Act
            var actual = _sut.Map<(double, double)[]>(tuples);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test, AutoData]
        public void Tuple_Array_Vector_Conversion_Test((DaoPrediction, DaoTimeSerie)[] tuples)
        {
            // Arrange
            var expectedPredictions = tuples.Select(_ => _.Item1.Prediction);
            var expectedTimeSeries = tuples.Select(_ => _.Item2.Value);

            // Act
            var actual = _sut.Map<(Vector<double>, Vector<double>)>(tuples);

            // Assert
            actual.Item1.Should().ContainInOrder(expectedPredictions);
            actual.Item2.Should().ContainInOrder(expectedTimeSeries);
            actual.Item1.Count.Should().Be(actual.Item2.Count);
        }

        [Test, AutoData]
        public void Prediction_To_GraphDataPoint_Test(DtoPrediction prediction)
        {
            // Arrange
            var expectedDataPoint = new GraphDataPoint<DateTime, double>
            {
                X = prediction.PredictionForDate.ToUniversalTime(),
                Y = prediction.Prediction
            };

            // Act
            var actualDataPoint = _sut.Map<GraphDataPoint<DateTime, double>>(prediction);

            // Assert
            actualDataPoint.Should().BeEquivalentTo(expectedDataPoint);
        }
    }
}
