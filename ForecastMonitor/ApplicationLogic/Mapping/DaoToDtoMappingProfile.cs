using System;
using System.Collections.Generic;
using AutoMapper;
using ForecastMonitor.Service.ApplicationLogic.DataTransferObjects;
using ForecastMonitor.Service.ApplicationLogic.Mapping.ValueResolvers;
using ForecastMonitor.Service.DataAccessLogic.DataAccessObjects;
using ForecastMonitor.Service.DomainLogic.DomainModel;
using DtoClient = ForecastMonitor.Service.ApplicationLogic.DataTransferObjects.DtoClient;

namespace ForecastMonitor.Service.ApplicationLogic.Mapping
{
    public class DaoToDtoMappingProfile : Profile
    {
        public DaoToDtoMappingProfile()
        {
            CreateMap<DaoInstallation, DtoInstallation>()
                .ForMember(dto => dto.LastUpdate, cfg => cfg.MapFrom(dao => dao.TimeStamp));
            CreateMap<DaoClient, DtoClient>()
                .ForMember(dto => dto.LastUpdate, cfg => cfg.MapFrom(dao => dao.TimeStamp));
            CreateMap<DaoUnit, DtoUnit>()
                .ForMember(dto => dto.Performance, cfg => cfg.ResolveUsing<UnitPerformanceValueResolver>())
                .ForMember(dto => dto.LastUpdate, cfg => cfg.MapFrom(dao => dao.TimeStamp));
        }
    }
}
