using NSwag.AspNetCore;

namespace ForecastMonitor.Service.Configuration.Swagger
{
    public static class SwaggerExtensions
    {
        public static void DefaultProfile(this SwaggerUiSettings<NSwag.SwaggerGeneration.WebApi.WebApiToSwaggerGeneratorSettings> settings)
        {
            settings.SwaggerUiRoute = "/help";
            settings.DocExpansion = "list";     // show controller methods in list form

            settings.PostProcess = document =>
            {
                document.Info.Version = "v1";
                document.Info.Title = "Forecast Monitoring System API";
                document.Info.Description = "A ASP.NET Core web API to monitor the Forecast System";
                document.Info.Contact = new NSwag.SwaggerContact
                {
                    Name = "Team Mr. Fantastic",
                    Email = "tomas.izo@systematic.com"
                };
            };
        }
    }
}
