using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tavenem.Time.Test;

[TestClass]
public class ConversionTests
{
    [TestMethod]
    public void FromSecondsTest()
    {
        _ = Duration.FromSeconds(0.5);
        _ = Duration.FromSeconds(0.957495426743473964);
        _ = Duration.FromSeconds(14245231.0050989573);
        _ = Duration.FromSeconds(5);
    }
}
