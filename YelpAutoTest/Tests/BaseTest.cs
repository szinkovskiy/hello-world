using NUnit.Framework;
using OpenQA.Selenium.Appium.Android;

namespace YelpAutoTest
{
    public class BaseTest
    {
        protected AndroidDriver driver;
        protected Autorization auth;
        protected Filtering filter;
        protected ResultsManipulation results;

        [SetUp]
        public void Setup()
        {
            driver = new MobileDriver().RunApp();
            
            auth = new Autorization(driver);
            filter = new Filtering(driver);
            results = new ResultsManipulation(driver);
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}