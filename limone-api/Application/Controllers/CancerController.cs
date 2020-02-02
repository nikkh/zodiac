using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using limone_api.Utils;

namespace limone_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CancerController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly TelemetryClient _telemetryClient;

        public CancerController(ILogger<CancerController> logger, TelemetryClient telemetryClient)
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
            const string controllerName = "Cancer";

            var metricName = $"{controllerName}Transactions";
            var message = $"{controllerName} has been invoked. TraceGuid={traceGuid}";
            _telemetryClient.TrackEvent(message);
            _telemetryClient.GetMetric(metricName).TrackValue(1);
            _logger.LogInformation(message);
            
            string result = await RestApi.Call("https://jsonplaceholder.typicode.com", "posts");
            return result;

        }

       

    }
}
