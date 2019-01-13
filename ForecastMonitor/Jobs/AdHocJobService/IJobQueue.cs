using System.Threading;
using System.Threading.Tasks;
using ForecastMonitor.Service.Jobs.JobTypes;
using ForecastMonitor.Service.Jobs.JobTypes.AdHoc;
using MediatR;

namespace ForecastMonitor.Service.Jobs.AdHocJobService
{
    public interface IJobQueue
    {
        void Enqueue(IAdHocJob job);
        Task<IAdHocJob> DequeAsync(CancellationToken cancellationToken);
    }
}
