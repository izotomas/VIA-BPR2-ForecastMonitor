using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using ForecastMonitor.Service.AppSettings;
using ForecastMonitor.Service.Configuration.AppSettings.Manager;
using ForecastMonitor.Service.DomainLogic.ForecastSystemService;
using ForecastMonitor.Service.DomainLogic.HttpClients.DataTransferObjects;
using ForecastMonitor.Service.DomainLogic.HttpClients.ForecastSystemClient;
using Moq;
using NUnit.Framework;

namespace ForecastMonitor.Test.Unit.DomainLogic
{
    [TestFixture]
    public class ForecastSystemServiceTests : BaseUnitTest
    {
        private ForecastSystemService _sut;

        [Test]
        public void GetMappablePredictionsAndTimeSeries_Returns_Mappable_Predictions_And_TimeSeries()
        {
            // Arrange
            var rand = new Random();
            var model = Fixture.Create<DtoModelInfo>();
            var unit = Fixture.Create<DtoUnitKey>();
            var dates = Fixture.CreateMany<DateTime>(50).ToList();

            Fixture.Inject(Mock.Of<IForecastSystemClient>(client =>
                client.GetPredictions(It.IsAny<Installation>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<int?>()) == 
                    Task.FromResult(CreateRandomPredictionsFromDates(dates, rand)) &&
                client.GetTimeSeries(It.IsAny<Installation>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<int?>()) ==
                    Task.FromResult(CreateRandomTimeSeriesFromDates(dates, rand)))
            );

            _sut = Fixture.Create<ForecastSystemService>();

            // Act
            var result = _sut.GetMappablePredictionsAndTimeSeries(unit, model).Result;
            var actualPredictionDates = result.Item1.Select(x => x.PredictionForDate).ToList();
            var actualTimeSeriesDates = result.Item2.Select(x => x.Date).ToList();

            // Assert
            foreach (var date in actualTimeSeriesDates)
            {
                actualPredictionDates.Should().Contain(date);
            }

            foreach (var date in actualPredictionDates)
            {
                actualTimeSeriesDates.Should().Contain(date);
            }
        }

        [Test, Description("PP1-2")]
        public void GetMappablePredictionsAndTimeSeries_Uses_Configurable_HistoricalDataLookupInDays()
        {
            // Arrange
            var actualLookupInDays = 5;
            var rand = new Random();
            var model = Fixture.Create<DtoModelInfo>();
            var unit = Fixture.Create<DtoUnitKey>();
            var dates = Fixture.CreateMany<DateTime>(50)
                .OrderBy(_ => _.Date)
                .ToList();

            var predictions = CreateRandomPredictionsFromDates(dates, rand);
            var timeSeries = CreateRandomTimeSeriesFromDates(dates, rand);


            var firstAllowedPredictionDate = predictions.First().PredictionForDate.AddDays(-actualLookupInDays);

            Fixture.Inject(Mock.Of<IForecastSystemClient>(client =>
                client.GetPredictions(It.IsAny<Installation>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<int?>()) ==
                Task.FromResult(predictions) &&
                client.GetTimeSeries(It.IsAny<Installation>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<int?>()) ==
                Task.FromResult(timeSeries))
            );
            Fixture.Inject(Mock.Of<IAppSettingsManager>(manager => manager.HistoricalDataLookupInDays == actualLookupInDays));

            _sut = Fixture.Create<ForecastSystemService>();

            // Act
            var result = _sut.GetMappablePredictionsAndTimeSeries(unit, model).Result;

            var actualPredictionDates = result.Item1.Select(x => x.PredictionForDate).ToList();
            var actualTimeSeriesDates = result.Item2.Select(x => x.Date).ToList();

            // Assert
            actualPredictionDates.Should().OnlyContain(_ => _ >= firstAllowedPredictionDate);
            actualTimeSeriesDates.Should().OnlyContain(_ => _ >= firstAllowedPredictionDate);
        }

        #region Helpers
        private IEnumerable<DtoPrediction> CreateRandomPredictionsFromDates(IReadOnlyList<DateTime> dates, Random rand)
        {
            var predictions = new List<DtoPrediction>();
            var count = rand.Next(100);
            while (count-- > 0)
            {
                var prediction = Fixture.Build<DtoPrediction>()
                    .With(x => x.PredictionForDate, dates[rand.Next(dates.Count)])
                    .Create();
                predictions.Add(prediction);
            }

            predictions = predictions
                .OrderBy(_ => _.PredictionForDate)
                .ToList();

            return predictions;
        }

        private IEnumerable<DtoTimeSerie> CreateRandomTimeSeriesFromDates(IReadOnlyList<DateTime> dates, Random rand)
        {
            var timeSeries = new List<DtoTimeSerie>();
            var count = rand.Next(100);
            while (count-- > 0)
            {
                var timeSerie = Fixture.Build<DtoTimeSerie>()
                    .With(x => x.Date, dates[rand.Next(dates.Count)])
                    .Create();
                timeSeries.Add(timeSerie);
            }

            timeSeries = timeSeries
                .OrderBy(_ => _.Date)
                .ToList();

            return timeSeries;
        }
        #endregion
    }
}
