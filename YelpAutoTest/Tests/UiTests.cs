using NUnit.Framework;

namespace YelpAutoTest
{
    public class UiTests : BaseTest
    {
        [Test]
        public void RestaurantsShouldBeSortedByDistanceAscIfDistanceFilterWasApplied()
        {
            auth.AnonymousUserNavigatesToHomeScreen();
            filter.UserFiltersRestaurantsByDistance();
            results.RestaurantsAreFilteredByDistanceInResultsPanel();
        }
    }
}