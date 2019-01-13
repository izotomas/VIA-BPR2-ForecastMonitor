using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;

// todo: Navigate using TitaniumProxy
namespace ForecastMonitor.Test.UI.TestUtils.SeleniumWebDriver
{
    /*
    public static class WebDriverExtensions
    {
        private const int INTERRUPT_MS = 100;
        private const int TIMEOUT_S = 10;


        public static int NavigateTo(this IWebDriver driver, string targetUrl)
        {
            var responseCode = 0;
            SessionStateHandler responseHandler = delegate (Session targetSession)
            {
                if (targetSession.fullUrl == targetUrl)
                {
                    responseCode = targetSession.responseCode;
                }
            };

            FiddlerApplication.AfterSessionComplete += responseHandler;

            var endTime = DateTime.Now.Add(TimeSpan.FromSeconds(TIMEOUT_S));
            Console.WriteLine($@"Navigating to {targetUrl}");
            driver.Navigate().GoToUrl(targetUrl);
            while (responseCode == 0 && DateTime.Now < endTime)
            {
                System.Threading.Thread.Sleep(INTERRUPT_MS);
            }

            FiddlerApplication.AfterSessionComplete -= responseHandler;
            return responseCode;
        }

        public static int ClickNavigate(this IWebElement element)
        {
            var responseCode = 0;
            var targetUrl = string.Empty;
            SessionStateHandler responseHandler = delegate (Session targetSession)
            {
                // For the first session of the click, the URL should be the initial 
                // URL requested by the element click.
                if (string.IsNullOrEmpty(targetUrl))
                {
                    targetUrl = targetSession.fullUrl;
                }

                if (targetSession.oResponse["Content-Type"].Contains("text/html") &&
                    targetSession.fullUrl == targetUrl &&
                    responseCode == 0)
                {
                    // If the response code is a redirect, get the URL of the
                    if (targetSession.responseCode >= 300 &&
                        targetSession.responseCode < 400)
                    {
                        // Use GetRedirectTargetURL rather than examining the "Location" header.
                        targetUrl = targetSession.GetRedirectTargetURL();
                    }
                    else
                    {
                        responseCode = targetSession.responseCode;
                    }
                }
            };

            // Using the ResponseHeadersAvailable event to avoid a race condition 
            FiddlerApplication.ResponseHeadersAvailable += responseHandler;

            var endTime = DateTime.Now.Add(TimeSpan.FromSeconds(TIMEOUT_S));
            element.Click();
            while (responseCode == 0 && DateTime.Now < endTime)
            {
                System.Threading.Thread.Sleep(100);
            }

            FiddlerApplication.ResponseHeadersAvailable -= responseHandler;
            return responseCode;
        }
    }
    */
}
