using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Zodiac.Generator.UI;

namespace Zodiac.Generator.UI
{
    public class UIControllerTimerTrigger
    {
        private readonly ZodiacContext _zodiacContext;

        public UIControllerTimerTrigger(IConfiguration config, ZodiacContext zodiacContext)
        {
            _zodiacContext = zodiacContext;
        }

        [FunctionName("UIControllerTimerTrigger")]
        public async Task Run([TimerTrigger("0 * * * * *")]TimerInfo myTimer, ILogger log, ExecutionContext ec)
        {
            try
            {
                log.LogInformation($"{ec.FunctionName} (timer trigger) function executed at: {DateTime.UtcNow}");
                var worker = new UIControllerWorker(_zodiacContext);
                await worker.Run(log, ec.FunctionName);
                return;
            }
            catch (Exception e)
            {
                log.LogError($"Exeception during execution of {ec.FunctionName}. Message: {e.Message}. Check Inner Exception", e);
            }

        }
       
    }
}
