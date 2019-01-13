using System;
using ForecastMonitor.Shared;
using ForecastMonitor.Test.UI.TestUtils;
using ForecastMonitor.Test.UI.TestUtils.SeleniumWebDriver;
using NUnit.Framework;
using OpenQA.Selenium.Remote;
using WireMock.Server;
using WireMock.Settings;

namespace ForecastMonitor.Test.UI
{
    /// <summary>
    /// All UI tests should inherit from this base
    /// </summary>
    [TestFixture, NonParallelizable]
    [TestProgressLogging]
    public abstract class BaseSeleniumTest
    {
        private const string UIProjectDirectoryName = "ForecastMonitor.UI";
        private const string StartAngularServerCommand = "npm run start-test";

        private const int AngularServerDeploymentTimeInMs = 10000;
        private const int AngularServerPortNumber = 4200;
        private const int ForecastMonitorApiPortNumber = 50000;
        private const int WebDriverImplicitWaitInSec = 5;

        private static string ChangeDirectoryToUIProjectCommand => $"/C cd {TestHelper.FindFolder(UIProjectDirectoryName).FullName}";

        protected abstract BrowserKind BrowserKind { get; }
        protected string UIBaseUrl => $"http://localhost:{AngularServerPortNumber}";
        protected FluentMockServer ForecastMonitorServiceStub { get; private set; }
        protected RemoteWebDriver Driver { get; private set; }
        
        [OneTimeSetUp]
        public void BaseOneTimeSetUp()
        {
            // Set Console to write into Test Output 
            Console.SetOut(TestContext.Progress);

            // start angular server and wait until it has started
            ProcessManager.ExecuteCommand(ChangeDirectoryToUIProjectCommand, StartAngularServerCommand);
            System.Threading.Thread.Sleep(AngularServerDeploymentTimeInMs);

            // set driver
            Driver = WebDriverFactory.CreateWebDriver(BrowserKind);
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(WebDriverImplicitWaitInSec);
            Driver.Manage().Window.Maximize();
        }

        [OneTimeTearDown]
        public void BseOneTimeTeardown()
        {
            // kill angular server process
            ProcessManager.KillProcessByPort(AngularServerPortNumber);

            Driver.Quit();
            Console.WriteLine(@"Driver closed");
            Console.SetOut(new System.IO.StreamWriter(Console.OpenStandardOutput()) {AutoFlush = true});
        }

        [SetUp]
        public void BaseSetUp()
        {
            ForecastMonitorServiceStub = FluentMockServer.Start(new FluentMockServerSettings
            {
                Port = ForecastMonitorApiPortNumber,
                AllowPartialMapping = true
            });
        }

        [TearDown]
        public void BaseTearDown()
        {
            ForecastMonitorServiceStub.Dispose();
        }
    }
}
