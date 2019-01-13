using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using ForecastMonitor.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace ForecastMonitor.Test.E2E
{
    [TestFixture]
    public class EndpointTests
    {
        private const string EnvironmentName = "Test";
        private WebApplicationFactory<Startup> _factory;
        private HttpClient _client;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            this._factory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(cfg => cfg.UseEnvironment(EnvironmentName));
            this._client = this._factory.CreateClient();

            // wait for update cache job to complete
            System.Threading.Thread.Sleep(3000);
        }

        [Test]
        [TestCase("/")]
        [TestCase("/help")]
        public async Task Help_Endpoint_Is_Available(string endpoint)
        {
            // Arrange
            var expectedContentType = "text/html; charset=utf-8";
                
            // Act
            var httpResponseMessage = await _client.GetAsync(endpoint);
            var actualContentType = httpResponseMessage.Content.Headers.ContentType?.ToString();
            Action successStatusCodeVerification = () => { httpResponseMessage.EnsureSuccessStatusCode(); };

            // Assert
            successStatusCodeVerification.Should().NotThrow();
            actualContentType.Should().NotBeNullOrEmpty();
            expectedContentType.Should().Be(actualContentType);
        }

        [Test]
        public async Task Get_Installations()
        {
            // Arrange
            var url = "/installations";

            // Act
            var httpResponseMessage = await _client.GetAsync(url);
            var actualResponse = await httpResponseMessage.Content.ReadAsStringAsync();
            Action successStatusCodeVerification = () => { httpResponseMessage.EnsureSuccessStatusCode(); };

            // Assert
            successStatusCodeVerification.Should().NotThrow();
            actualResponse.Should().NotBeNullOrEmpty();
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public async Task Get_Clients(int validInstallationId)
        {
            // Arrange
            var url = $"/clients?installationId={validInstallationId}";

            // Act
            var httpResponseMessage = await _client.GetAsync(url);
            var actualResponse = await httpResponseMessage.Content.ReadAsStringAsync();
            Action successStatusCodeVerification = () => { httpResponseMessage.EnsureSuccessStatusCode(); };

            // Assert
            successStatusCodeVerification.Should().NotThrow();
            actualResponse.Should().NotBeNullOrEmpty();
        }

        [Test]
        [TestCase(1, 2015)]
        [TestCase(1, 2016)]
        [TestCase(2, 2015)]
        [TestCase(2, 2016)]
        public async Task Get_Units(int validInstallationId, int validClientId)
        {
            // Arrange
            var url = $"/units?installationId={validInstallationId}&clientId={validClientId}";

            // Act
            var httpResponseMessage = await _client.GetAsync(url);
            var actualResponse = await httpResponseMessage.Content.ReadAsStringAsync();
            Action successStatusCodeVerification = () => { httpResponseMessage.EnsureSuccessStatusCode(); };

            // Assert
            successStatusCodeVerification.Should().NotThrow();
            actualResponse.Should().NotBeNullOrEmpty();
        }


        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _client.Dispose();
            _factory.Dispose();
        }
    }
}
