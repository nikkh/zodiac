using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace scorpio_Api.Utils
{
    
        internal class TimedEvent : IDisposable
        {
            private string name;
            private Dictionary<string, string> properties;
            private TelemetryClient telemetryClient;
            private Stopwatch timer = Stopwatch.StartNew();

            public TimedEvent(TelemetryClient client, string name, Dictionary<string, string> properties = null)
            {
                this.name = name;
                this.properties = properties;
                this.telemetryClient = client;
            }

            public void Dispose()
            {
                timer.Stop();
                this.telemetryClient.TrackEvent(this.name, this.properties,
                    new Dictionary<string, double> { { "duration", timer.ElapsedMilliseconds } });
            }
        }
    
}
