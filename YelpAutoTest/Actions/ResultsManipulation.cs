using NUnit.Framework;
using OpenQA.Selenium.Appium.Android;

namespace YelpAutoTest.Actions
{
    public class ResultsManipulation(AndroidDriver driver)
    {
        private readonly int _borderOffset = 20;
        private readonly int _middleOfScreen = 100;
        
        public void RestaurantsAreFilteredByDistanceInResultsPanel()
        {
            ExpandSearchResultsArea();

            //get the search results panel and the quick filter panel coordinates for scrolling
            var resultsArea = driver.FindElement(Element.SearchResultPanel());
            var filterArea = driver.FindElement(Element.QuickFilterPanel());
            var upperYLimit = resultsArea.Location.Y + filterArea.Size.Height;
            var lowerYLimit = resultsArea.Location.Y + resultsArea.Size.Height;

            //skip the promoted items at the top of the search results
            SkipPromotedResultItems(lowerYLimit, upperYLimit);
            
            //parse the results and get the distances
            var restaurantsData = ParseRestaurantInfoWhileScrollingList(upperYLimit);
            
            var distances = new List<double>();
            foreach (var item in restaurantsData)
            {
                Console.WriteLine($"{item.Name} - {item.Distance}");
                distances.Add(ParseDistanceAndConvertItToMiles(item.Distance?? "0"));
            }
            
            Console.WriteLine("==============================================");
            Console.WriteLine("=================double check=================");

            // Print out the distances for debugging
            distances.ForEach(distance => Console.WriteLine(distance));
            
            // Assert that the distances are ordered in ascending order
            Assert.That(distances, Is.Ordered.Ascending);
        }

        private void ExpandSearchResultsArea()
        {
            driver.SwipeElementOnScreen(driver.FindElement(Element.SearchResultPanelDivider()), 300, 50);
        }

        private List<Restaurant> ParseRestaurantInfoWhileScrollingList(int scrollableResultAreaUpperYLimit)
        {
            List<Restaurant> restaurants = new List<Restaurant>();

            // Approximate number of iterations to scroll to the bottom of the search results
            for (int i = 0; i < 30; i++)
            {
                var searchItems = driver.FindElements(Element.SearchResultItemTile());

                if (searchItems.Any())
                {
                    // Scroll first found item to the top to avoid intersection with the previous one
                    driver.SwipeElementOnScreen(
                        searchItems.First().FindElement(Element.SearchResultTileTitle()),
                        _middleOfScreen, scrollableResultAreaUpperYLimit - _borderOffset
                    );

                    // Parse and add the restaurant details
                    var firstItem = searchItems.First();
                    restaurants.Add(new Restaurant
                    {
                        Name = firstItem.FindElement(Element.SearchResultTileTitle()).Text,
                        Distance = firstItem.FindElement(Element.SearchResultTileSecondaryLabel()).Text
                    });
                }

                // Scroll using the last tile divider to skip some promotions within the search results
                driver.SwipeElementOnScreen(
                    driver.FindElements(Element.SearchResultTileDivider()).Last(),
                    _middleOfScreen, scrollableResultAreaUpperYLimit - _borderOffset
                );

                // Break the loop if "Next 20 results" button is present
                if (driver.IsElementPresent(Element.Button("Next 20 results"))) break;
            }

            return restaurants;
        }
        
        private void SkipPromotedResultItems(int scrollableResultAreaLowerYLimit, int scrollableResultAreaUpperYLimit)
        {
            // Usually there are up to 3 promo cards at the top of the search results
            for (int i = 0; i < 5; i++)
            {
                // Scroll entire search result panel
                driver.SwipeScreenByCoordinates(_middleOfScreen, scrollableResultAreaLowerYLimit - _borderOffset, 
                    _middleOfScreen, scrollableResultAreaUpperYLimit);
               
                // Until All Results label is found and then scroll up carefully to not miss the first item
                if (driver.IsElementPresent(Element.Text("All Results")))
                {
                    driver.SwipeElementOnScreen(driver.FindElement(Element.Text("All Results")), 
                        _middleOfScreen, scrollableResultAreaUpperYLimit);
                    
                    break;
                }
            }
        }
        
        private double ParseDistanceAndConvertItToMiles(string distance)
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