using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;


namespace Zodiac.Generator
{
    public class ZodiacGeneratorHttpTrigger
    {
        private readonly ZodiacContext _zodiacContext;
        public ZodiacGeneratorHttpTrigger(IConfiguration config, ZodiacContext zodiacContext)
        {
            _zodiacContext = zodiacContext;
        }

        [FunctionName("ZodiacGeneratorHttpTrigger")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get",  Route = null)] HttpRequest req,
             ILogger log, ExecutionContext ec)
        {
            log.LogInformation($"{ec.FunctionName} (http trigger) function executed at: {DateTime.UtcNow}");
            int requestsMade = 0;
            var worker = new ZodiacGeneratorWorker(_zodiacContext);
            try
            {
                if (Int32.TryParse(req.Query["NumberOfCalls"], out int numRequests))
                {
                    requestsMade = await worker.Run(log, ec.FunctionName, numRequests);
                }
                else
                {
                    requestsMade = await worker.Run(log, ec.FunctionName);
                }
            }
             catch (Exception e)
            {
                log.LogError($"Exeception during execution of {ec.FunctionName}. Message: {e.Message}. Check Inner Exception", e);
            }
            return new OkObjectResult($"ZodiacGenerator generated {requestsMade} requests");
        }
    }
}
