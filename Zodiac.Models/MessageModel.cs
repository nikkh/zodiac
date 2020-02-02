using System;
using System.Collections.Generic;
using System.Text;

namespace Zodiac.Models
{
    public class MessageModel
    {
        public string Payload { get; set; }
        public string TraceGuid { get; set; }
        public string Queue { get; internal set; }
    }
}

