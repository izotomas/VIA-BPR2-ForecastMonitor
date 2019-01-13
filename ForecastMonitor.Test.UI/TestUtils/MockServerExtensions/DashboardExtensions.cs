using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using ForecastMonitor.Shared;
using HandlebarsDotNet;
using Newtonsoft.Json;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace ForecastMonitor.Test.UI.TestUtils.MockServerExtensions
{
    public static partial class MockServerExtensions
    {
        private const string ProjectFolder = "ForecastMonitor.Test.UI";
        private const string PathForForecastMonitorTestData = "TestData/";

        private const string DefaultInstallationsJson = "default/installations.json";
        private const string DefaultClientsJson = "default/clients.json";
        private const string DefaultUnitsJson = "default/units.json";
        private const string DefaultPlotsJson = "default/plots.json";

        public static FluentMockServer AsDashboard(this FluentMockServer server,
            string pathToInstallations = DefaultInstallationsJson,
            string pathToClients = DefaultClientsJson,
            string pathToUnits = DefaultUnitsJson
        )
        {

            server.ConfigureInstallations(pathToInstallations);
            server.ConfigureClients(pathToClients);
            server.ConfigureUnits(pathToUnits);

            SetupSignalRSubscription(server, "/hub/units");

            return server;
        }

        public static FluentMockServer ConfigureInstallations(this FluentMockServer server,
            string installationJsonPath = DefaultInstallationsJson)
        {
            var body = TestHelper.ReadJson($"{PathForForecastMonitorTestData}{installationJsonPath}", ProjectFolder);

            server.Given(Request.Create()
                    .WithPath("/installations")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithHeader("Content-type", "application/json")
                    .WithHeader("Access-Control-Allow-Origin", "*")
                    .WithHeader("Access-Control-Allow-Methods", "*")
                    .WithBody(body));

            return server;
        }

        public static FluentMockServer ConfigureClients(this FluentMockServer server,
            string clientJsonPath = DefaultClientsJson)
        {
            var json = TestHelper.ReadJson($"{PathForForecastMonitorTestData}{clientJsonPath}", ProjectFolder);

            Handlebars.RegisterHelper("filter-clients", (output, context, arguments) =>
            {
                var obj = FilterJson(json, arguments);
                output.Write(obj);
            });

            server.Given(Request.Create()
                    .WithPath("/clients")
                    .WithParam("installationId")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithHeader("Content-type", "application/json")
                    .WithHeader("Access-Control-Allow-Origin", "*")
                    .WithHeader("Access-Control-Allow-Methods", "*")
                    .WithBody("{{{filter-clients request.query}}}")
                    .WithTransformer());

            return server;
        }

        public static FluentMockServer ConfigureUnits(this FluentMockServer server, string unitJsonPath = DefaultUnitsJson)
        {
            var json = TestHelper.ReadJson($"{PathForForecastMonitorTestData}{unitJsonPath}", ProjectFolder);

            Handlebars.RegisterHelper("filter-units", (output, context, arguments) =>
            {
                var obj = FilterJson(json, arguments);
                output.Write(obj);
            });

            server.Given(Request.Create()
                    .WithPath("/units")
                    .WithParam("installationId")
                    .WithParam("clientId")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithHeader("Content-type", "application/json;charset=utf-8")
                    .WithHeader("Access-Control-Allow-Origin", "*")
                    .WithHeader("Access-Control-Allow-Methods", "*")
                    .WithBody("{{{filter-units request.query}}}")
                    .WithTransformer());

            return server;
        }

        private static string QueryParamToJsonProperty(string queryParam)
        {
            var parts = Regex.Split(queryParam, @"(?<!^)(?=[A-Z])");
            parts = parts.Select(_ => _.ToLowerInvariant()).ToArray();
            var property = string.Join('_', parts);
            return property;
        }

        private static object FilterJson(string json, object[] arguments)
        {
            var result = json;
            var args = (Dictionary<string, WireMock.Util.WireMockList<string>>) arguments[0];

            foreach (var arg in args)
            {
                var key = QueryParamToJsonProperty(arg.Key);
                var val = arg.Value.ToString();
                result = TestHelper.FilterJson(result, key, val);
            }

            var obj = JsonConvert.DeserializeObject(result);
            return obj;
        }
    }
}
