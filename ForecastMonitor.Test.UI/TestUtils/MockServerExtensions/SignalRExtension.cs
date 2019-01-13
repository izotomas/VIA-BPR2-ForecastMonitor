using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using ForecastMonitor.Shared;
using HandlebarsDotNet;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace ForecastMonitor.Test.UI.TestUtils.MockServerExtensions
{
    public static partial class MockServerExtensions
    {
        private const string AngularLocalhostServerUrl = "http://localhost:4200";

        public static FluentMockServer SetupSignalRSubscription(this FluentMockServer server, string hub, string origin = AngularLocalhostServerUrl)
        {
            Handlebars.RegisterHelper("websocket-accept", (output, context, arguments) =>
            {
                var args = (Dictionary<string, WireMock.Util.WireMockList<string>>) arguments[0];
                var key = args["Sec-WebSocket-Key"].FirstOrDefault();
                var value = GetWebSocketAcceptHeaderValue(key);
                output.Write(value);
            });

            server.Given(Request.Create().WithUrl($"{hub}/negotiate").UsingMethod("OPTIONS"))
                .RespondWith(
                    Response.Create()
                        .WithHeader("Cache-Control", "no-store,no-cache")
                        .WithHeader("Vary", "Origin")
                        .WithStatusCode(HttpStatusCode.NoContent)
                        .WithHeader("Access-Control-Allow-Origin", origin)
                        .WithHeader("Access-Control-Allow-Credentials", "true")
                        .WithHeader("Access-Control-Allow-Headers", "X-Requested-With")
                );

            server.Given(Request.Create().WithUrl($"{hub}/negotiate").UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(HttpStatusCode.OK)
                        .WithHeader("Cache-Control", "no-store,no-cache")
                        .WithHeader("Vary", "Origin")
                        .WithHeader("Content-type", "application/json")
                        .WithHeader("Access-Control-Allow-Origin", origin)
                        .WithHeader("Access-Control-Allow-Credentials", "true")
                        .WithBody(TestHelper.ReadJson($"TestData/signalr.json", ProjectFolder))
                );

            server.Given(Request.Create().WithPath($"{hub}").UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(HttpStatusCode.SwitchingProtocols)
                        .WithHeader("Access-Control-Allow-Origin", origin)
                        .WithHeader("Cache-Control", "no-store,no-cache")
                        .WithHeader("Access-Control-Allow-Credentials", "true")
                        .WithHeader("Connection", "Upgrade")
                        .WithHeader("Upgrade", "websocket")
                        .WithHeader("Sec-WebSocket-Accept", "{{websocket-accept request.Headers}}")
                        .WithTransformer()
                        .WithBody("{}")
                );

            return server;
        }

        private static string GetWebSocketAcceptHeaderValue(string key)
        {
            var magic = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
            var sha1 = new SHA1Managed();
            var data = System.Text.Encoding.UTF8.GetBytes(key + magic);
            sha1.ComputeHash(data);
            var value = Convert.ToBase64String(sha1.Hash);
            return value;
        }
    }
}