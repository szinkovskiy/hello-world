using NUnit.Framework;

namespace YelpAutoTest.Tests
{
    public class BaseTest
    {
        protected AppManager app;

        [SetUp]
        public void Setup()
        {
            app = new AppManager();
        }

        [TearDown]
        public void TearDown()
        {
            app.Stop();
        }
    }
}