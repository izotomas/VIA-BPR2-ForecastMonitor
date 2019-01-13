namespace ForecastMonitor.Service.DomainLogic.DomainModel
{
    public class GraphDataPoint<TX, TY>
    {
        public TX X { get; set; }
        public TY Y { get; set; }
    }
}
