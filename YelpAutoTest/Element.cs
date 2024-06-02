using OpenQA.Selenium;

namespace YelpAutoTest
{
    public static class Element{

        public static By Button(string title)
        {
            return By.XPath($"//android.widget.Button[@text='{title}']");
        }
        
        public static By Icon(string title)
        {
            return By.XPath($"//android.widget.ImageButton[@content-desc='{title}']");
        }
        
        public static By Text(string title)
        {
            return By.XPath($"//android.widget.TextView[@text='{title}']");
        }

        public static By Radio(string label)
        {
            return By.XPath($"//android.widget.TextView[@resource-id='com.yelp.android:id/group_radiobutton_text' and @text='{label}']/following-sibling :: android.view.View[@resource-id='com.yelp.android:id/group_radiobutton']");
        }

        public static By QuickFilterPanel()
        {
            return By.XPath(
                "//androidx.recyclerview.widget.RecyclerView[@resource-id='com.yelp.android:id/search_tag_filters']");
        }
        
        public static By SearchResultPanel()
        {
            return By.XPath("//android.widget.GridView[@resource-id='com.yelp.android:id/search_list_recycler_view']");
        }
        
        public static By SearchResultPanelDivider()
        {
            return By.XPath("//android.view.View[@resource-id='com.yelp.android:id/divider']");
        }
        
        public static By SearchResultItemTile()
        {
            return By.XPath("//android.view.ViewGroup[@resource-id='com.yelp.android:id/business_passport']");
        }
        
        public static By SearchResultTileTitle()
        {
            return By.XPath("//android.widget.TextView[@resource-id='com.yelp.android:id/business_name']");
        }
        
        public static By SearchResultTileSecondaryLabel()
        {
            return By.XPath("//android.widget.TextView[@resource-id='com.yelp.android:id/secondary_label']");
        }
        
        public static By SearchResultTileDivider()
        {
            return By.XPath("//android.widget.FrameLayout[@resource-id='com.yelp.android:id/search_list_divider_container']");
        }
        
        public static By CloseAlertIcon()
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));
            return By.XPath($"//android.view.View[@content-desc='Dismiss']/following-sibling::*");
        }
    }
}