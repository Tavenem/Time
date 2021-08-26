using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;

namespace Tavenem.Time.Test;

[TestClass]
public class FormatTests
{
    [TestMethod]
    public void ToStringTest()
    {
        var value = new Duration(
            false,
            null,
            1,
            2,
            3,
            4,
            5,
            6,
            7,
            8,
            9,
            10,
            11,
            12,
            13,
            14);

        Assert.AreEqual("1 2", value.ToString("d"));
        Assert.AreEqual("1 2", value.ToString("D"));
        Assert.AreEqual("1 2 03:04:05:006:007:008:009:010:011:012:013:14", value.ToString("E"));
        Assert.AreEqual("1 2 03:04", value.ToString("f"));
        Assert.AreEqual("1 2 03:04:05", value.ToString("F"));
        Assert.AreEqual("1 2 03:04", value.ToString("g"));
        Assert.AreEqual("1 2 03:04:05", value.ToString("G"));
        Assert.AreEqual("1-183845006007008:9010011012013:14", value.ToString("o"));
        Assert.AreEqual("03:04", value.ToString("t"));
        Assert.AreEqual("03:04:05", value.ToString("T"));
        Assert.AreEqual("1 y 2 d 3 h 4 min 5 s 6 ms 7 μs 8 ns 9 ps 10 fs 11 as 12 zs 13 ys 14 tP", value.ToString("X"));

        var relativeValue = RelativeDuration.FromProportionOfDay(0.25m);
        Assert.AreEqual("Dx0.25", relativeValue.ToString());

        relativeValue = RelativeDuration.FromProportionOfYear(0.25m);
        Assert.AreEqual("Yx0.25", relativeValue.ToString());

        value = Duration.Zero;

        Assert.AreEqual("0 0", value.ToString("d"));
        Assert.AreEqual("0 0", value.ToString("D"));
        Assert.AreEqual("0 0 00:00:00:000:000:000:000:000:000:000:000:0", value.ToString("E"));
        Assert.AreEqual("0 0 00:00", value.ToString("f"));
        Assert.AreEqual("0 0 00:00:00", value.ToString("F"));
        Assert.AreEqual("0 0 00:00", value.ToString("g"));
        Assert.AreEqual("0 0 00:00:00", value.ToString("G"));
        Assert.AreEqual("0-0:0:0", value.ToString("o"));
        Assert.AreEqual("00:00", value.ToString("t"));
        Assert.AreEqual("00:00:00", value.ToString("T"));
        Assert.AreEqual("0", value.ToString("X"));

        Assert.AreEqual(CultureInfo.InvariantCulture.NumberFormat.PositiveInfinitySymbol, Duration.PositiveInfinity.ToString(CultureInfo.InvariantCulture));
        Assert.AreEqual(CultureInfo.InvariantCulture.NumberFormat.NegativeInfinitySymbol, Duration.NegativeInfinity.ToString(CultureInfo.InvariantCulture));
    }

    [TestMethod]
    public void ParsingTest()
    {
        Assert.AreEqual(Duration.Zero, Duration.ParseExact(Duration.Zero.ToString("o"), "o"));

        var value = new Duration(
            false,
            null,
            1,
            2,
            3,
            4,
            5,
            6,
            7,
            8,
            9,
            10,
            11,
            12,
            13,
            14);
        Assert.AreEqual(value, Duration.ParseExact(value.ToString("o"), "o"));

        var relativeValue = RelativeDuration.FromProportionOfDay(0.25m);
        Assert.AreEqual(relativeValue, RelativeDuration.ParseExact(relativeValue.ToString(), "o"));

        relativeValue = RelativeDuration.FromProportionOfYear(0.25m);
        Assert.AreEqual(relativeValue, RelativeDuration.ParseExact(relativeValue.ToString(), "o"));

        Assert.AreEqual(Duration.PositiveInfinity, Duration.ParseExact(Duration.PositiveInfinity.ToString("o"), "o"));
        Assert.AreEqual(Duration.NegativeInfinity, Duration.ParseExact(Duration.NegativeInfinity.ToString("o"), "o"));
    }
}
