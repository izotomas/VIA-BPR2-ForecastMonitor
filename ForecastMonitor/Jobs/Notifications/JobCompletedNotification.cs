using System;
using MediatR;

namespace ForecastMonitor.Service.Jobs.Notifications
{
    public class JobCompletedNotification : INotification
    {
        public Type JobType { get; }

        public JobCompletedNotification(Type jobType)
        {
            JobType = jobType;
        }
    }
}
