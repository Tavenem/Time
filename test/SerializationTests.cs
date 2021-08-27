using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json;

namespace Tavenem.Time.Test;

[TestClass]
public class SerializationTests
{
    [TestMethod]
    public void CosmicTimeTest()
    {
        var item = new CosmicTime();
        var json = JsonSerializer.Serialize(item);

        var deserialized = JsonSerializer.Deserialize<CosmicTime>(json);
        Assert.IsNotNull(deserialized);

        Assert.AreEqual(item.CurrentEpoch, deserialized.CurrentEpoch);

        CollectionAssert.AreEquivalent(item.Epochs.ToList(), deserialized.Epochs.ToList());

        Assert.AreEqual(item.Now, deserialized.Now);

        var reserialized = JsonSerializer.Serialize(deserialized);
        Assert.AreEqual(json, reserialized);
    }

    [TestMethod]
    public void JsonSerializationTest()
    {
        var json = JsonSerializer.Serialize(Duration.Zero);
        Assert.AreEqual("\"0-0:0:0\"", json);
        var result = JsonSerializer.Deserialize<Duration>(json);
        Assert.IsTrue(result.IsZero);

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

        json = JsonSerializer.Serialize(value);
        Assert.AreEqual("\"1-183845006007008:9010011012013:14\"", json);
        Assert.AreEqual(value, JsonSerializer.Deserialize<Duration>(json));

        value = value.Negate();
        json = JsonSerializer.Serialize(value);
        Assert.AreEqual("\"-1-183845006007008:9010011012013:14\"", json);
        Assert.AreEqual(value, JsonSerializer.Deserialize<Duration>(json));

        var relativeValue = RelativeDuration.FromProportionOfDay(0.25m);

        json = JsonSerializer.Serialize(relativeValue);
        Assert.AreEqual("\"Dx0.25\"", json);
        Assert.AreEqual(relativeValue, JsonSerializer.Deserialize<RelativeDuration>(json));

        relativeValue = RelativeDuration.FromProportionOfYear(0.25m);

        json = JsonSerializer.Serialize(relativeValue);
        Assert.AreEqual("\"Yx0.25\"", json);
        Assert.AreEqual(relativeValue, JsonSerializer.Deserialize<RelativeDuration>(json));

        json = JsonSerializer.Serialize(Duration.PositiveInfinity);
        Assert.AreEqual($"\"{System.Globalization.CultureInfo.InvariantCulture.NumberFormat.PositiveInfinitySymbol}\"", json);
        Assert.IsTrue(JsonSerializer.Deserialize<Duration>(json).IsPositiveInfinity);

        json = JsonSerializer.Serialize(Duration.NegativeInfinity);
        Assert.AreEqual($"\"{System.Globalization.CultureInfo.InvariantCulture.NumberFormat.NegativeInfinitySymbol}\"", json);
        Assert.IsTrue(JsonSerializer.Deserialize<Duration>(json).IsNegativeInfinity);
    }
}
