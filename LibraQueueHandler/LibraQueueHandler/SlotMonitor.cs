using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using LibraQueueHandler.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using MimeKit;
using Newtonsoft.Json.Linq;

namespace LibraQueueHandler
{
    public class SlotMonitor
    {
        private readonly IConfiguration _config;
        private readonly MonitoringContext _monitoringContext;
        private readonly string _storageConnectionString;
        private readonly string _mailPassword;

        const string FUNCTION_NAME = "[SlotMonitor]";

        public SlotMonitor(IConfiguration config, MonitoringContext monitoringContext)
        {
            _config = config;
            _monitoringContext = monitoringContext;
            _storageConnectionString = _config["AzureWebJobsStorage"];
            _mailPassword = _config["MailPassword"];
        }

        [FunctionName("SlotMonitor")]
        public async void Run([TimerTrigger("0 */30 * * * *")]TimerInfo myTimer, ILogger log)
        {
            DateTime lastSlotPreviously = await GetPreviousSlotDate();
            DateTime lastSlotCurrently = DateTime.MaxValue;
            log.LogInformation($"Slot Monitor Timer trigger function executed at: {DateTime.Now}");

            string body = await CheckForSlots();
            var slots = ParseSlots(body);

            var freeSlots = slots.Where(s => s.Status != "UNAVAILABLE");
            
            
            if (freeSlots != null && freeSlots.Count() > 0)
            {
            log.LogInformation($"{freeSlots.Count()} available");
            
                foreach (var item in freeSlots)
                {
                    log.LogDebug($"FREE SLOT: {item.StartDateTime.ToShortDateString()} at {item.StartDateTime.ToShortDateString()}");
                }
                await Notify(freeSlots);

            }
            else
            {
                    
                
                var firstSlot = slots.OrderBy(s => s.StartDateTime).FirstOrDefault();
                var lastSlot = slots.OrderByDescending(s => s.StartDateTime).First();
                log.LogInformation($"There are no free slots in the period {firstSlot.StartDateTime.ToShortDateString()} to {lastSlot.EndDateTime.ToShortDateString()}");
                await Notify();
            }

            
            lastSlotCurrently = slots.OrderByDescending(s => s.StartDateTime).First().EndDateTime;
            log.LogDebug($"Last slot was previously {lastSlotPreviously.ToLongDateString()} and is now {lastSlotCurrently.ToLongDateString()}");
            if (lastSlotCurrently > lastSlotPreviously)
            {
                log.LogInformation($"Date of the last slot has changed.  It is now: {lastSlotCurrently.ToShortDateString()}");
            }
            await SetPreviousSlotDate(lastSlotCurrently);
            return;
        }

