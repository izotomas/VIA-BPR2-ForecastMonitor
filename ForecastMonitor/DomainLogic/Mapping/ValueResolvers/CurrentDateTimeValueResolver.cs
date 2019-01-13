using System;
using AutoMapper;
using ForecastMonitor.Service.AppSettings;
using ForecastMonitor.Service.DataAccessLogic.DataAccessObjects;
using ForecastMonitor.Service.DomainLogic.HttpClients.DataTransferObjects;

namespace ForecastMonitor.Service.DomainLogic.Mapping.ValueResolvers
{
    public class CurrentDateTimeValueResolver : 
        IValueResolver<DtoClient, DaoClient, DateTime>, 
        IValueResolver<DtoUnitKey, DaoUnit, DateTime>, 
        IValueResolver<Installation, DaoInstallation, DateTime>, 
        IValueResolver<DtoModelInfo, DaoModel, DateTime>, 
        IValueResolver<DtoPrediction, DaoPrediction, DateTime>,
        IValueResolver<DtoTimeSerie, DaoTimeSerie, DateTime>
    {
        public DateTime Resolve(DtoClient source, DaoClient destination, DateTime destMember, ResolutionContext context)
        {
            return DateTime.Now;
        }

        public DateTime Resolve(DtoUnitKey source, DaoUnit destination, DateTime destMember, ResolutionContext context)
        {
            return DateTime.Now;
        }

        public DateTime Resolve(Installation source, DaoInstallation destination, DateTime destMember, ResolutionContext context)
        {
            return DateTime.Now;
        }

        public DateTime Resolve(DtoModelInfo source, DaoModel destination, DateTime destMember, ResolutionContext context)
        {
            return DateTime.Now;
        }

        public DateTime Resolve(DtoPrediction source, DaoPrediction destination, DateTime destMember, ResolutionContext context)
        {
            return DateTime.Now;
        }

        public DateTime Resolve(DtoTimeSerie source, DaoTimeSerie destination, DateTime destMember, ResolutionContext context)
        {
            return DateTime.Now;
        }
    }
}
