using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SirmioneInsightsGenerator
{
    class Program
    {
        const int NUMBER_OF_CALLS = 100;
        const string baseUrl = "https://sirmione-web.azurewebsites.net/home/";
        
        static void Main(string[] args)
        {

            string[] pageMaster = { "aries", "cancer", "taurus", "gemini", "leo", "virgo", "libra", "scorpio", "sagittarius", "capricorn", "pisces", "aquarius" };
            Console.WriteLine($"Possible API Calls are:");
            foreach (var item in pageMaster)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine($"About to generate {NUMBER_OF_CALLS} calls.");
            List<string> pages = new List<string>();
            for (int i = 0; i < NUMBER_OF_CALLS; i++)
            {
                int min = 1;
                int max = pageMaster.Length + 1;
                Random random = new Random();
                int index = random.Next(min, max);
                var traceGuid = Guid.NewGuid().ToString();
                var pageString = $"{pageMaster[index - 1]}?traceGuid=INSIGHTSGENERATOR: {traceGuid}";
                if (pageMaster[index - 1] == "pisces")
                {
                    if (shouldMaxCPU())
                    {
                        pageString += "&cpumax=true";
                    }
                    else
                    {
                        pageString += "&cpumax=false";
                    }

                }
                pages.Add(pageString);

                Console.WriteLine($"Call will be {pageString}");
            }


            foreach (var page in pages)
            {
                RestApi.Call(baseUrl, page);
                Thread.Sleep(100);
                

            }

            
        }

        private static bool shouldMaxCPU()
        {
            int min = 1;
            int max = 5;
            Random random = new Random();
            int index = random.Next(min, max);
            if (index == 3) return true;
            return false;
        }

     
    }

   
}
