using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using ForecastMonitor.Service.ApplicationLogic.Publishing.UnitPublishingService;
using ForecastMonitor.Service.DomainLogic.ForecastSystemService;
using ForecastMonitor.Service.Jobs;
using ForecastMonitor.Service.Jobs.JobTypes.Scheduled;
using ForecastMonitor.Service.Jobs.Notifications;
using ForecastMonitor.Test.Integration.TestUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace ForecastMonitor.Test.Integration.Jobs.Notification
{

    [TestFixture]
    public class NotificationHandlerTests : BaseIntegrationTest
    {

        private static readonly Mock<INotificationHandler<JobCompletedNotification>> _sut = new Mock<INotificationHandler<JobCompletedNotification>>();

        private static readonly Mock<IForecastSystemService> ForecastSystemMock = new Mock<IForecastSystemService>();
        private static readonly Mock<IUnitPublishingService> UnitPublishingServiceMock = new Mock<IUnitPublishingService>();

        public NotificationHandlerTests() : base(services =>
            services.RemoveJobScheduler()
                .ReplaceServices(ForecastSystemMock.Object)
                .ReplaceService(UnitPublishingServiceMock.Object)
                .ReplaceService(_sut.Object)
        )
        {
        }

        [SetUp]
        public void SetUp()
        {
            _sut.ResetCalls();
        }

        [Test]
        public async Task Notifications_Are_Received()
        {
            // Arrange
            var cacheUpdateJob = TestServer.Host.Services.GetServices<IScheduledJob>()
                .First(x => x.GetType() == typeof(CacheUpdateJob));

            // Act
            await cacheUpdateJob.ExecuteAsync(CancellationToken.None);

            // Assert
            _sut.Verify(x => x.Handle(It.Is<JobCompletedNotification>(notification => notification.JobType == typeof(CacheUpdateJob)), It.IsAny<CancellationToken>()), Times.Once);
            _sut.Verify(x => x.Handle(It.Is<JobCompletedNotification>(notification => notification.JobType == typeof(PerformanceCalculationJob)), It.IsAny<CancellationToken>()), Times.Once);
            _sut.Verify(x => x.Handle(It.Is<JobCompletedNotification>(notification => notification.JobType == typeof(UnitsPublishJob)), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task Notifications_Are_In_Correct_Order()
        {
            // Arrange
            var cacheUpdateJob = TestServer.Host.Services.GetServices<IScheduledJob>()
                .First(x => x.GetType() == typeof(CacheUpdateJob));

            var expectedReturnOrder = new List<Type>
            {
                typeof(CacheUpdateJob),
                typeof(PerformanceCalculationJob),
                typeof(UnitsPublishJob)
            };

            var actualReturnOrder = new List<Type>();
            _sut.Setup(_ => _.Handle(It.IsAny<JobCompletedNotification>(), It.IsAny<CancellationToken>()))
                .Callback<JobCompletedNotification, CancellationToken>((notification, token) => actualReturnOrder.Add(notification.JobType))
                .Returns(Fixture.Create<Task>());

            // Act
            await cacheUpdateJob.ExecuteAsync(CancellationToken.None);

            // Assert
            _sut.Verify(x => x.Handle(It.IsAny<JobCompletedNotification>(), It.IsAny<CancellationToken>()), Times.Exactly(3));
            actualReturnOrder.Should().BeEquivalentTo(expectedReturnOrder, opts => opts.WithStrictOrdering());
        }
    }
}