        private async Task<DateTime> GetPreviousSlotDate()
        {
            CloudBlobClient blobClient;
            CloudBlobContainer inboundContainer;

            try
            {
                var storageAccount = CloudStorageAccount.Parse(_storageConnectionString);
                blobClient = storageAccount.CreateCloudBlobClient();
                inboundContainer = blobClient.GetContainerReference(_monitoringContext.MonitoringContainerName);
                var dateBlob = inboundContainer.GetBlockBlobReference("dateBlob.txt");
                string blobDate = await dateBlob.DownloadTextAsync();
                DateTime dateValue;
                if (DateTime.TryParse(blobDate, out dateValue))
                    return dateValue;
                else
                {
                    return DateTime.MinValue;
                }

            }
            catch (Exception e)
            {
                return DateTime.MinValue;
            }
        }
        private async Task SetPreviousSlotDate(DateTime latestSlotDate)
        {
            CloudBlobClient blobClient;
            CloudBlobContainer inboundContainer;

            try
            {
                var storageAccount = CloudStorageAccount.Parse(_storageConnectionString);
                blobClient = storageAccount.CreateCloudBlobClient();
                inboundContainer = blobClient.GetContainerReference(_monitoringContext.MonitoringContainerName);
                var dateBlob = inboundContainer.GetBlockBlobReference("dateBlob.txt");
                await dateBlob.UploadTextAsync(latestSlotDate.ToString("o"));
            }
            catch (Exception e) 
            {
                throw new Exception($"Exception loadeding last slot date from storage {e.Message}");
            }
        }
        private async Task<string> CheckForSlots()
        {

            CloudBlobClient blobClient;
            CloudBlobContainer inboundContainer;
            string reqbodyBlobContents = "";
            string[] reqhdrBlobContents = null;
            try
            {
                var storageAccount = CloudStorageAccount.Parse(_storageConnectionString);
                blobClient = storageAccount.CreateCloudBlobClient();
                inboundContainer = blobClient.GetContainerReference(_monitoringContext.MonitoringContainerName);
                var reqhdrBlob = inboundContainer.GetBlockBlobReference(_monitoringContext.RequestHeaderFileName);
                var reqhdrBlobContentsString = await reqhdrBlob.DownloadTextAsync();
                string[] stringSeparators = new string[] { "\r\n" };
                reqhdrBlobContents = reqhdrBlobContentsString.Split(stringSeparators, StringSplitOptions.None);

                var reqbodyBlob = inboundContainer.GetBlockBlobReference(_monitoringContext.RequestBodyFileName);
                reqbodyBlobContents = await reqbodyBlob.DownloadTextAsync();
                var jsonBody = JObject.Parse(reqbodyBlobContents);
                jsonBody["data"]["start_date"] = DateTime.Now.ToString("o");
                jsonBody["data"]["end_date"] = DateTime.Now.AddDays(15).ToString("o");
                reqbodyBlobContents = jsonBody.ToString();
            }
            catch (Exception e)
            {
                throw new Exception($"Error processing blob storage {e.Message}", e);
            }
            
            string responseBody = null;

            var url = $"https://groceries.asda.com/api/v3/slot/view";
            HttpResponseMessage response;
            var clientHandler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            using (var client = new HttpClient(clientHandler))
            {
                client.DefaultRequestHeaders.Clear();
                var data = new StringContent(reqbodyBlobContents, Encoding.UTF8, "application/json");

                foreach (var line in reqhdrBlobContents)
                {

                    var header = line.Split(':');
                    var name = header[0].Trim();
                    var value = header[1].Trim();


                    if (!name.ToLower().Contains("content-"))
                    {
                        client.DefaultRequestHeaders.Add(name, value);
                    }
                }
                data.Headers.ContentLength = reqbodyBlobContents.Length;
                data.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                response = await client.PostAsync(url, data);
                if (response.IsSuccessStatusCode)
                {
                    responseBody = await response.Content.ReadAsStringAsync();

                }
                else
                {
                    throw new Exception($"That didnt work.  Calling slots api {url} Response:{response.StatusCode.ToString()}");
                }
                return responseBody;
            }
        }
        private static List<DeliverySlot> ParseSlots(string jsonToParse)
        {
            JObject jsonContent;
            JToken slotDays;
            List<DeliverySlot> deliverySlots = new List<DeliverySlot>();
            try
            {
                
                jsonContent = JObject.Parse(jsonToParse);
                slotDays = jsonContent["data"]["slot_days"];
            }
            catch (Exception e)
            {
                throw new Exception($"Error parsing json response {e.Message}.  See inner exception for details", e);
            }

            
            foreach (var slotDay in slotDays.Children())
            {
                foreach (var day in slotDay.Children())
                {

                    foreach (var slots in day.Children())
                    {

                        foreach (var slot in slots.Children())
                        {

                            var slotInfo = slot["slot_info"];

                            DeliverySlot ds = new DeliverySlot();
                            ds.SlotId = slotInfo["slot_id"].ToString();

                            DateTime dateValue;
                            if (DateTime.TryParse(slotInfo["start_time"].ToString(), out dateValue))
                                ds.StartDateTime = dateValue;
                            else
                            {
                                throw new Exception($"Slot {ds.SlotId} has an invalid start date");
                            }

                            if (DateTime.TryParse(slotInfo["end_time"].ToString(), out dateValue))
                                ds.EndDateTime = dateValue;
                            else
                            {
                                throw new Exception($"Slot {ds.SlotId} has an invalid end date");
                            }


                            ds.Status = slotInfo["status"].ToString();
                            deliverySlots.Add(ds);

                        }



                    }


                }


            }
            return deliverySlots;

        }
        private async Task Notify(IEnumerable<DeliverySlot> freeSlots = null)
        {
            bool slotsAvailable = false;
            if ((!(freeSlots == null)) && (freeSlots.Count() > 0))
            {
                slotsAvailable = true;
            }
            if (!slotsAvailable && !_monitoringContext.NotifyUnavailability)
            {
                return;
            }
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("ASDA Slot Checker", "nick@nikkh.net"));
            message.To.Add(new MailboxAddress("Nick Hill", "nhill@microsoft.com"));
            message.To.Add(new MailboxAddress("Nikki Chatwin", "nikki.chatwin@hotmail.co.uk"));

            if (slotsAvailable) 
            {
                message.Subject = "ASDA Slots: There are free slots available at ASDA Groceries!";
                string body = $"There are free slots as of {DateTime.Now.ToShortDateString()} at {DateTime.Now.ToShortTimeString()}:\n";
                foreach (var slot in freeSlots)
                {
                    body += $"{slot.StartDateTime.ToLongDateString()}\n";
                }

                message.Body = new TextPart("plain")
                {
                    Text = $"{body}"
                };
            }
            else
            {
                message.Subject = "ASDA Slots: None Available :-(";
                string body = $"There are no free slots as of {DateTime.Now.ToShortDateString()} at {DateTime.Now.ToShortTimeString()}";
                message.Body = new TextPart("plain")
                {
                    Text = $"{body}"
                };
            }
           

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.office365.com", 587, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync("nick@nikkh.net", _mailPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }

        private async Task Notify(DateTime previousLastSlotDate, DateTime currentLastSlotDate)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("ASDA Slot Checker", "nick@nikkh.net"));
            message.To.Add(new MailboxAddress("Nick Hill", "nhill@microsoft.com"));
            message.To.Add(new MailboxAddress("Nikki Chatwin", "nikki.chatwin@hotmail.co.uk"));
            message.Subject = $"Slots have been released up to {currentLastSlotDate.ToShortDateString()}";
            string body = $"See separate notifications to detemine if any of these slots are free.  The last date for which slots had previously been published was {previousLastSlotDate.ToShortDateString()} at {previousLastSlotDate.ToShortTimeString()}:\n";
            body += $"This date has been extended to {currentLastSlotDate.ToShortDateString()} at {currentLastSlotDate.ToShortTimeString()}";
            message.Body = new TextPart("plain")
            {
                Text = $"{body}"
            };
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.office365.com", 587, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync("nick@nikkh.net", _mailPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
