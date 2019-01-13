using System.Threading.Tasks;
using ForecastMonitor.Service.Jobs.JobTypes.AdHoc;

namespace ForecastMonitor.Service.Jobs.AdHocJobService
{
    public interface IAdHocJobService
    {
        Task AddJob(IAdHocJob job);
    }
}
