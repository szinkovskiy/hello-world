using NUnit.Framework;

namespace YelpAutoTest
{
    public class UiTests : BaseTest
    {
        [Test, Retry(2)]
        [Category("UI")]
        public void RestaurantsShouldBeSortedByDistanceAscIfDistanceFilterWasApplied()
        {
            auth.AnonymousUserNavigatesToHomeScreen();
            filter.UserFiltersRestaurantsByDistance();
            results.RestaurantsAreFilteredByDistanceInResultsPanel();
        }
    }
}