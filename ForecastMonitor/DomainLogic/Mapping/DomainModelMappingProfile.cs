using System;
using System.Collections.Generic;
using AutoMapper;
using ForecastMonitor.Service.DataAccessLogic.DataAccessObjects;
using ForecastMonitor.Service.DomainLogic.DomainModel;
using ForecastMonitor.Service.DomainLogic.HttpClients.DataTransferObjects;
using ForecastMonitor.Service.DomainLogic.Mapping.TypeConverters;
using ForecastMonitor.Service.DomainLogic.Mapping.ValueResolvers;
using MathNet.Numerics.LinearAlgebra;

namespace ForecastMonitor.Service.DomainLogic.Mapping
{
    public class DomainModelMappingProfile : Profile
    {
        public DomainModelMappingProfile()
        {
            CreateMap<DaoPrediction, double>()
                .ConvertUsing(dao => dao.Prediction);

            CreateMap<DaoTimeSerie, double>()
                .ConvertUsing(dao => dao.Value);

            CreateMap<IEnumerable<DaoPrediction>, Vector<double>>()
                .ConvertUsing<VectorConverter>();

            CreateMap<IEnumerable<DaoTimeSerie>, Vector<double>>()
                .ConvertUsing<VectorConverter>();

            CreateMap<DtoTimeSerie, GraphDataPoint<DateTime, double>>()
                .ForMember(graph => graph.X, opts => opts.MapFrom(ts => ts.Date.ToUniversalTime()))
                .ForMember(graph => graph.Y, opts => opts.ResolveUsing<TimeSerieLabelValueResolver>());

            CreateMap<DtoPrediction, GraphDataPoint<DateTime, double>>()
                .ForMember(graph => graph.X, opts => opts.MapFrom(prediction => prediction.PredictionForDate.ToUniversalTime()))
                .ForMember(graph => graph.Y, opts => opts.MapFrom(prediction => prediction.Prediction));
        }
    }
}
