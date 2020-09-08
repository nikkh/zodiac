using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var capricornLink = driver.FindElement(By.Id("action-Capricorn"));
            capricornLink.Click();
            var emailbox = driver.FindElement(By.Id("i0116"));
            log.LogTrace($"Found emailbox {emailbox.Text}");
            emailbox.SendKeys("neves@xekina.onmicrosoft.com");
            log.LogTrace($"Typed email address {emailbox.Text}");
            var nextButton = driver.FindElement(By.Id("idSIButton9"));
            log.LogTrace($"Found Next Button");
            nextButton.Click();
            log.LogTrace($"Clicked Next Button");
            var passwordBox = driver.FindElement(By.Id("i0118"));
            log.LogTrace($"Found passwordBox");
            passwordBox.SendKeys("Boldmere1883@@@");
            log.LogTrace("Typed Password");
            var signinButton = driver.FindElement(By.Id("idSIButton9"));
            log.LogTrace($"Found signinButton");

            signinButton.Click();
            log.LogTrace($"signinButton clicked");
            try
            {
                var staySignedIn = driver.FindElement(By.Id("idBtn_Back"));
                log.LogTrace($"Found idBtnBack");
                staySignedIn.Click();
                log.LogTrace("clicked stay signed in");

            }
            catch (Exception e)
            {

            }
            var goButton = driver.FindElement(By.ClassName("btn btn-primary"));
            log.LogTrace($"Found goButton");
            goButton.Click();
            return "ok";
        }
    }
}
