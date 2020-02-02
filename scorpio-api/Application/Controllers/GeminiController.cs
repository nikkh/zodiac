using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace scorpio_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeminiController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly TelemetryClient _telemetryClient;

        public GeminiController(ILogger<GeminiController> logger, TelemetryClient telemetryClient)
        {
            _logger = logger;
            _telemetryClient = telemetryClient;
        }

         
         /// <summary>
         /// This api throws an exception
         /// </summary>
       
        [HttpGet]
        public ActionResult<string> Get(string traceGuid)
        {
            const string controllerName = "Gemini";

            var metricName = $"{controllerName}Transactions";
            var message = $"{controllerName} has been invoked. TraceGuid={traceGuid}";
            _telemetryClient.TrackEvent(message);
            _telemetryClient.GetMetric(metricName).TrackValue(1);
            _logger.LogInformation(message);
            try
            {
                throw new Exception("Exception thrown intentionally");
            }
            catch (Exception e)
            {
                _telemetryClient.TrackException(e);
                throw;
            }
        }

       

    }
}
