using System.Threading;
using System.Threading.Tasks;

namespace ForecastMonitor.Service.ApplicationLogic.Publishing.UnitPublishingService
{
    public interface IUnitPublishingService
    {
        Task PublishUnits(CancellationToken cancellationToken);
    }
}
