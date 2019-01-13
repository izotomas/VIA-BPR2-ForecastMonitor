using System.Threading;
using FluentAssertions;
using ForecastMonitor.Test.UI.TestUtils.SeleniumWebDriver;
using ForecastMonitor.Test.UI.TestUtils.MockServerExtensions;
using NUnit.Framework;
using OpenQA.Selenium;

namespace ForecastMonitor.Test.UI
{
    [TestFixture]
    public class ModelPerformanceTests : BaseSeleniumTest
    {
        protected override BrowserKind BrowserKind => BrowserKind.Chrome;

        [Test, Description("Empty data")]
        public void Visualize_Graph_Empty_Data()
        {
            // Arrange
            ForecastMonitorServiceStub.AsModelPerformance(pathToPlots: "plots/empty_data.json");
            var expectedAlertText = "There is no data for this interval!";

            // Act
            Driver.Navigate().GoToUrl(UIBaseUrl + "/model/1/512");

            var element = Driver.FindElementByClassName("graph")
                .FindElement(By.Id("no-data-alert"));

            var actualElementText = element.Text;

            // Assert
            actualElementText.Should().NotBeNullOrEmpty();
            actualElementText.Should().BeEquivalentTo(expectedAlertText);
        }
    }
}
