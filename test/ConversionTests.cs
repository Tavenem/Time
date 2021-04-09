using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tavenem.HugeNumbers;

namespace Tavenem.Time.Test
{
    [TestClass]
    public class ConversionTests
    {
        [TestMethod]
        public void FromSecondsTest()
        {
            _ = Duration.FromSeconds(HugeNumber.Zero);
            _ = Duration.FromSeconds(0.5);
            _ = Duration.FromSeconds(new HugeNumber(957495426743473964, -22));
            _ = Duration.FromSeconds(new HugeNumber(142452310050989573, -10));
            _ = Duration.FromSeconds(5);
        }
    }
}
