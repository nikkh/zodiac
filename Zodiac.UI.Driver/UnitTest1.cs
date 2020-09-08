using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;

namespace Zodiac.UI.Driver
{
    [TestClass]
    public class UITests
    {
        IWebDriver driver;

        public UITests()
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--start-maximized");
            driver = new RemoteWebDriver(new Uri("http://52.191.225.140:4444/wd/hub"), chromeOptions.ToCapabilities());
            // driver = new ChromeDriver(@"C:\Users\nhill\Downloads\chromedriver_win32");

        }

        [TestMethod]
        public void TestHomePage()
        {
            driver.Navigate().GoToUrl("https://sirmione-web.azurewebsites.net/");
            //Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
            //string screenshot = ss.AsBase64EncodedString;
            //byte[] screenshotAsByteArray = ss.AsByteArray;
            //ss.SaveAsFile("ss_" + "HomePage" + ".png", ScreenshotImageFormat.Png);
            //var temp = driver.Title;
            //Assert.IsTrue(driver.Title.Contains("Home PageX - Sirmione.Web"));
            //driver.Quit();
        }

        [TestMethod]
        public void TestAries()
        {
            driver.Navigate().GoToUrl("https://sirmione-web.azurewebsites.net/Home/Aries");
            //Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
            //string screenshot = ss.AsBase64EncodedString;
            //byte[] screenshotAsByteArray = ss.AsByteArray;
            //ss.SaveAsFile("ss_" + "Aries" + ".png", ScreenshotImageFormat.Png);
            //var results = driver.FindElement(By.ClassName("row"));
            //Assert.IsTrue(results.Text.Contains("Hello World from Aries!"));
            //driver.Quit();
        }

        [TestMethod]
        public void TestMethod10()
        {
            SearchTest("amazon");
        }

        private void SearchTest(string queryString)
        {
            driver.Navigate().GoToUrl("https://www.google.co.in");
            driver.Manage().Window.Maximize();
            var query = driver.FindElement(By.Name("q"));
            query.SendKeys(queryString);
            query.Submit();
            string text = driver.FindElement(By.XPath("//*[@id='rso']/div/div/div/div/div/h3/a")).Text;
            Assert.IsTrue(text.ToUpper().Contains(queryString.ToUpper()));
            driver.FindElement(By.XPath("//*[@id='rso']/div/div/div/div/div/h3/a")).Click();
            Assert.IsTrue(driver.Title.ToUpper().Contains(queryString.ToUpper()));
            Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
            string screenshot = ss.AsBase64EncodedString;
            byte[] screenshotAsByteArray = ss.AsByteArray;
            ss.SaveAsFile("ss_" + queryString + ".png", ScreenshotImageFormat.Png);
            driver.Quit();
        }
    }
}
