using AutoMapper;
using ForecastMonitor.Service.AppSettings;
using ForecastMonitor.Service.DataAccessLogic.DataAccessObjects;
using ForecastMonitor.Service.DomainLogic.HttpClients.DataTransferObjects;
using ForecastMonitor.Service.DomainLogic.Mapping.ValueResolvers;

namespace ForecastMonitor.Service.DomainLogic.Mapping
{
    public class DtoToDaoMappingProfile : Profile
    {
        public DtoToDaoMappingProfile()
        {
            CreateMap<Installation, DaoInstallation>()
                .ForMember(dao => dao.TimeStamp, opts => opts.ResolveUsing<CurrentDateTimeValueResolver>());

            CreateMap<DtoClient, DaoClient>()
                .ForMember(dao => dao.TimeStamp, opts => opts.ResolveUsing<CurrentDateTimeValueResolver>())
                .ForMember(dao => dao.Name, opts => opts.MapFrom(dto => dto.Key));

            CreateMap<DtoUnitKey, DaoUnit>()
                .ForMember(dao => dao.TimeStamp, opts => opts.ResolveUsing<CurrentDateTimeValueResolver>())
                .ForMember(dao => dao.Key, opts => opts.MapFrom(dto => dto.UnitKey))
                .ForMember(dao => dao.ClientKey, opts => opts.MapFrom(dto => dto.Client.Key))
                .ForMember(dao => dao.Mae, opts => opts.UseValue(double.NaN))
                .ForMember(dao => dao.ZScore, opts => opts.UseValue(double.NaN));

            CreateMap<DtoModelInfo, DaoModel>()
                .ForMember(dao => dao.Id, opts => opts.MapFrom(dto => dto.ModelId))
                .ForMember(dao => dao.UnitId, opts => opts.MapFrom(dto => dto.UnitKeyId))
                .ForMember(dao => dao.TrainedOn, opts => opts.MapFrom(dto => dto.FinishedTrainingOn))
                .ForMember(dao => dao.Mae, opts => opts.UseValue(double.NaN))
                .ForMember(dao => dao.IsLatestEvaluable, opts => opts.UseValue(false))
                .ForMember(dao => dao.TimeStamp, opts => opts.ResolveUsing<CurrentDateTimeValueResolver>());

            CreateMap<DtoPrediction, DaoPrediction>()
                .ForMember(dao => dao.Id, opts => opts.MapFrom(dto => dto.PredictionId))
                .ForMember(dao => dao.UnitId, opts => opts.MapFrom(dto => dto.UnitKeyId))
                .ForMember(dao => dao.PredictionFor, opts => opts.MapFrom(dto => dto.PredictionForDate))
                .ForMember(dao => dao.TimeStamp, opts => opts.ResolveUsing<CurrentDateTimeValueResolver>());

            CreateMap<DtoTimeSerie, DaoTimeSerie>()
                .ForMember(dao => dao.UnitId, opts => opts.MapFrom(dto => dto.UnitKeyId))
                .ForMember(dao => dao.TimeStamp, opts => opts.ResolveUsing<CurrentDateTimeValueResolver>())
                .ForMember(dao => dao.Value, opts => opts.ResolveUsing<TimeSerieLabelValueResolver>());

            CreateMap<DaoUnit, DtoUnitKey>()
                .ForMember(dto => dto.UnitKey, opts => opts.MapFrom(dao => dao.Key))
                .ForMember(dto => dto.Client, opts => opts.ResolveUsing<UnitKeyClientValueResolver>());

            CreateMap<DaoClient, DtoClient>()
                .ForMember(dto => dto.Key, opts => opts.MapFrom(dao => dao.Name))
                .ForMember(dto => dto.AuxiliarySeriesStartDaysAgo, opts => opts.Ignore())
                .ForMember(dto => dto.TimeSeriesStartDaysAgo, opts => opts.Ignore());
        }
    }
}
