using OpenQA.Selenium.Appium.Android;

namespace YelpAutoTest
{
    public class Autorization
    {
        private AndroidDriver _driver;
        
        public Autorization (AndroidDriver driver)
        {
            _driver = driver;
        }
        
        public void AnonymousUserNavigatesToHomeScreen()
        {
            _driver.TapOn(Element.Button("OK, I understand"));
            _driver.TapOn(Element.Button("While using the app"));
            _driver.TapOn(Element.Button("Allow"));
            _driver.TapOn(Element.Button("CONTINUE"));
            _driver.TapOn(Element.Icon("Navigate up"));
            
            if (_driver.IsElementPresent(Element.CloseAlertIcon()))
                _driver.TapOn(Element.CloseAlertIcon());

            _driver.TapOn(Element.Text("Skip"));
            _driver.WaitFor(() =>_driver.IsElementPresent(Element.Text("Search for burgers, delivery, barbers on Yelp")));
        }
    }
}