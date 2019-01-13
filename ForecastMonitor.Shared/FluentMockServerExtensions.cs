using System.Net;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace ForecastMonitor.Shared
{
    public static partial class FluentMockServerExtensions
    {
        
        private const string ClientsUrl = "/client/all";
        private const string ModelsUrl = "/model_infos";
        private const string ModelUrl = "/model_info/";
        private const string PredictionsUrl = "/predictions";
        private const string TimeSeriesUrl = "/timeseries";
        private const string UnitsUrl = "/unit_keys/all";   

        private const string PathForForecastSystemTestData = "TestData/ForecastSystem/";

        public static FluentMockServer ConfigureClientWithFixedData(this FluentMockServer server)
        {
            var clients = TestHelper.ReadJson($"{PathForForecastSystemTestData}clients.json");
            var models = TestHelper.ReadJson($"{PathForForecastSystemTestData}models.json");
            var model = TestHelper.ReadJson($"{PathForForecastSystemTestData}model.json");
            var units = TestHelper.ReadJson($"{PathForForecastSystemTestData}unit_keys.json");
            var timeSeries = TestHelper.ReadJson($"{PathForForecastSystemTestData}time_series.json");
            var predictions = TestHelper.ReadJson($"{PathForForecastSystemTestData}predictions.json");

            server.SetupGetRequest(ClientsUrl, clients);
            server.SetupGetRequest(ModelsUrl, models);
            server.SetupGetRequest(ModelUrl, model);
            server.SetupGetRequest(UnitsUrl, units);
            server.SetupGetRequest(TimeSeriesUrl, timeSeries);
            server.SetupGetRequest(PredictionsUrl, predictions);

            return server;
        }

        /// <summary>
        /// A stateful server responding with data after pre-configured amount of tries
        /// </summary>
        /// <param name="server"></param>
        /// <param name="responsiveAfter">Number of tries after server responds. If 0, server remains unresponsive</param>
        /// <returns></returns>
        public static FluentMockServer ConfigureClientUnresponsiveWithFixedData(this FluentMockServer server, int responsiveAfter = 0)
        {
            var clients = TestHelper.ReadJson($"{PathForForecastSystemTestData}clients.json");
            var models = TestHelper.ReadJson($"{PathForForecastSystemTestData}models.json");
            var model = TestHelper.ReadJson($"{PathForForecastSystemTestData}model.json");
            var units = TestHelper.ReadJson($"{PathForForecastSystemTestData}unit_keys.json");
            var timeSeries = TestHelper.ReadJson($"{PathForForecastSystemTestData}time_series.json");
            var predictions = TestHelper.ReadJson($"{PathForForecastSystemTestData}predictions.json");


            server.SetupUnresponsiveGetRequest(ClientsUrl, clients, responsiveAfter);
            server.SetupUnresponsiveGetRequest(ModelsUrl, models, responsiveAfter);
            server.SetupUnresponsiveGetRequest(ModelUrl, model, responsiveAfter);
            server.SetupUnresponsiveGetRequest(UnitsUrl, units, responsiveAfter);
            server.SetupUnresponsiveGetRequest(TimeSeriesUrl, timeSeries, responsiveAfter);
            server.SetupUnresponsiveGetRequest(PredictionsUrl, predictions, responsiveAfter);

            return server;
        }

        public static FluentMockServer ConfigurePredictionsRequest(this FluentMockServer server, string predictionsResponse = null)
        {
            if (predictionsResponse == null)
            {
                predictionsResponse = TestHelper.ReadJson($"{PathForForecastSystemTestData}predictions.json");
            }
            server.SetupGetRequest(PredictionsUrl, predictionsResponse);
            return server;
        }

        public static FluentMockServer ConfigureTimeSeriesRequest(this FluentMockServer server, string timeSeriesResponse = null)
        {
            if (timeSeriesResponse == null)
            {
                timeSeriesResponse = TestHelper.ReadJson($"{PathForForecastSystemTestData}time_series.json");
            }
            server.SetupGetRequest(TimeSeriesUrl, timeSeriesResponse);
            return server;
        }

        private static void SetupUnresponsiveGetRequest(this FluentMockServer server, string path, string responseBody, int responsiveAfter)
        {
            var unresponsive = "Unresponsive";
            var request = Request.Create().WithPath($"{path}*").UsingGet();
            var requestTimeoutResponse = Response.Create().WithStatusCode(HttpStatusCode.RequestTimeout);
            var okResponse = Response.Create()
                .WithStatusCode(HttpStatusCode.OK)
                .WithHeader("Content-type", "application/json")
                .WithBody(responseBody);

            if (responsiveAfter == 0)
            {
                server.Given(request).RespondWith(requestTimeoutResponse);
            }
            else
            {
                server.Given(request)
                    .InScenario(unresponsive)
                    .WillSetStateTo(0.ToString())
                    .RespondWith(requestTimeoutResponse);

                for (var i = 0; i < responsiveAfter; i++)
                {
                    server.Given(request)
                        .InScenario(unresponsive)
                        .WhenStateIs(i.ToString())
                        .WillSetStateTo((i + 1).ToString())
                        .RespondWith(requestTimeoutResponse);
                }

                server.Given(request)
                    .InScenario(unresponsive)
                    .WhenStateIs(responsiveAfter.ToString())
                    .RespondWith(okResponse);
            }
        }

        private static void SetupGetRequest(this FluentMockServer server, string path, string responseBody, HttpStatusCode statusCode = HttpStatusCode.OK, params string[] parameters)
        {
            var thePath = parameters == null || parameters.Length == 0
                ? $"{path}*"
                : path;

            var request = Request.Create()
                .WithPath(thePath)
                .UsingGet();

            var response = Response.Create()
                .WithStatusCode(statusCode)
                .WithHeader("Content-type", "application/json")
                .WithHeader("Access-Control-Allow-Origin", "*")
                .WithHeader("Access-Control-Allow-Methods", "*")
                .WithBody(responseBody);

            foreach (var parameter in parameters)
            {
                request.WithParam(parameter);
            }

            server.Given(request).RespondWith(response);
        }
    }
}
