using OpenQA.Selenium;
using SeleniumLab.PageObjects;
using NUnit.Framework;
using SeleniumLab.FrontendHelper;
using SeleniumLab.WebDriversConfigs;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SeleniumLab.Automation
{
    internal class TestScenario
    {
        internal required IWebDriver driver;
        internal required WebDriverFactory driverFactory;
        internal required GooglePage googlePage;
        internal required YouTubePage youtubePage;
        internal required GooglePage.Actions actions;

        #region SetUp and TearDown
        [SetUp]
        public void Setup() {
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--no-sandbox");
            options.AddArguments("--disable-dev-shm-usage");
            options.AddArguments("--headless");
            driver = new ChromeDriver(options);
            // launch the forefox browser

            driver.Navigate().GoToUrl("https://lively-moss-0496d6403.4.azurestaticapps.net/");
        }     
        [TearDown]
        public void TearDown(){
            driver.Close();
        }
        #endregion

        [Test]
        public void ClickDropdownOptions()
        {
            try
            {
                // Wait for the dropdown to appear using the label text "Grid Company"
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
                IWebElement dropDown = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//label[text()='Grid Company']/ancestor::div[contains(@class, 'mud-select')]")));
                Thread.Sleep(4000);

                // Click to open the dropdown
                dropDown.Click();
                Console.WriteLine("Dropdown opened");
                Thread.Sleep(4000);

                // Wait for the dropdown options to become visible
                IList<IWebElement> options = wait.Until(driver =>
                    driver.FindElements(By.XPath("//div[contains(@class, 'mud-popover')]//div[contains(@class, 'mud-list-item-text')]")));
                Console.WriteLine($"Options found: {options.Count}");
                // Initialize the Actions class for hover actions
                IWebElement secondOption = options[1];
                secondOption.Click();
                Thread.Sleep(2000);
                Console.WriteLine("Second option clicked");
                IWebElement result = wait.Until(ExpectedConditions.ElementIsVisible(
                                   By.XPath("//label[text()='Grid Company']/ancestor::div[contains(@class, 'mud-select')]//div[contains(@class,'mud-input-slot')]")));
                Assert.That(result.Text, Is.EqualTo("Grid Company 2"));

            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine($"Element not found: {ex.Message}");
            }
            catch (WebDriverTimeoutException ex)
            {
                Console.WriteLine($"Timeout waiting for element: {ex.Message}");
            }
        }
        //[Test]
        public void Should_Open_Google_And_Search_For_Selenium()
        {
            actions.GoToGoogleUrl(driver);
            actions.SearchForTerm("Selenium");
            AssertionExtensions.ShouldContainRoute(driver, "/search?q=Selenium", "Google Route is not correct");
        }

        //[Test]
        public void Should_Open_YouTube_And_Search_For_Selenium()
        {
            youtubePage.Open();
            Assert.That(driver.Url, Is.EqualTo(youtubePage.Url), "YouTube URL is not correct");
            Assert.That(youtubePage.YouTubeLogo.Displayed, "YouTube logo should be visible.");

            Assert.That(youtubePage.SearchBox.Enabled, "YouTube search box should be enabled.");
            Assert.That(youtubePage.SearchBox.Displayed, "YouTube search box should be visible.");
            youtubePage.SearchBox.Click();
            youtubePage.SearchBox.SendKeys("Selenium" + Keys.Enter);
            Assert.That(driver.Url.ToString(), Does.Contain("/results?search_query=Selenium"), "YouTube Route is not correct");
        }
    }
}