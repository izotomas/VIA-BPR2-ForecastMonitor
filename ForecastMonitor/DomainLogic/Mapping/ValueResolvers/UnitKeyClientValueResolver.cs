using AutoMapper;
using ForecastMonitor.Service.DataAccessLogic.DataAccessObjects;
using ForecastMonitor.Service.DomainLogic.HttpClients.DataTransferObjects;

namespace ForecastMonitor.Service.DomainLogic.Mapping.ValueResolvers
{
    public class UnitKeyClientValueResolver : IValueResolver<DaoUnit, DtoUnitKey, DtoClient>
    {
        public DtoClient Resolve(DaoUnit source, DtoUnitKey destination, DtoClient destMember, ResolutionContext context)
        {
            return new DtoClient{Key = source.ClientKey};
        }
    }
}
