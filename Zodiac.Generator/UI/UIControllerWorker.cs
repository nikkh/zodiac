using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zodiac.Generator.UI
{
    class UIControllerWorker
    {
        private readonly Random random = new Random();
        private readonly ZodiacContext _zodiacContext;
        IWebDriver driver;

        internal UIControllerWorker(ZodiacContext zodiacContext)
        {

            _zodiacContext = zodiacContext;
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--start-maximized");
            chromeOptions.AddArgument("--disable-dev-shm-usage"); // overcome limited resource problems
            chromeOptions.AddArgument("--no-sandbox"); // Bypass OS security model
            chromeOptions.AddArgument("--headless");
            chromeOptions.AddArgument("--verbose");
            chromeOptions.AddArgument("--whitelisted-ips=");
            chromeOptions.AddArgument("--user-agent=" + "Zodiac.Generator");
            try
            {
                driver = new ChromeDriver(chromeOptions);
            }
            catch (Exception e)
            {
                throw e;
            }
           
        }

        internal User GetRandomUser()
        {
            return _zodiacContext.Users[random.Next(0, _zodiacContext.Users.Count())];
        }

        internal Session GetRandomSession()
        {
            return _zodiacContext.Sessions[random.Next(0, _zodiacContext.Sessions.Count())];
        }

        internal async Task<string> Run(ILogger log, string triggerFunction, int calls = 0)
        {

            Random random = new Random();
            if (calls == 0)
            {
                calls = random.Next(1, _zodiacContext.NumberOfUserJourneys);
            }
            try
            {
                for (int i = 0; i < calls; i++)
                {
                    driver.Navigate().GoToUrl("https://sirmione-web.azurewebsites.net/");
                    AADLogin(GetRandomUser(), log);
                    EnactSession(GetRandomSession(), log);
                }
            }
            catch (Exception e)
            {
                log.LogError($"Exception while generating synthetic user traffic! Message: {e.Message}");
            }
            return driver.Title;
        }

        private void EnactSession(Session session, ILogger log)
        {
            // Click the Go button
            FindAndClick("capricorn-go", log);
            Think();

            for (int i = 0; i < session.Steps.Length - 1; i++)
            {
                FindAndClick(session.Steps[i].StepId, log);
                Think();
            }
        }

        private void AADLogin(User user, ILogger log)
        {
            
            // Click Capricorn Option (causes ADD Login)
            FindAndClick("action-Capricorn", log);
            Think();

            // Fill in email address and click next.
            SendToElement("i0116", user.Id, log);
            Think();
            FindAndClick("idSIButton9", log);
            Think();

            // Fill in the password and click next
            SendToElement("i0118", user.Password, log);
            Think();
            FindAndClick("idSIButton9", log);
            Think();

            // Do not select the stay signed in option
            try
            {
                FindAndClick("idBtn_Back", log);
                Think();
            }
            catch (Exception){}
        }

        private void Think()
        {
            int thinkTime = random.Next(250, 3000);
            Thread.Sleep(thinkTime);
        }

        public IWebElement FindAndClick(string Id, ILogger log)
        {
            try
            {
                var element = driver.FindElement(By.Id(Id));
                element.Click();
                return element;
            }
            catch (Exception e)
            {
                log.LogError($"FindAndClick(): Exception {e.Message}");
                throw e;
            }
        }

        public void SendToElement(string Id, string data, ILogger log)
        {
            try
            {
                var element = driver.FindElement(By.Id(Id));
                log.LogTrace($"Found element with Id {Id}");
                element.SendKeys(data);
            }
            catch (Exception e)
            {
                log.LogError($"SendToElement(): Exception {e.Message}");
                throw e;
            }
        }

    }
}
