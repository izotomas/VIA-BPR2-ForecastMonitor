namespace ForecastMonitor.Service.Jobs.JobTypes.Scheduled
{
    public interface IScheduledJob : IJob
    {
        string Schedule { get; }
    }
}
