using OpenQA.Selenium.Appium.Android;

namespace YelpAutoTest.Actions
{
    public class Authorization(AndroidDriver driver)
    {
        public void AnonymousUserNavigatesToHomeScreen()
        {
            driver.TapOn(Element.Button("OK, I understand"));
            driver.TapOn(Element.Button("While using the app"));
            driver.TapOn(Element.Button("Allow"));
            driver.TapOn(Element.Button("CONTINUE"));
            driver.TapOn(Element.Icon("Navigate up"));
            
            if (driver.IsElementPresent(Element.CloseAlertIcon()))
                driver.TapOn(Element.CloseAlertIcon());

            driver.TapOn(Element.Text("Skip"));
            driver.WaitFor(() =>driver.IsElementPresent(Element.Text("Search for burgers, delivery, barbers on Yelp")));
        }
    }
}