using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.ApplicationInsights;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Diagnostics;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using LibraQueueHandler.Models;
using Newtonsoft.Json;

namespace LibraQueueHandler
{
    public class LibraQueueHandler
    {


        private readonly TelemetryClient telemetryClient;
        public LibraQueueHandler(TelemetryConfiguration configuration)
        {
            telemetryClient = new TelemetryClient(configuration);
        }

        [FunctionName("Libra-Receive-and-Process")]
        public void Run(
        [ServiceBusTrigger("libra-queue", Connection = "ServiceBusConnection")]
        Message message,
        ILogger _logger)
        {
            try
            {
                int second = System.DateTime.Now.Second;
               
                // telemetryClient.TrackEvent("LibraMessageReceived");
                string payload = System.Text.Encoding.UTF8.GetString(message.Body);
                var messageModel = JsonConvert.DeserializeObject<MessageModel>(payload);
                _logger.LogInformation($"TraceGuid={messageModel.TraceGuid}, MessageId={message.MessageId}");
                var activity = message.ExtractActivity();
                _logger.LogDebug($"activity.RootId={activity.RootId}, activity.ParentID={activity.ParentId}");
                using (var operation = telemetryClient.StartOperation<RequestTelemetry>(activity))
                {
                    try
                    {
                       // Metric test = telemetryClient.GetMetric("LibraSeconds");x
                       // test.TrackValue(second, "Seconds");
                        // dont do anything else with this message
                    }
                    catch (Exception ex)
                    {
                        telemetryClient.TrackException(ex);
                        operation.Telemetry.Success = false;
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                telemetryClient.TrackException(ex);
                throw;
            }
        }
    }
}
