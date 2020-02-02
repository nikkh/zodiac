using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sirmione.Web.Models
{
    public class IndexViewModel
    {
        public IndexViewModel() 
        {
            Signs = new Dictionary<string, SignPartialViewModel>();
        }
        public Dictionary<string, SignPartialViewModel> Signs { get; set; }
        
    }
}
