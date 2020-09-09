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

        public User[] Users { get; set; }

        public Session[] Sessions { get; set; }

    }

    public class User
    {
        public string Id { get; set; }
        public string Password { get; set; }
    }

    public class Session
    {
        public Step[] Steps { get; set; }
        
    }

    public class Step
    {
        public string StepId { get; set; }
    }
}
