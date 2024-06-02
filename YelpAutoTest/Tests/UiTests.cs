using NUnit.Framework;

namespace YelpAutoTest.Tests
{
    public class UiTests : BaseTest
    {
        [Test, Retry(2)]
        [Category("UI")]
        public void RestaurantsShouldBeSortedByDistanceAscIfDistanceFilterWasApplied()
        {
            app.Auth.AnonymousUserNavigatesToHomeScreen();
            app.Filter.UserFiltersRestaurantsByDistance();
            app.Results.RestaurantsAreFilteredByDistanceInResultsPanel();
        }
    }
}