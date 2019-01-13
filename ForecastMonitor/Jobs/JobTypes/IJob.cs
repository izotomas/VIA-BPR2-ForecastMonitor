using System.Threading;
using System.Threading.Tasks;

namespace ForecastMonitor.Service.Jobs.JobTypes
{
    public interface IJob
    {
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
