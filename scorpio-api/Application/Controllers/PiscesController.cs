using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using scorpio_api.Utils;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;
using System.IO;
using System.Text;
using Azure.Storage.Blobs.Models;
using Azure;

namespace scorpio_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PiscesController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly TelemetryClient _telemetryClient;
        private readonly IConfiguration _configuration;

        public PiscesController(ILogger<PiscesController> logger, TelemetryClient telemetryClient, IConfiguration configuration)
        {
            _logger = logger;
            _telemetryClient = telemetryClient;
            _configuration = configuration;
        }

         
         /// <summary>
         /// This api may or maynot use a lot of CPU... depends on the query string
         /// </summary>
       
        [HttpGet]
        public async Task<ActionResult<string>> Get(string traceGuid, string cpumax)
        {
            const string controllerName = "Pisces";

            var metricName = $"{controllerName}Transactions";
            var message = $"{controllerName} has been invoked. TraceGuid={traceGuid}";
            _telemetryClient.TrackEvent(message);
            _telemetryClient.GetMetric(metricName).TrackValue(1);
            _logger.LogInformation(message);
            if (cpumax.ToUpper() == "TRUE")
            {
                _logger.LogWarning($"CPU max is requested");
                long nthPrime = FindPrimeNumber(10000); //set higher value for more time
                return $"Pisces calculated the nthPrime(10000) and the result was {nthPrime}";

            }

            return $"There was nothing for Pisces to do";

        }

        private long FindPrimeNumber(int n)
        {
            int count = 0;
            long a = 2;
            while (count < n)
            {
                long b = 2;
                int prime = 1;// to check if found a prime
                while (b * b <= a)
                {
                    if (a % b == 0)
                    {
                        prime = 0;
                        break;
                    }
                    b++;
                }
                if (prime > 0)
                {
                    count++;
                }
                a++;
            }
            return (--a);
        }

    }
}
