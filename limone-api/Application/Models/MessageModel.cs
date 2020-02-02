using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace limone_api.Models
{
    public class MessageModel
    {
        public string Payload { get; set; }
        public string TraceGuid { get; set; }
        public string Queue { get; internal set; }
    }
}
