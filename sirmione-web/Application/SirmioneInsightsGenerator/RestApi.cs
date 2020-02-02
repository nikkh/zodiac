using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SirmioneInsightsGenerator
{
    public class RestApi
    {
        public static string Call(string _baseUrl, string api)
        {
            string content = "";
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(_baseUrl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                Console.WriteLine($"About to call to Restful service {_baseUrl}/{api}");
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage response = client.GetAsync(api).Result;

                //Checking the response is successful or not which is sent using HttpClient  
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Success!");
                    //Storing the response details recieved from web api   
                    content = response.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    Console.WriteLine($"Call to Restful service {_baseUrl}/{api} failed. {response.StatusCode.ToString()}");
                    return null;
                }
                
                return content;
            }
        }
    }
}
