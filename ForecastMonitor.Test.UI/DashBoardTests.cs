using System;
using System.Linq;
using FluentAssertions;
using ForecastMonitor.Test.UI.TestUtils.SeleniumWebDriver;
using ForecastMonitor.Test.UI.TestUtils.MockServerExtensions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace ForecastMonitor.Test.UI
{
    [TestFixture]
    public class DashBoardTests : BaseSeleniumTest
    {
        protected override BrowserKind BrowserKind => BrowserKind.Chrome;

        [Test]
        public void Long_Unit_Name_Has_Ellipsis_And_A_Full_Length_Tooltip()
        {
            // Arrange
            ForecastMonitorServiceStub.AsDashboard(pathToUnits:"units/long_unit_name.json");
            var expectedTooltipText = "Long Unit Name That Should Be Out Of Range";
            var expectedEllipsisInElement = "...";

            // Act
            Driver.Navigate().GoToUrl(UIBaseUrl);

            var element = Driver.FindElementByClassName("installation")
                .FindElement(By.Id("installation-content-1"))
                .FindElement(By.Id("client-content-1"));

            element = element.FindElement(By.ClassName("card-column"))
                .FindElement(By.ClassName("model-name"));

            var actualElementText = element.Text;

            element.Click();

            element = Driver.FindElementByClassName("mat-tooltip");
            var actualTooltipText = element.Text;

            // Assert
            actualElementText.Should().NotBeNullOrEmpty();
            actualElementText.Should().Contain(expectedEllipsisInElement);
            actualElementText.Length.Should().BeLessThan(actualTooltipText.Length);
            actualTooltipText.Should().BeEquivalentTo(expectedTooltipText);
        }

        [Test]
        public void Unknown_Unit_Name_Uses_Unit_Id()
        {
            // Arrange
            ForecastMonitorServiceStub.AsDashboard(pathToUnits:"units/null_unit_name.json");
            var expectedPartOfElementText = "Unit not found";
            var expectedTooltipText = "Unit not found: 410";

            // Act
            Driver.Navigate().GoToUrl(UIBaseUrl);

            var element = Driver.FindElementByClassName("installation")
                .FindElement(By.Id("installation-content-1"))
                .FindElement(By.Id("client-content-1"));

            element = element.FindElement(By.ClassName("card-column"))
                .FindElement(By.ClassName("model-name"));

            var actualElementText = element.Text;

            element.Click();

            element = Driver.FindElementByClassName("mat-tooltip");
            var actualTooltipText = element.Text;

            // Assert
            actualElementText.Should().Contain(expectedPartOfElementText);
            actualTooltipText.Should().BeEquivalentTo(expectedTooltipText);

        }

        [Test]
        public void Error_Dialog_Shown_In_Dashboard_On_Connection_Error()
        {
            // Arrange
            ForecastMonitorServiceStub
                .Given(Request.Create().WithPath("/installations").UsingGet())
                .RespondWith(Response.Create().WithNotFound());

            // Act
            Driver.Navigate().GoToUrl(UIBaseUrl);
            Action errorElementLookup = () =>
            {
                Driver.FindElementByClassName("error-modal");
            };

            // Assert
            errorElementLookup.Should().NotThrow<NoSuchElementException>();
        }

        [Test, Description("PP5-2")]
        [TestCase("grey_only", "secondary")]
        [TestCase("green_grey", "success")]
        [TestCase("grey_red", "danger")]
        [TestCase("yellow_green", "warning")]
        public void Client_Performance_Indicator_Is_As_Its_Worse_Unit(string unitsJson, string expectedClientBadge)
        {
            // Arrange
            ForecastMonitorServiceStub.AsDashboard(pathToUnits:$"units/pp5_2/{unitsJson}.json");
            var expectedBadge = $"badge-{expectedClientBadge}";

            // Act
            Driver.Navigate().GoToUrl(UIBaseUrl);
            var clientElement = Driver.FindElementByClassName("client");
            var performanceElement = clientElement.FindElement(By.TagName("app-performance-indicator"));
            Action badgeLookup = () =>
            {
                performanceElement.FindElement(By.ClassName(expectedBadge));
            };

            // Assert
            badgeLookup.Should().NotThrow<NoSuchElementException>();
        }

        [Test, Description("PP5-3")]
        [TestCase("grey_only", "secondary")]
        [TestCase("green_grey", "success")]
        [TestCase("grey_red", "danger")]
        [TestCase("yellow_green", "warning")]
        public void Installation_Performance_Indicator_Is_As_Its_Worse_Unit(string unitsJson, string expectedInstallationBadge)
        {
            // Arrange
            ForecastMonitorServiceStub
                .AsDashboard(pathToClients: "clients/two_clients.json", pathToUnits: $"units/pp5_3/{unitsJson}.json");

            var expectedBadge = $"badge-{expectedInstallationBadge}";

            // Act
            Driver.Navigate().GoToUrl(UIBaseUrl);

            var clientElement = Driver.FindElementByClassName("installation");
            var performanceElement = clientElement.FindElement(By.TagName("app-performance-indicator"));
            Action badgeLookup = () =>
            {
                performanceElement.FindElement(By.ClassName(expectedBadge));
            };

            // Assert
            badgeLookup.Should().NotThrow<NoSuchElementException>();
        }

        [Test, Description("PP6-1")]
        [TestCase("green", false)]
        [TestCase("grey", false)]
        [TestCase("yellow", false)]
        [TestCase("red", true)]
        public void Poor_Performing_Clients_Unfold_Automatically(string unitJson, bool shouldBeUnfolded)
        {
            // Arrange
            ForecastMonitorServiceStub.AsDashboard(pathToUnits: $"units/{unitJson}.json");

            // Act
            Driver.Navigate().GoToUrl(UIBaseUrl);

            var element = Driver.FindElementByClassName("installation")
                .FindElement(By.Id("installation-content-1"))
                .FindElement(By.Id("client-content-1"));

            element = element.FindElement(By.ClassName("card-column"))
                    .FindElement(By.ClassName("model-name"));

            var isUnfolded = element.Displayed;

            // Assert
            isUnfolded.Should().Be(shouldBeUnfolded);
        }

        [Test, Description("PP4-1")]
        public void Units_Clients_And_Installations_Are_Ordered_By_Worse_Performance()
        {
            // Arrange
            ForecastMonitorServiceStub
                .AsDashboard(
                    pathToInstallations: "installations/two_installations.json",
                    pathToClients: "clients/pp4_1.json", 
                    pathToUnits: "units/pp4_1.json"
                    );

            var expectedInstallationOrder = new[] {"Installation 2", "Installation 1"};
            var expectedUnitOrder = new []{"Red Unit", "Yellow Unit", "Green Unit", "Grey Unit"};
            var expectedClientOrder = new[] {"Client Two", "Client Three", "Client One"};

            // Act
            Driver.Navigate().GoToUrl(UIBaseUrl);

            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));
            wait.Until(driver =>
                driver.FindElement(By.ClassName("installation"))
                    .FindElement(By.ClassName("installation-name")).Text == "Installation 2"
            );

            var installationElements = Driver.FindElements(By.ClassName("installation"));

            var actualInstallationOrder = installationElements
                .Select(_ => _.FindElement(By.ClassName("installation-name")).Text);

            Action worseClientLookup = () => Driver.FindElementByClassName("installation")
                .FindElement(By.ClassName("client"))
                .FindElement(By.Id("client-content-2"));

            var clientElements = Driver.FindElementByClassName("installation")
                .FindElements(By.ClassName("client-name"));

            var actualClientOrder = clientElements
                .Select(_ => _.Text)
                .ToList();

            var unitElements = Driver.FindElementByClassName("installation")
                .FindElement(By.Id("client-content-2"))
                .FindElements(By.TagName("app-model"))
                .ToList();

            var actualUnitOrder = unitElements
                .Select(_ => _.FindElement(By.ClassName("model-name")).Text)
                .ToList();

            // Assert
            worseClientLookup.Should().NotThrow<NoSuchElementException>();
            actualInstallationOrder.Should().BeEquivalentTo(expectedInstallationOrder, options => options.WithStrictOrdering());
            actualClientOrder.Should().BeEquivalentTo(expectedClientOrder, options => options.WithStrictOrdering());
            actualUnitOrder.Should().BeEquivalentTo(expectedUnitOrder, options => options.WithStrictOrdering());
        }

        [Test]
        public void Unit_MAE_Display_Test()
        {
            // Arrange
            var expectedMAE = double.Parse("0.0");
            ForecastMonitorServiceStub.AsDashboard(pathToUnits:"units/red_0_mae.json");

            // Act
            Driver.Navigate().GoToUrl(UIBaseUrl);
            var element = Driver.FindElementByClassName("installation")
                .FindElement(By.Id("installation-content-1"))
                .FindElement(By.Id("client-content-1"))
                .FindElement(By.ClassName("mae-value"));

            var actualMae = double.Parse(element.Text);

            // Assert
            actualMae.Should().Be(expectedMAE);
        }
    }
}
