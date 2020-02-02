using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

using System.Text;
using scorpio_api.Models;

namespace scorpio_api.Utils
{
    public class ServiceBusSender
    {
        
        private QueueClient _queueClient;
        private string connectionString;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public ServiceBusSender(IConfiguration configuration,
            ILogger<ServiceBusSender> logger)
        {
            _configuration = configuration;
            _logger = logger;
            connectionString = _configuration["Azure:ServiceBus:ConnectionString"];
            
        }

        public async Task SendMessage(MessageModel messageModel)
        {
            var csb = new ServiceBusConnectionStringBuilder(connectionString);
            csb.EntityPath = messageModel.Queue;
            _queueClient = new QueueClient(csb);
            string data = JsonConvert.SerializeObject(messageModel);
            Message message = new Message(Encoding.UTF8.GetBytes(data));

            try
            {
                await _queueClient.SendAsync(message);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }
    }
}
