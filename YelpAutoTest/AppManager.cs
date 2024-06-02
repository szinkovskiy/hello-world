using OpenQA.Selenium.Appium.Android;
using YelpAutoTest.Actions;

namespace YelpAutoTest
{
    public class AppManager
    {
        private readonly AndroidDriver _driver;
        private readonly Authorization _auth;
        private readonly Filtering _filter;
        private readonly ResultsManipulation _results;
        
        public AppManager()
        {
            _driver = new MobileDriver().RunApp();
            
            _auth = new Authorization(_driver);
            _filter = new Filtering(_driver);
            _results = new ResultsManipulation(_driver);
        }
        
        public Authorization Auth => _auth;
        public Filtering Filter => _filter;
        public ResultsManipulation Results => _results;
        
        
        public void Stop()
        {
            _driver.Quit();
        }
    }
}