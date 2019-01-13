using System;
using System.IO;
using AutoFixture;
using AutoFixture.AutoMoq;
using ForecastMonitor.Service;
using ForecastMonitor.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace ForecastMonitor.Test.Integration
{
    [TestFixture]
    public abstract class BaseIntegrationTest
    {
        private const string Environment = "Test";
        private const string ForecastMonitorRootFolderName = "ForecastMonitor";
        private const string ForecastMonitorServiceFolderName = "ForecastMonitor";

        private readonly string _projectDirectory;
        private readonly Action<IServiceCollection> _testServicesConfiguration;

        protected TestServer TestServer { get; private set; }
        protected IFixture Fixture { get; private set; }

        protected BaseIntegrationTest(): this(services => { })
        {
        }

        protected BaseIntegrationTest(Action<IServiceCollection> testServicesConfiguration)
        {
            this._projectDirectory = Path.Combine(TestHelper.FindFolder(ForecastMonitorRootFolderName).FullName, ForecastMonitorServiceFolderName);
            this._testServicesConfiguration = testServicesConfiguration;
        }

        [OneTimeSetUp]
        public void BaseOneTimeSetup()
        {
            Fixture = new Fixture().Customize(new AutoMoqCustomization());

            TestServer = new TestServer(new WebHostBuilder()
                .ConfigureTestServices(this._testServicesConfiguration)
                .UseStartup<Startup>()
                .UseEnvironment(Environment)
                .UseContentRoot(_projectDirectory)
            );

        }

        [OneTimeTearDown]
        public void BaseOneTimeTearDown()
        {
            TestServer.Dispose();
        }
    }
}
