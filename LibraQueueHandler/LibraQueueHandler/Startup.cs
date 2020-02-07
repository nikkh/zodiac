using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(LibraQueueHandler.Startup))]
namespace LibraQueueHandler
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var appInsightsKey = "0220f791-8d97-401c-9916-164d4481cde9";
            var aiOptions = new Microsoft.ApplicationInsights.AspNetCore.Extensions.ApplicationInsightsServiceOptions();
            aiOptions.EnableAdaptiveSampling = false;
            aiOptions.InstrumentationKey = appInsightsKey;
            builder.Services.AddApplicationInsightsTelemetry(aiOptions);


        }
    }
}
