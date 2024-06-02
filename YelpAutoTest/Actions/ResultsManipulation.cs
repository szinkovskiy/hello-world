using NUnit.Framework;
using OpenQA.Selenium.Appium.Android;

namespace YelpAutoTest
{
    public class ResultsManipulation
    {
        private AndroidDriver _driver;
        private int borderOffset = 20;
        private int midleOfScreen = 100;
        
        public ResultsManipulation (AndroidDriver driver)
        {
            _driver = driver;
        }
        
        public void RestaurantsAreFilteredByDistanceInResultsPanel()
        {
            //expand the search results panel
           _driver.SwipeElementOnScreen(_driver.FindElement(Element.SearchResultPanelDivider()), 300, 50);
           
            //get the search results panel and the quick filter panel coordinates for scrolling
            var resultsArea = _driver.FindElement(Element.SearchResultPanel());
            var filterArea = _driver.FindElement(Element.QuickFilterPanel());
            var upperYLimit = resultsArea.Location.Y + filterArea.Size.Height;
            var lowerYLimit = resultsArea.Location.Y + resultsArea.Size.Height;

            //skip the promoted items at the top of the search results
            SkipPromotedResultItems(lowerYLimit, upperYLimit);
            
            //parse the results and get the distances
            var restaurantsData = ParceResultsItems(upperYLimit);
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

        private List<Restaurant> ParceResultsItems(int scrollableResultAreaUpperYLimit)
        {
            List<Restaurant> restaurants = new List<Restaurant>();

            // Approximate number of iterations to scroll to the bottom of the search results
            for (int i = 0; i < 30; i++)
            {
                var searchItems = _driver.FindElements(Element.SearchResultItemTile());

                if (searchItems.Any())
                {
                    // Scroll first found item to the top to avoid intersection with the previous one
                    _driver.SwipeElementOnScreen(
                        searchItems.First().FindElement(Element.SearchResultTileTitle()),
                        midleOfScreen, scrollableResultAreaUpperYLimit - borderOffset
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
                _driver.SwipeElementOnScreen(
                    _driver.FindElements(Element.SearchResultTileDivider()).Last(),
                    midleOfScreen, scrollableResultAreaUpperYLimit - borderOffset
                );

                // Break the loop if "Next 20 results" button is present
                if (_driver.IsElementPresent(Element.Button("Next 20 results"))) break;
            }

            return restaurants;
        }
        
        private void SkipPromotedResultItems(int scrollableResultAreaLowerYLimit, int scrollableResultAreaUpperYLimit)
        {
            // Usually there are up to 3 promo cards at the top of the search results
            for (int i = 0; i < 5; i++)
            {
                // Scroll entire search result panel
                _driver.SwipeScreenByCoordinates(midleOfScreen, scrollableResultAreaLowerYLimit - borderOffset, 
                    midleOfScreen, scrollableResultAreaUpperYLimit);
               
                // Until All Results label is found and then scroll up carefully to not miss the first item
                if (_driver.IsElementPresent(Element.Text("All Results")))
                {
                    _driver.SwipeElementOnScreen(_driver.FindElement(Element.Text("All Results")), 
                        midleOfScreen, scrollableResultAreaUpperYLimit);
                    
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