using OpenQA.Selenium.Appium.Android;

namespace YelpAutoTest
{
    public class AppManager
    {
        private readonly AndroidDriver _driver;
        private readonly Autorization _auth;
        private readonly Filtering _filter;
        private readonly ResultsManipulation _results;
        
        public AppManager()
        {
            _driver = new MobileDriver().RunApp();
            
            _auth = new Autorization(_driver);
            _filter = new Filtering(_driver);
            _results = new ResultsManipulation(_driver);
        }
        
        public Autorization Auth => _auth;
        public Filtering Filter => _filter;
        public ResultsManipulation Results => _results;
        
        
        public void Stop()
        {
            _driver.Quit();
        }
    }
}