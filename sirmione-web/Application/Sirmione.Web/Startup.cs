using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.ApplicationInsights.SnapshotCollector;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.IdentityModel.Logging;

namespace Sirmione.Web
{

    internal class MyTelemetryInitializer : ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            var requestTelemetry = telemetry as RequestTelemetry;
            // Is this a TrackRequest() ?
            if (requestTelemetry == null) return;

            if (string.IsNullOrEmpty(telemetry.Context.User.Id) || string.IsNullOrEmpty(telemetry.Context.Session.Id))
            {
                // Set the user id on the Application Insights telemetry item.
                //telemetry.Context.User.Id = "NickiWiki";

                // Set the session id on the Application Insights telemetry item.
                //telemetry.Context.Session.Id = "NickiWiki";
            }
        }
    }
    internal class MyTelemetryProcessor : ITelemetryProcessor
    {
        private ITelemetryProcessor _next;
        private IHttpContextAccessor _httpContextAccessor;

        public MyTelemetryProcessor(ITelemetryProcessor next, IHttpContextAccessor httpContextAccessor)
        {
            _next = next;
            _httpContextAccessor = httpContextAccessor;
        }

        public void Process(ITelemetry item)
        {
            HttpContext context = _httpContextAccessor.HttpContext;
            if (context != null)
            {
                if (context.User.Identity.IsAuthenticated)
                {
                    item.Context.User.AuthenticatedUserId = context.User.Identity.Name;
                }
            }
            _next.Process(item);
        }
    }
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;
            services.AddAuthentication(AzureADDefaults.AuthenticationScheme).AddAzureAD(options => Configuration.Bind("AzureAd", options));
            services.AddControllersWithViews();
            var appInsightsKey = Configuration["ApplicationInsights:InstrumentationKey"];
            var aiOptions = new Microsoft.ApplicationInsights.AspNetCore.Extensions.ApplicationInsightsServiceOptions();
            aiOptions.EnableAdaptiveSampling = false;
            aiOptions.InstrumentationKey = appInsightsKey;
            services.AddHttpContextAccessor();
            var accessor = (IHttpContextAccessor)services.Last(s => s.ServiceType == typeof(IHttpContextAccessor)).ImplementationInstance;
            services.AddApplicationInsightsTelemetry(aiOptions);
            services.AddSingleton<ITelemetryInitializer, MyTelemetryInitializer>();
            services.AddSnapshotCollector((configuration) => Configuration.Bind(nameof(SnapshotCollectorConfiguration), configuration));
            var configDescriptor = services.SingleOrDefault(tc => tc.ServiceType == typeof(TelemetryConfiguration));
            if (configDescriptor?.ImplementationFactory != null)
            {
                var implFactory = configDescriptor.ImplementationFactory;
                services.Remove(configDescriptor);

                services.AddSingleton(provider =>
                {
                    if (implFactory.Invoke(provider) is TelemetryConfiguration config)
                    {
                        var newConfig = config;
                        newConfig.ApplicationIdProvider = config.ApplicationIdProvider;
                        newConfig.InstrumentationKey = config.InstrumentationKey;
                        newConfig.TelemetryProcessorChainBuilder.Use(next => new MyTelemetryProcessor(next, provider.GetRequiredService<IHttpContextAccessor>()));
                        foreach (var processor in config.TelemetryProcessors)
                        {
                            newConfig.TelemetryProcessorChainBuilder.Use(next => processor);
                        }

                        newConfig.TelemetryProcessorChainBuilder.Build();
                        newConfig.TelemetryProcessors.OfType<ITelemetryModule>().ToList().ForEach(module => module.Initialize(newConfig));
                        return newConfig;
                    }
                    return null;
                });

            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });




        }
    }
}

