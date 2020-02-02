using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using scorpio_api.Utils;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;
using System.IO;
using System.Text;
using Azure.Storage.Blobs.Models;
using Azure;
using scorpio_Api.Utils;
using System.Diagnostics;
using System.Data.SqlClient;
using Microsoft.ApplicationInsights.DataContracts;

namespace scorpio_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScorpioController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly TelemetryClient _telemetryClient;
        private readonly IConfiguration _configuration;

        public ScorpioController(ILogger<ScorpioController> logger, TelemetryClient telemetryClient, IConfiguration configuration)
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
            const string controllerName = "Scorpio";
            var metricName = $"{controllerName}Transactions";
            var message = $"{controllerName} has been invoked. TraceGuid={traceGuid}";
            _telemetryClient.TrackEvent(message);
            _telemetryClient.GetMetric(metricName).TrackValue(1);
            
            // call some methods that do some artificial work
            await AccessDatabase();
            DoArtificialWork(traceGuid);
            return await WriteBlob();
        }

        private async Task<string> WriteBlob()
        {
            _telemetryClient.TrackTrace($"WriteBlob()", SeverityLevel.Information);
            var storageConnectionString = _configuration["Azure:Storage:ConnectionString"];
            // Create a BlobServiceClient object which will be used to create a container client
            BlobServiceClient blobServiceClient = new BlobServiceClient(storageConnectionString);
            _telemetryClient.TrackTrace($"BlobServiceClient has been created", SeverityLevel.Information);
            //Create a unique name for the container
            string containerName = "scorpioblobs";

            // Dont know whats happened to CreateIfNotExists in the API?
            BlobContainerClient containerClient = null;
            try
            {
                containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);
                _telemetryClient.TrackTrace($"Container {containerName} was created.", SeverityLevel.Information);
            }
            catch (RequestFailedException rfe)
            {
                containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                _telemetryClient.TrackTrace($"Container {containerName} already exists.", SeverityLevel.Information);
            }
            string blobName = $"blob{System.DateTime.Now.Ticks.ToString()}";
            _telemetryClient.TrackTrace($"Blob name will be {blobName}", SeverityLevel.Information);
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            string blobContents = $"Blob {blobName} created by Scorpio {System.DateTime.Now.ToLongDateString()}\n";
            _telemetryClient.TrackTrace($"Blob contents are {blobContents}", SeverityLevel.Information);

            Byte[] byteArray = Encoding.ASCII.GetBytes(blobContents);
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                await blobClient.UploadAsync(stream);
                stream.Close(); ;
            }
            _telemetryClient.TrackTrace($"Blob {blobName} was created at {blobClient.Uri.ToString()}.", SeverityLevel.Information);
            return blobClient.Uri.ToString();
        }


        private async Task<List<string>> AccessDatabase()
        {
            _telemetryClient.TrackTrace($"AccessDatabase()", SeverityLevel.Information);
            List<string> records = new List<string>();
            string queryString = @"SELECT Description FROM [dbo].[RequestEvents] where Description like '%approved%'";
            var dbConnectionString = _configuration["Azure:a3ssDevDb:ConnectionString"];
            _telemetryClient.TrackTrace($"ConnectionString has been retrieved from configuration");
            
                using (SqlConnection connection = new SqlConnection(dbConnectionString))
                {
                    _telemetryClient.TrackTrace($"SQL connection created");
                    SqlCommand command = new SqlCommand(queryString, connection);
                    await connection.OpenAsync();
                    _telemetryClient.TrackTrace($"SQL connection opened");
                    SqlDataReader reader = command.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                        {
                        _telemetryClient.TrackTrace($"Data Reader has rows");
                        while (reader.Read())
                            {
                                var record = reader.GetString(0);
                                _telemetryClient.TrackTrace($"Datebase record read: {record}");
                                records.Add(record);
                            }
                        }

                    }
                    finally
                    {
                        _telemetryClient.TrackTrace($"Close the Data Reader");
                        // Always call Close when done reading.
                        reader.Close();
                    }
                }
           
            return records;
        }

        private void DoArtificialWork(string traceGuid)
        {
            const string methodName = "DoArtificalWork";
            Stopwatch timer = Stopwatch.StartNew();
            var metricName = $"{methodName}Transactions";
            var message = $"{methodName} has been invoked. TraceGuid={traceGuid}";
            _telemetryClient.TrackEvent(message);
            _telemetryClient.GetMetric(metricName).TrackValue(1);
            Method1();
            Method2();
            Method3();
            Method4();
            timer.Stop();
             metricName = $"{methodName}Timings";
            _telemetryClient.GetMetric(metricName).TrackValue(timer.ElapsedMilliseconds);
        }

        private void Method4()
        {
            Method4a();
            Method4b();
            Method4c();
        }

        private void Method4c()
        {
            for (int i = 0; i < 1000; i++)
            {
                ;
            }
        }

        private void Method4b()
        {
            // this has the biggest impact on the E2E trace.
            // Increase the time it sleeps progressively.
            var minute = DateTime.Now.Minute * 2;
            var delay = (((minute + 1) * 100));
            Thread.Sleep(delay);
            _telemetryClient.TrackTrace($"Method4b slept for {delay} ms", SeverityLevel.Information);
        }

        private async void Method4a()
        {
            await RestApi.Call("https://jsonplaceholder.typicode.com", "posts");
        }

        private void Method3()
        {
            Method1a();
            Method2();
            Method4c();
        }

        private void Method2()
        {
            for (int i = 0; i < 10; i++)
            {
                _logger.LogTrace($"ScorpioController Method2 ITeration={i}");
            }
            Method2a();
           
        }

        private async void Method2a()
        {
            await WriteBlob();
        }

        private void Method1()
        {
            Method1a();
            Method1b();
            Method1c();
        }

        private void Method1c()
        {

            using (new TimedEvent(_telemetryClient, "Method1cEvent"))
            {
                for (int i = 0; i < 1900000; i++)
                {
                    string s = i.ToString();
                }
            }
        }

        private void Method1b()
        {
               
        }

        private void Method1a()
        {
            var levels = Enum.GetValues(typeof(Microsoft.ApplicationInsights.DataContracts.SeverityLevel)).Cast<Microsoft.ApplicationInsights.DataContracts.SeverityLevel>();
            foreach (var level in levels)
            {
                _telemetryClient.TrackTrace("Hello from Method1a", level);
            }
            

        }
    }
}
