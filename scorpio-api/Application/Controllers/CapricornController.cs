using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using scorpio_api.Utils;

namespace scorpio_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CapricornController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly TelemetryClient _telemetryClient;

        public CapricornController(ILogger<CapricornController> logger, TelemetryClient telemetryClient)
        {
            _logger = logger;
            _telemetryClient = telemetryClient;
        }

         
         /// <summary>
         /// This api returns a random list of posts in lorem ipsum, obtained form a third party web service
         /// </summary>
       
        [HttpGet]
        public async Task<ActionResult<string>> Get(string traceGuid)
        {
            const string controllerName = "Capricorn";

            var metricName = $"{controllerName}Transactions";
            var message = $"{controllerName} has been invoked. TraceGuid={traceGuid}";
            _telemetryClient.TrackEvent(message);
            _telemetryClient.GetMetric(metricName).TrackValue(1);
            _logger.LogInformation(message);
            string result = await RestApi.Call("http://sienna-api.uksouth.cloudapp.azure.com/", "api/votes");
            return result;

        }

       

    }
}
