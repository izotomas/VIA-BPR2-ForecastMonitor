using System;
using System.Linq;
using AutoMapper;
using ForecastMonitor.Service.DataAccessLogic.DataAccessObjects;
using ForecastMonitor.Service.DataAccessLogic.DataServices.ModelDataService;
using ForecastMonitor.Service.DataAccessLogic.DataServices.PredictionDataService;
using ForecastMonitor.Service.DataAccessLogic.DataServices.TimeSerieDataService;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Statistics;

namespace ForecastMonitor.Service.DomainLogic.Statistics.PerformanceCalculationLogic
{
    public class PerformanceCalculationLogic : IPerformanceCalculationLogic
    {
        private readonly IMapper _mapper;
        private readonly IModelDataService _modelDataService;
        private readonly IPredictionDataService _predictionDataService;
        private readonly ITimeSerieDataService _timeSerieDataService;

        public PerformanceCalculationLogic(IMapper mapper,IModelDataService modelDataService, IPredictionDataService predictionDataService, ITimeSerieDataService timeSerieDataService)
        {
            _mapper = mapper;
            _modelDataService = modelDataService;
            _predictionDataService = predictionDataService;
            _timeSerieDataService = timeSerieDataService;
        }

        public double MAE(DaoModel model)
        {
            var predictionsByLatestModel = this._predictionDataService.GetPredictions(model).ToList();
            var timeSeriesByLatestModel = this._timeSerieDataService.GetTimeSeries(predictionsByLatestModel);
            if (predictionsByLatestModel.Any() && timeSeriesByLatestModel.Any())
            {
                var predictions = this._mapper.Map<Vector<double>>(predictionsByLatestModel);
                var timeSeries = this._mapper.Map<Vector<double>>(timeSeriesByLatestModel);

                var mae = MathNet.Numerics.Distance.MAE(predictions, timeSeries);
                return mae;
            }
            return double.NaN;
        }

        public double ZScore(DaoUnit unit)
        {
            var mostRecentModel = this._modelDataService.GetLatestEvaluableModel(unit);
            if (mostRecentModel?.Mae != null)
            {
                var predictionsByOlderModels = this._predictionDataService.GetPredictions(unit)
                    .Where(_ => _.ModelId != mostRecentModel.Id).ToList();
                var timeSeriesByOlderModels = this._timeSerieDataService.GetTimeSeries(predictionsByOlderModels);

                if (predictionsByOlderModels.Any() && timeSeriesByOlderModels.Any())
                {
                    var predictions = this._mapper.Map<Vector<double>>(predictionsByOlderModels);
                    var timeSeries = this._mapper.Map<Vector<double>>(timeSeriesByOlderModels);

                    var errors = Vector<double>.Abs(predictions.Subtract(timeSeries));

                    var mean = errors.Mean();
                    var std = errors.StandardDeviation();
                    var zScore = ZScore(mean, std, mostRecentModel.Mae.Value);
                    return zScore;
                }
            }

            return double.NaN;
        }

        private double ZScore(double mean, double std, double value)
        {
            return Math.Abs(value - mean) / std;
        }
    }
}