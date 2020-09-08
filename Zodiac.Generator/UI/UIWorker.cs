using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Zodiac.Generator.UI
{
    class UIWorker
    {
        private readonly ZodiacContext _zodiacContext;
        IWebDriver driver;

        internal UIWorker(ZodiacContext zodiacContext)
        {

            _zodiacContext = zodiacContext;
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--start-maximized");
            chromeOptions.AddArgument("--disable-dev-shm-usage"); // overcome limited resource problems
            chromeOptions.AddArgument("--no-sandbox"); // Bypass OS security model
            chromeOptions.AddArgument("--headless");
            chromeOptions.AddArgument("--verbose");
            chromeOptions.AddArgument("--whitelisted-ips=");
            try
            {
                driver = new ChromeDriver(chromeOptions);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        internal async Task<string> Run(ILogger log, string triggerFunction, int calls = 0)
        {
            driver.Navigate().GoToUrl("https://sirmione-web.azurewebsites.net/");
            var links = driver.FindElements(By.TagName("a"));
            return links.ToString(); ;
        }
    }
}
