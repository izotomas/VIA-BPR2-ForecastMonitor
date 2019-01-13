using System;
using AutoMapper;
using ForecastMonitor.Service.DataAccessLogic.DataAccessObjects;
using ForecastMonitor.Service.DomainLogic.DomainModel;
using ForecastMonitor.Service.DomainLogic.HttpClients.DataTransferObjects;

namespace ForecastMonitor.Service.DomainLogic.Mapping.ValueResolvers
{
    public class TimeSerieLabelValueResolver : 
        IValueResolver<DtoTimeSerie, DaoTimeSerie, double>, 
        IValueResolver<DtoTimeSerie, GraphDataPoint<DateTime, double>, double>
    {
        public double Resolve(DtoTimeSerie source, DaoTimeSerie destination, double destMember, ResolutionContext context)
        {
            var value = double.Parse(source.Label);
            return value;
        }

        public double Resolve(DtoTimeSerie source, GraphDataPoint<DateTime, double> destination, double destMember, ResolutionContext context)
        {
            var value = double.Parse(source.Label);
            return value;
        }
    }
}
