using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ForecastMonitor.Service.Api
{
    public abstract class BaseController : ControllerBase
    {
        protected readonly ILogger Logger;

        protected BaseController(ILoggerFactory loggerFactory)
        {
            this.Logger = loggerFactory.CreateLogger(GetType().FullName);
        }

        protected IEnumerable<TOut> HttpGet<TOut>(IEnumerable<TOut> result)
        {
            var response = result.ToList();
            if (!response.Any())
            {
                Response.StatusCode = (int) HttpStatusCode.NotFound;
            }
            return response;
        }

        protected TOut HttpGet<TOut>(TOut result)
        {
            if (result == null)
            {
                Response.StatusCode = (int) HttpStatusCode.NotFound;
            }
            return result;
        }
    }
}
