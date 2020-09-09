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
            var worker = new UIControllerWorker(_zodiacContext);
            var responseMessage = await worker.Run(log, ec.FunctionName, 1);
            return new OkObjectResult(responseMessage);
        }
    }
}
