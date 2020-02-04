using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.ApplicationInsights;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Diagnostics;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Zodiac.Models;
using Newtonsoft.Json;

namespace LibraQueueHandler
{
    public class LibraQueueHandler
    {


        private readonly TelemetryClient _telemetryClient;
        public LibraQueueHandler(TelemetryConfiguration configuration)
        {
            _telemetryClient = new TelemetryClient(configuration);
        }

        [FunctionName("Libra-Receive-and-Process")]
        public void Run([ServiceBusTrigger("libra-queue", Connection = "ServiceBusConnection")] Message message, ILogger _logger)
        {
            try
            {
                int second = System.DateTime.Now.Second;
                string payload = System.Text.Encoding.UTF8.GetString(message.Body);
                var messageModel = JsonConvert.DeserializeObject<MessageModel>(payload);
                _logger.LogInformation($"TraceGuid={messageModel.TraceGuid}, MessageId={message.MessageId}");
                var activity = message.ExtractActivity();
                _logger.LogDebug($"activity.RootId={activity.RootId}, activity.ParentID={activity.ParentId}");
                using var operation = _telemetryClient.StartOperation<RequestTelemetry>(activity);
                try
                {
                    _telemetryClient.TrackTrace("This is where processing would happen for LibraQueueHandler.....", SeverityLevel.Information);
                    // dont do anything else with this message
                }
                catch (Exception ex)
                {
                    _telemetryClient.TrackException(ex);
                    operation.Telemetry.Success = false;
                    throw;
                }
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex);
                throw;
            }
        }
    }
}
