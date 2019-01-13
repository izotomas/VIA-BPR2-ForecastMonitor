using AutoMapper;
using ForecastMonitor.Service.ApplicationLogic.DataTransferObjects;
using ForecastMonitor.Service.DataAccessLogic.DataAccessObjects;

namespace ForecastMonitor.Service.ApplicationLogic.Mapping.ValueResolvers
{

    public class UnitPerformanceValueResolver : IValueResolver<DaoUnit, DtoUnit, string>
    {
        private enum PerformanceIndicator
        {
            Grey,
            Green,
            Yellow,
            Red
        }

        public string Resolve(DaoUnit source, DtoUnit destination, string destMember, ResolutionContext context)
        {
            var zScore = source.ZScore;
            var result = PerformanceIndicator.Grey.ToString();
            if (!double.IsNaN(zScore))
            {
                if (zScore < 1.5)
                {
                    result = PerformanceIndicator.Green.ToString();
                }
                else if (zScore < 3)
                {
                    result = PerformanceIndicator.Yellow.ToString();
                }
                else
                {
                    result = PerformanceIndicator.Red.ToString();
                }
            }
            return result;
        }
    }
}
