using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Zodiac.Generator
{
  
        internal class RestApi
    {
        static HttpClient client = GetHttpClient();
        internal static async Task<string> Call(string _baseUrl, string api, ILogger log)
        {
            string content;
            HttpResponseMessage response = await client.GetAsync($"{_baseUrl}/{api}");
            if (response.IsSuccessStatusCode) content = await response.Content.ReadAsStringAsync(); else return null;
            return content;
        }
        private static HttpClient GetHttpClient()
        {
            if (client == null)
            {
                client = new HttpClient();
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
            return client;
        }
    }

}
