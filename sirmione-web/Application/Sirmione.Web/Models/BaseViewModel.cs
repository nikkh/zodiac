using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sirmione.Web.Models
{
    public abstract class BaseViewModel
    {
        public string ResponseData { get; set; }
        public string TraceGuid { get; set; }
    }
}
