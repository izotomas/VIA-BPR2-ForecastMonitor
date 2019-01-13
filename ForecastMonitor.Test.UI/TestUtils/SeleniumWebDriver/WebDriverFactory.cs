using System;
using System.IO;
using ForecastMonitor.Shared;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace ForecastMonitor.Test.UI.TestUtils.SeleniumWebDriver
{
    /// <summary>
    /// Only Chrome is supported at the moment
    /// </summary>
    public enum BrowserKind
    {
        InternetExplorer,
        IE = InternetExplorer,
        Chrome
    }

    public static class WebDriverFactory
    {
        private const string TestRootDirectoryName = "ForecastMonitor.Test.UI";
        private static readonly DirectoryInfo WebDriverDirectory = TestHelper.FindFolder(TestRootDirectoryName);

        public static RemoteWebDriver CreateWebDriver(BrowserKind kind = BrowserKind.Chrome)
        {
            switch (kind)
            {
                case BrowserKind.Chrome:
                    return new ChromeDriver(WebDriverDirectory.FullName);
                default:
                    throw new NotImplementedException($"{kind.GetFullName()} is not supported yet");
            }
        }

        private static string GetFullName(this BrowserKind kind)
        {
            return string.Format("{0}.{1}", nameof(BrowserKind), kind.ToString());
        }
    }
}
