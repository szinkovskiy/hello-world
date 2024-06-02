using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;

namespace YelpAutoTest
{
    public class MobileDriver
    {
        protected AndroidDriver _driver;

        public AndroidDriver RunApp()
        {
            var serverUri = new Uri("http://127.0.0.1:4723");

            var driverOptions = new AppiumOptions()
            {
                AutomationName = AutomationName.AndroidUIAutomator2,
                PlatformName = "Android",
                DeviceName = "testArm", // emulator id
            };

            driverOptions.App =
                "/Users/serhiizinkivskyi/Desktop/com.yelp.android_v24.21.0-28242113-28242113_Android-9.0.apk";
            driverOptions.AddAdditionalAppiumOption("appPackage", "com.yelp.android");
            driverOptions.AddAdditionalAppiumOption("appActivity", "com.yelp.android.home.ui.RootSingleActivity");
            driverOptions.AddAdditionalAppiumOption("fullReset", true);

            return new AndroidDriver(serverUri, driverOptions, TimeSpan.FromSeconds(90));
        }
    }

    static class MobileDriverExtensions
    {

        public static void TapOn(this AndroidDriver driver, By by)
        {
            int attempts = 5;

            for (int i = 0; i < attempts; i++)
            {
                if (driver.FindElements(by).Count > 0)
                {
                    new Actions(driver)
                        .MoveToElement(driver.FindElement(by))
                        .Click()
                        .Perform();
                    break;
                }
                else
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }

                if (i == attempts - 1)
                {
                    throw new NoSuchElementException($"Element not found! Locator: {by}");
                }
            }
        }

        public static bool IsElementPresent(this AndroidDriver driver, By by)
        {
            return driver.FindElements(by).Count == 0 ? false : true;
        }
        
        public static void WaitFor(this AndroidDriver driver, Func<bool> flag)
        {
            for (int i = 0; i < 30; i++)
            {
                if (i.Equals(29))
                    throw new Exception("WAIT FOR condition finishes with FALSE state");

                if (flag())
                    break;

                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }
        
        public static void SwipeElementUp(this AndroidDriver driver, By by)
        {
            new Actions(driver)
                .MoveToElement(driver.FindElement(by))
                .Pause(TimeSpan.FromSeconds(1))
                .ClickAndHold()
                .Pause(TimeSpan.FromSeconds(1))
                .MoveToLocation(300, 50)
                .Perform();
        }
    }
}