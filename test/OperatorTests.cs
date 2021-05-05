using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tavenem.Time.Test
{
    [TestClass]
    public class OperatorTests
    {
        [TestMethod]
        public void AdditionTests()
        {
            Assert.AreEqual(
                new Duration(seconds: 2),
                Duration.OneSecond + Duration.OneSecond);

            Duration? nullable1 = Duration.OneSecond;
            Duration? nullable2 = null;
            nullable1 += nullable2;
            Assert.IsNotNull(nullable1);
            Assert.AreEqual(Duration.OneSecond, nullable1);

            Assert.AreEqual(
                Duration.OneSecond,
                Duration.OneSecond + nullable2);

            Assert.AreEqual(
                Duration.OneSecond,
                nullable2 + Duration.OneSecond);

            Assert.IsNull(nullable2 + nullable2);
        }
    }
}
