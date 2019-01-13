using System;
using System.Net;
using System.Net.Http;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;

namespace ForecastMonitor.Service.DomainLogic.HttpClients.Policy
{
    public static class HttpClientPolicy
    {
        public static int RetryCount => 6;
        public static IAsyncPolicy<HttpResponseMessage> RetryPolicy => GetRetryPolicy();

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
                .WaitAndRetryAsync(RetryCount, retryAttempt =>
                {
                    var jitter = new Random();
                    return TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                           + TimeSpan.FromMilliseconds(jitter.Next(0, 100));
                });
        }
    }
}
