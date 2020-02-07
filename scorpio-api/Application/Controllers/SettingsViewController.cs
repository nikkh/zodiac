using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace scorpio_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsViewController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly TelemetryClient _telemetryClient;
        private readonly IConfiguration _configuration;
        private List<string> _results = new List<string>();

        public SettingsViewController(ILogger<SettingsViewController> logger, TelemetryClient telemetryClient, IConfiguration configuration)
        {
            _logger = logger;
            _telemetryClient = telemetryClient;
            _configuration = configuration;
            
        }

        private void LogString(string name, string stringToLog)
        {
            _results.Add($"{name}:{stringToLog}");
           
        }
        // GET: api/SettingsView
        [HttpGet]
        public IEnumerable<string> Get()
        {
 
            string myCustomKeyFormatA = _configuration["MyCustomKey"];
            LogString("myCustomKeyFormatA", myCustomKeyFormatA);

            string myCompoundCustomKeyElement1FormatA = _configuration["MyCompoundCustomKey:Element1"];
            LogString("myCompoundCustomKeyElement1FormatA", myCompoundCustomKeyElement1FormatA);

            string myCompoundCustomKeyElement2FormatA = _configuration["MyCompoundCustomKey:Element2"];
            LogString("myCompoundCustomKeyElement2FormatA", myCompoundCustomKeyElement2FormatA);

            var dbConnectionString = _configuration["Azure:a3ssDevDb:ConnectionString"];
            LogString("dbConnectionString", dbConnectionString.Substring(0, 15));

            return _results;
        }

        
    }
}
