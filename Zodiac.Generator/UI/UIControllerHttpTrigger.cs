using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Zodiac.Generator.UI
{
    public class UIControllerHttpTrigger
    {

        private readonly ZodiacContext _zodiacContext;
       

        public UIControllerHttpTrigger(IConfiguration config, ZodiacContext zodiacContext)
        {
            _zodiacContext = zodiacContext;
            
        }

        [FunctionName("UIControllerHttpTrigger")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log, ExecutionContext ec)
        {
            log.LogInformation($"{ec.FunctionName} (http trigger) function executed at: {DateTime.UtcNow}");
            int numSimulations;
            var worker = new UIControllerWorker(_zodiacContext);
            try
            {
                if (Int32.TryParse(req.Query["NumberOfSimulations"], out int numRequests))
                {
                    numSimulations = await worker.Run(log, ec.FunctionName, numRequests);
                }
                else
                {
                    numSimulations = await worker.Run(log, ec.FunctionName);
                }
            }
            catch (Exception e)
            {
                log.LogError($"Exeception during execution of {ec.FunctionName}. Message: {e.Message}. Check Inner Exception", e);
                return new StatusCodeResult(500);
            }
            var responseMessage = $"{ec.FunctionName} performed {numSimulations} simulations";
            return new OkObjectResult(responseMessage);
        }
    }
}
