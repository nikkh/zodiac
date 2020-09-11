using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace Zodiac.Generator
{
    public class ZodiacContext
    {
        public int NumberOfCallsPerInvocation { get; set; }
        public int NumberOfUserJourneys { get; set; }
        public string BaseUrl { get; set; }

        public bool UserSimulationEnabled { get; set; }
        public string UserTestingParametersStorageConnectionString { get; set; }
        public int MinimumThinkTimeInMilliseconds { get; set; }
    }
}
