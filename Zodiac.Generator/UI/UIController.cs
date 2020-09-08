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
    public class UIControllerHttp
    {

        private readonly ZodiacContext _zodiacContext;
        
        public UIControllerHttp(IConfiguration config, ZodiacContext zodiacContext)
        {
            _zodiacContext = zodiacContext;
        }

        [FunctionName("UIControllerHttp")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log, ExecutionContext ec)
        {
            log.LogInformation($"{ec.FunctionName} (http trigger) function executed at: {DateTime.UtcNow}");
            var worker = new UIWorker(_zodiacContext);
            var responseMessage = await worker.Run(log, ec.FunctionName);
            return new OkObjectResult(responseMessage);
        }
    }
}
