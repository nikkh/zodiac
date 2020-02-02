using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sirmione.Web.Models;
using Sirmione.Web.Utils;

namespace Sirmione.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _limoneBaseUrl;
        private readonly string _scorpioBaseUrl;
        const string limoneUrlKey = "LimoneBaseUrl";
        const string scorpioUrlKey = "ScorpioBaseUrl";
        private readonly TelemetryClient _telemetryClient;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, TelemetryClient telemetryClient)
        {
            _logger = logger;
            _configuration = configuration;
            _limoneBaseUrl = _configuration[limoneUrlKey];
            _scorpioBaseUrl = _configuration[scorpioUrlKey];
            _telemetryClient = telemetryClient;
        }

        public IActionResult Index()
        {
            var vm = new IndexViewModel();
            vm.Signs.Add("ARIES", new SignPartialViewModel { Name = "Aries", 
                Description = "The most simple transaction. Simply calls a RESTful service in different Web Application, different AppInsights instance", 
                Image = "/images/aries.svg" });
            vm.Signs.Add("TAURUS", new SignPartialViewModel { Name = "Taurus", 
                Description = "This transaction calls a RESTful service that waits a random amount of time before returning. Fails between 16.00-16.30", 
                Image = "/images/taurus.svg" });
            vm.Signs.Add("GEMINI", new SignPartialViewModel { Name = "Gemini", Description = "Calls a RESTFul service that always throws an exception [show snapshot debugger]", Image = "/images/gemini.svg" });
            vm.Signs.Add("CANCER", new SignPartialViewModel { Name = "Cancer", Description = "Calls a RESTFul service that calls another RESTful service", Image = "/images/cancer.svg" });
            vm.Signs.Add("LEO", new SignPartialViewModel { Name = "Leo", Description = "Writes a blob to Blob Storage", Image = "/images/leo.svg" });
            vm.Signs.Add("VIRGO", new SignPartialViewModel { Name = "Virgo", Description = "writes to service bus queue which triggers azure function, using a different AI instance", Image = "/images/virgo.svg" });
            vm.Signs.Add("LIBRA", new SignPartialViewModel { Name = "Libra", Description = "writes to service bus queue which triggers azure function, using the same AI instance", Image = "/images/libra.svg" });
            vm.Signs.Add("SCORPIO", new SignPartialViewModel { Name = "Scorpio", Description = "[calls a restful service that does lot and lots of stuff, including SQL database and Cosmos]", Image = " /images/scorpio.svg" });
            vm.Signs.Add("SAGITTARIUS", new SignPartialViewModel { Name = "Sagittarius", Description = "[Like Taurus, also called from Availability Tests.  Occasionally takes a really long time]", Image = "/images/sagittarius.svg" });
            vm.Signs.Add("CAPRICORN", new SignPartialViewModel { Name = "Capricorn", Description = "calls a RESTful service running in Service Fabric", Image = "/images/capricorn.svg" });
            vm.Signs.Add("AQUARIUS", new SignPartialViewModel { Name = "Aquarius", Description = "calls a restful service that is instrumented with lots of custom metrics and stuff", Image = "/images/aquarius.svg" });
            vm.Signs.Add("PISCES", new SignPartialViewModel { Name = "Pisces", Description = "calls a service that uses a lot of CPU", Image = "/images/pisces.svg" });
            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Scorpio()
        {
            var traceGuid = Guid.NewGuid().ToString();
            var vm = new ScorpioViewModel();
            vm.ResponseData = await CallRestApi(_scorpioBaseUrl, $"api/scorpio?traceGuid={traceGuid}");
            vm.TraceGuid = traceGuid;
            return View("ScorpioResult", vm);
        }

        public async Task<IActionResult> Aries()
        {
                       
            var traceGuid = Guid.NewGuid().ToString();
            var vm = new AriesViewModel();
            vm.ResponseData = await CallRestApi(_limoneBaseUrl, $"api/aries?traceGuid={traceGuid}");
            vm.TraceGuid = traceGuid;
            return View("AriesResult", vm);
        }
        public async Task<IActionResult> Libra()
        {
            var traceGuid = Guid.NewGuid().ToString();
            var vm = new LibraViewModel();
            vm.ResponseData = await CallRestApi(_limoneBaseUrl, $"api/libra?traceGuid={traceGuid}");
            vm.TraceGuid = traceGuid;
            return View("LibraResult", vm);
        }
        public async Task<IActionResult> Capricorn()
        {
            var traceGuid = Guid.NewGuid().ToString();
            var vm = new CapricornViewModel();
            vm.ResponseData = await CallRestApi(_limoneBaseUrl, $"api/capricorn?traceGuid={traceGuid}");
            vm.TraceGuid = traceGuid;
            return View("CapricornResult", vm);
        }

        public async Task<IActionResult> Sagittarius()
        {
            var traceGuid = Guid.NewGuid().ToString();
            var vm = new SagittariusViewModel();
            vm.ResponseData = await CallRestApi(_limoneBaseUrl, $"api/sagittarius?traceGuid={traceGuid}");
            vm.TraceGuid = traceGuid;
            return View("SagittariusResult", vm);
        }

        public async Task<IActionResult> Taurus()
        {
            var traceGuid = Guid.NewGuid().ToString();
            var vm = new TaurusViewModel();
            vm.ResponseData = await CallRestApi(_limoneBaseUrl, $"api/taurus?traceGuid={traceGuid}");
            vm.TraceGuid = traceGuid;
            return View("TaurusResult", vm);
        }

        public async Task<IActionResult> Gemini()
        {
            var traceGuid = Guid.NewGuid().ToString();
            var vm = new GeminiViewModel();
            vm.ResponseData = await CallRestApi(_scorpioBaseUrl, $"api/gemini?traceGuid={traceGuid}");
            vm.TraceGuid = traceGuid;
            return View("GeminiResult", vm);
        }
        public async Task<IActionResult> Cancer()
        {
            var traceGuid = Guid.NewGuid().ToString();
            var vm = new CancerViewModel();
            vm.ResponseData = await CallRestApi(_limoneBaseUrl, $"api/cancer?traceGuid={traceGuid}");
            vm.TraceGuid = traceGuid;
            return View("CancerResult", vm);
        }
        public async Task<IActionResult> Virgo()
        {
            var traceGuid = Guid.NewGuid().ToString();
            var vm = new VirgoViewModel();
            vm.ResponseData = await CallRestApi(_limoneBaseUrl, $"api/virgo?traceGuid={traceGuid}");
            vm.TraceGuid = traceGuid;
            return View("VirgoResult", vm);
        }
        public async Task<IActionResult> Leo()
        {
            var traceGuid = Guid.NewGuid().ToString();
            var vm = new LeoViewModel();
            vm.ResponseData = await CallRestApi(_limoneBaseUrl, $"api/leo?traceGuid={traceGuid}");
            vm.TraceGuid = traceGuid;
            return View("LeoResult", vm);
        }
        public async Task<IActionResult> Pisces()
        {
            var traceGuid = Guid.NewGuid().ToString();
            var vm = new PiscesViewModel();
            vm.ResponseData = await CallRestApi(_limoneBaseUrl, $"api/pisces?traceGuid={traceGuid}&cpumax=false");
            vm.TraceGuid = traceGuid;
            return View("PiscesResult", vm);
        }

        public async Task<IActionResult> Aquarius()
        {
            var traceGuid = Guid.NewGuid().ToString();
            var vm = new AquariusViewModel();
            vm.ResponseData = await CallRestApi(_limoneBaseUrl, $"api/aquarius?traceGuid={traceGuid}");
            vm.TraceGuid = traceGuid;
            return View("AquariusResult", vm);
        }

        public async Task<IActionResult> Zodiac(string key ="", int numberOfCalls = 50)
        {
            if (key != "112fbd24-5680-4c60-9a46-fa68c4b915e5") { throw new Exception("Zodiac requires an Api Key"); }

            
            const string baseUrl = "https://limone-api.azurewebsites.net/api/";
            const string scorpioUrl = "https://scorpio-api.azurewebsites.net/api/";
            var vm = new ZodiacViewModel();
            string lastGuid = "";
            string[] pageMaster = { "aries", "cancer", "taurus", "gemini", "leo", "virgo", "libra", "scorpio", "sagittarius", "capricorn", "pisces", "aquarius" };
            List<string> pages = new List<string>();
            for (int i = 0; i < numberOfCalls; i++)
            {
                int min = 1;
                int max = pageMaster.Length + 1;
                Random random = new Random();
                int index = random.Next(min, max);
                var traceGuid = Guid.NewGuid().ToString();
                lastGuid = traceGuid;
                var pageString = $"{pageMaster[index - 1]}?traceGuid=zodiac-{traceGuid}";
                if (pageMaster[index - 1] == "pisces")
                {
                    if (shouldMaxCPU())
                    {
                        pageString += "&cpumax=true";
                    }
                    else
                    {
                        pageString += "&cpumax=false";
                    }

                }
                pages.Add(pageString);

                Console.WriteLine($"Call will be {pageString}");
            }


            foreach (var page in pages)
            {
                Parameters p = new Parameters { Operation = page, Url = baseUrl };
                if ((page.ToLower().StartsWith("scorpio")) || (page.ToLower().StartsWith("gemini")))
                {
                    p.Url = scorpioUrl;
                }
                Thread newThread = new Thread(HomeController.InvokeAsync);
                newThread.Start(p);

            }

            vm.ResponseData = "Zodiac completed sucessfully";
            vm.TraceGuid = lastGuid;
            return View("ZodiacResult", vm);
        }

        public static void InvokeAsync(object parameters)
        {
            try
            {
                Parameters p = (Parameters)parameters;
                Console.WriteLine(RestApi.Call(p.Url, p.Operation));
            }
            catch (Exception e) { } // expect some exceptions as normal}
        }

        private static bool shouldMaxCPU()
        {
            int min = 1;
            int max = 5;
            Random random = new Random();
            int index = random.Next(min, max);
            if (index == 3) return true;
            return false;
        }

        private async Task<string> CallRestApi(string _baseUrl, string api)
        {
            string content = "";
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(_baseUrl);
                // client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage response = await client.GetAsync(api);
                
                //Checking the response is successful or not which is sent using HttpClient  
                if (response.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    content = response.Content.ReadAsStringAsync().Result;
                }
                else 
                {
                    throw new Exception($"Call to Restful service {_baseUrl}/{api} failed. {response.StatusCode.ToString()}");
                }
                return content;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
    class Parameters
    {
        public string Operation { get; internal set; }
        public string Url { get; internal set; }
    }
}
