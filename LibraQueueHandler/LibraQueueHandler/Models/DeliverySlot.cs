using System;
using System.Collections.Generic;
using System.Text;

namespace LibraQueueHandler.Models
{
    public class DeliverySlot
    {
        public string SlotId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Status { get; set; }
    }
}
