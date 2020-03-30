using LibraQueueHandler.Models;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(LibraQueueHandler.Startup))]
namespace LibraQueueHandler
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var localRoot = Environment.GetEnvironmentVariable("AzureWebJobsScriptRoot");
            var azureRoot = $"{Environment.GetEnvironmentVariable("HOME")}/site/wwwroot";

            var actualRoot = localRoot ?? azureRoot;

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(actualRoot)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            IConfiguration configuration = configBuilder.Build();
            builder.Services.AddSingleton(configuration);
            builder.Services.AddSingleton(configuration.GetSection("MonitoringContext").Get<MonitoringContext>());

            var appInsightsKey = "0220f791-8d97-401c-9916-164d4481cde9";
            var aiOptions = new Microsoft.ApplicationInsights.AspNetCore.Extensions.ApplicationInsightsServiceOptions();
            aiOptions.EnableAdaptiveSampling = false;
            aiOptions.InstrumentationKey = appInsightsKey;
            builder.Services.AddApplicationInsightsTelemetry(aiOptions);


        }
    }
}
