using OpenQA.Selenium.Appium.Android;

namespace YelpAutoTest
{
    public class Filtering
    {
        private AndroidDriver _driver;
        
        public Filtering (AndroidDriver driver)
        {
            _driver = driver;
        }
        
        public void UserFiltersRestaurantsByDistance()
        {
            _driver.TapOn(Element.Text("Restaurants"));
            _driver.TapOn(Element.Text("Sort"));
            _driver.TapOn(Element.Radio("Distance"));
        }
    }
}