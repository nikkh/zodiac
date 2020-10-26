using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace Zodiac.Generator
{
    public class ZodiacGeneratorTimerTrigger
    {
        private readonly ZodiacContext _zodiacContext;

        public ZodiacGeneratorTimerTrigger(IConfiguration config, ZodiacContext zodiacContext)
        {
            _zodiacContext = zodiacContext;
        }

        [FunctionName("ZodiacGeneratorTimerTrigger")]
        public async Task Run([TimerTrigger("0 */7 * * * *")]TimerInfo myTimer, ILogger log, ExecutionContext ec)
        {
            try
            {
                log.LogInformation($"{ec.FunctionName} (timer trigger) function executed at: {DateTime.UtcNow}");
                var worker = new ZodiacGeneratorWorker(_zodiacContext);
                await worker.Run(log, ec.FunctionName);
                return;
            }
            catch (Exception e)
            {
                log.LogError($"Exeception during execution of {ec.FunctionName}. Message: {e.Message}. Check Inner Exception", e);
                throw e;
            }

        }
       
    }
}
