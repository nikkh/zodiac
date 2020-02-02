using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using limone_api.Utils;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;
using System.IO;
using System.Text;
using Azure.Storage.Blobs.Models;
using Azure;

namespace limone_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeoController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly TelemetryClient _telemetryClient;
        private readonly IConfiguration _configuration;

        public LeoController(ILogger<LeoController> logger, TelemetryClient telemetryClient, IConfiguration configuration)
        {
            _logger = logger;
            _telemetryClient = telemetryClient;
            _configuration = configuration;
        }

         
         /// <summary>
         /// This api returns a random list of posts in lorem ipsum, obtained form a third party web service
         /// </summary>
       
        [HttpGet]
        public async Task<ActionResult<string>> Get(string traceGuid)
        {
            const string controllerName = "Leo";

            var metricName = $"{controllerName}Transactions";
            var message = $"{controllerName} has been invoked. TraceGuid={traceGuid}";
            _telemetryClient.TrackEvent(message);
            _telemetryClient.GetMetric(metricName).TrackValue(1);
            _logger.LogInformation(message);
            var storageConnectionString = _configuration["Azure:Storage:ConnectionString"];
            // Create a BlobServiceClient object which will be used to create a container client
            BlobServiceClient blobServiceClient = new BlobServiceClient(storageConnectionString);

            //Create a unique name for the container
            string containerName = "leoblobs";

            // Dont know whats happened to CreateIfNotExists in the API?
            BlobContainerClient containerClient = null;
            try
            {
                containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);
                _logger.LogInformation($"Container {containerName} was created.");
            }
            catch (RequestFailedException rfe)
            {
                containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                _logger.LogInformation($"Container {containerName} already exists.");
            }
            string blobName = $"blob{System.DateTime.Now.Ticks.ToString()}";
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            string blobContents = $"Blob {blobName} created by Leo {System.DateTime.Now.ToLongDateString()}\n";

            
            Byte[] byteArray = Encoding.ASCII.GetBytes(blobContents);
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                await blobClient.UploadAsync(stream);
                stream.Close(); ;
            }
            _logger.LogInformation($"Blob {blobName} was created at {blobClient.Uri.ToString()}.");
            return blobClient.Uri.ToString();

        }

       

    }
}
