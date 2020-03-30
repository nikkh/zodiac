using System;
using System.Collections.Generic;
using System.Text;

namespace LibraQueueHandler.Models
{
    public class MonitoringContext
    {
        public string MonitoringContainerName { get; set; }
        public string RequestHeaderFileName { get; set; }
        public string RequestBodyFileName { get; set; }
        public bool NotifyUnavailability { get; set; }
    }
}
