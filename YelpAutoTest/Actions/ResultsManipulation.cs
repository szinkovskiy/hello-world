using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;

namespace YelpAutoTest
{
    public class ResultsManipulation
    {
        private AndroidDriver _driver;
        private int borderOffset = 20;
        
        public ResultsManipulation (AndroidDriver driver)
        {
            _driver = driver;
        }
        
        public void RestaurantsAreFilteredByDistanceInResultsPanel()
        {
            //expand the search results panel
           _driver.SwipeElementUp(Element.SearchResultPanelDivider());
           
            //get the search results panel and the quick filter panel coordinates for scrolling
            var resultsArea = _driver.FindElement(Element.SearchResultPanel());
            var filterArea = _driver.FindElement(Element.QuickFilterPanel());
            var scrollableResultAreaUpperYLimit = resultsArea.Location.Y + filterArea.Size.Height;
            var scrollableResultAreaLowerYLimit = resultsArea.Location.Y + resultsArea.Size.Height;

            //skip the promoted items at the top of the search results
            SkipPromotedItems(scrollableResultAreaLowerYLimit, scrollableResultAreaUpperYLimit);
            
            //parse the results and get the distances
            var restaurantsData = ParseResults(scrollableResultAreaUpperYLimit);
            var distances = new List<double>();
            foreach (var item in restaurantsData)
            {
                Console.WriteLine($"{item.Name} - {item.Distance}");
                distances.Add(ParseDistance(item.Distance));
            }
            
            Console.WriteLine("==============================================\r\n=================double check=================" );

            // Print out the distances for debugging
            foreach (var dis in distances)
            {
                Console.WriteLine(dis);
            }
            
            // Assert that the distances are ordered in ascending order
            Assert.That(distances, Is.Ordered.Ascending);
        }

        private List<Restaurant> ParseResults(int scrollableResultAreaUpperYLimit)
        {
            List<Restaurant> restaurants = new List<Restaurant>();

            // 30 is an approximate number of iterations to scroll to the bottom of the search results
            for (int i = 0; i < 30; i++)
            {
                var searchItems = _driver.FindElements(Element.SearchResultItemTile());

                if (searchItems.Count > 0)
                {
                    // Scroll found item to the top to avoid intersection with the previous one
                    new Actions(_driver)
                        .MoveToElement(_driver.FindElements(Element.SearchResultTileTitle()).First())
                        .ClickAndHold()
                        .MoveToLocation(100, scrollableResultAreaUpperYLimit - borderOffset)
                        .Perform();

                    // Parse the restaurant name and distance
                    restaurants.Add(new Restaurant()
                    {
                        Name = searchItems.First().FindElement(Element.SearchResultTileTitle()).Text,
                        Distance = searchItems.First().FindElement(Element.SearchResultTileSecondaryLabel()).Text
                    });
                }

                // Scroll using tiles divider. Needed to skip some promotions within the search results
                new Actions(_driver)
                    .MoveToElement(_driver.FindElements(Element.SearchResultTileDivider()).Last())
                    .ClickAndHold()
                    .MoveToLocation(100, scrollableResultAreaUpperYLimit - borderOffset)
                    .Perform();

                if (_driver.IsElementPresent(Element.Button("Next 20 results"))) break;
            }

            return restaurants;
        }

        private void SkipPromotedItems(int scrollableResultAreaLowerYLimit, int scrollableResultAreaUpperYLimit)
        {
            for (int i = 0; i < 5; i++)
            {
                // Scroll entire search result panel
                new Actions(_driver)
                    .MoveToLocation(100, scrollableResultAreaLowerYLimit - borderOffset)
                    .ClickAndHold()
                    .MoveToLocation(100, scrollableResultAreaUpperYLimit)
                    .Perform();
                
                if (_driver.IsElementPresent(Element.Text("All Results")))
                {
                    //until All Results label is found and then scroll up carefully to not miss the first item
                    new Actions(_driver)
                        .MoveToElement(_driver.FindElement(Element.Text("All Results")))
                        .ClickAndHold()
                        .MoveToLocation(100, scrollableResultAreaUpperYLimit)
                        .Perform();

                    break;
                }
            }
        }


        private double ParseDistance(string distance)
        {
            const double feetToMiles = 0.0001894;

            if (distance.Contains("ft"))
            {
                return double.Parse(distance.Replace("ft", "").Trim()) * feetToMiles;
            }
            else
            {
                return double.Parse(distance.Replace("mi", "").Trim());
            }
        }
    }
}