using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSwag.AspNetCore;
using System.Reflection;
using ForecastMonitor.Service.ApplicationLogic.DataLogic.ClientLogic;
using ForecastMonitor.Service.ApplicationLogic.DataLogic.InstallationLogic;
using ForecastMonitor.Service.ApplicationLogic.DataLogic.UnitLogic;
using ForecastMonitor.Service.ApplicationLogic.Publishing.Hubs;
using ForecastMonitor.Service.ApplicationLogic.Publishing.UnitPublishingService;
using ForecastMonitor.Service.Configuration.AppSettings.Manager;
using ForecastMonitor.Service.Configuration.Rewriter;
using ForecastMonitor.Service.Configuration.Swagger;
using ForecastMonitor.Service.DataAccessLogic.DataContext;
using ForecastMonitor.Service.DataAccessLogic.DataServices;
using ForecastMonitor.Service.DataAccessLogic.DataServices.ClientDataService;
using ForecastMonitor.Service.DataAccessLogic.DataServices.InstallationDataService;
using ForecastMonitor.Service.DataAccessLogic.DataServices.ModelDataService;
using ForecastMonitor.Service.DataAccessLogic.DataServices.PredictionDataService;
using ForecastMonitor.Service.DataAccessLogic.DataServices.TimeSerieDataService;
using ForecastMonitor.Service.DataAccessLogic.DataServices.UnitDataService;
using ForecastMonitor.Service.DomainLogic.ForecastSystemService;
using ForecastMonitor.Service.DomainLogic.HttpClients.ForecastSystemClient;
using ForecastMonitor.Service.DomainLogic.HttpClients.Policy;
using ForecastMonitor.Service.DomainLogic.Statistics.PerformanceCalculationLogic;
using ForecastMonitor.Service.Jobs;
using ForecastMonitor.Service.Jobs.JobTypes.AdHoc;
using ForecastMonitor.Service.Jobs.JobTypes.Scheduled;
using MediatR;
using Serilog;

namespace ForecastMonitor.Service
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();

            Log.Information($"Reading settings from file: appsettings.{env.EnvironmentName}.json");
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
                .AddApiExplorer()
                .AddCors()
                .AddJsonFormatters()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Configuration
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<IAppSettingsManager, AppSettingsManager>();

            // Jobs
            services.AddSingleton<IAdHocJob, PerformanceCalculationJob>();
            services.AddSingleton<IAdHocJob, UnitsPublishJob>();
            services.AddSingleton<IScheduledJob, CacheUpdateJob>();

            services.AddAdHocJobService();
            services.AddSchedulerJobService((sender, args) =>
            {
                Log.Error(args.Exception.Message);
                args.SetObserved();
            });

            // Mapping
            services.AddAutoMapper();

            // Business Logic
            services.AddScoped<IInstallationLogic, InstallationLogic>();
            services.AddScoped<IClientLogic, ClientLogic>();
            services.AddScoped<IUnitLogic, UnitLogic>();
            services.AddScoped<IUnitPublishingService, UnitPublishingService>();

            // DataAccessLogic
            services.AddMemoryCache();
            services.AddScoped<IDataService, DataService>();
            services.AddScoped<IDataContext, MemoryDataContext>();
            services.AddScoped<IInstallationDataService, InstallationDataService>();
            services.AddScoped<IClientDataService, ClientDataService>();
            services.AddScoped<IUnitDataService, UnitDataService>();
            services.AddScoped<IModelDataService, ModelDataService>();
            services.AddScoped<ITimeSerieDataService, TimeSerieDataService>();
            services.AddScoped<IPredictionDataService, PredictionDataService>();

            // DomainLogic
            services.AddScoped<IPerformanceCalculationLogic, PerformanceCalculationLogic>();
            services.AddHttpClient<IForecastSystemClient, ForecastSystemClient>()
                .AddPolicyHandler(HttpClientPolicy.RetryPolicy);
            services.AddForecastSystemServices(Configuration);

            // MediatR
            services.AddMediatR();

            // SignalR
            services.AddSignalR();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IMapper mapper)
        {
            mapper.ConfigurationProvider.AssertConfigurationIsValid();

            app.UseSwaggerUi(typeof(Startup).GetTypeInfo().Assembly, cfg => cfg.DefaultProfile());

            app.UseRewriter(RewriterConfiguration.RewriteOptions);

            app.UseCors(builder =>
            {
                builder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .AllowAnyOrigin();
            });

            app.UseWebSockets();
            
            app.UseSignalR(routes => routes.MapHub<UnitHub>("/hub/units"));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
