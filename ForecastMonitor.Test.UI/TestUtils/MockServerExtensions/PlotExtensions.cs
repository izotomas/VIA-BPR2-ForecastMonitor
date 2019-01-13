using System;
using System.Collections.Generic;
using System.Net;
using ForecastMonitor.Shared;
using HandlebarsDotNet;
using Newtonsoft.Json;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace ForecastMonitor.Test.UI.TestUtils.MockServerExtensions
{
    public partial class MockServerExtensions
    {
        public static FluentMockServer AsModelPerformance(this FluentMockServer server, string pathToPlots = DefaultPlotsJson)
        {
            server.ConfigurePlots(pathToPlots);

            return server;
        }
        
        public static FluentMockServer ConfigurePlots(this FluentMockServer server, string plotsJsonPath = DefaultPlotsJson)
        {
            var json = TestHelper.ReadJson($"{PathForForecastMonitorTestData}{plotsJsonPath}", ProjectFolder);

            server.Given(Request.Create()
                    .WithPath("/unit/plot")
                    .WithParam("installationId")
                    .WithParam("unitId")
                    .WithParam("weeksAgo")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithHeader("Content-type", "application/json;charset=utf-8")
                    .WithHeader("Access-Control-Allow-Origin", "*")
                    .WithHeader("Access-Control-Allow-Methods", "*")
                    .WithBody(json));

            return server;
        }
    }
}
