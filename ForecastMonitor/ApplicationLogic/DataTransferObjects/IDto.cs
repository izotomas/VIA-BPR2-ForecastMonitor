using System;

namespace ForecastMonitor.Service.ApplicationLogic.DataTransferObjects
{
    public interface IDto
    {
        DateTime LastUpdate { get; set; }
        int Id { get; set; }
    }
}
