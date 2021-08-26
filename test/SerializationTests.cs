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
        Assert.AreEqual("{\"Aeons\":null,\"IsNegative\":false,\"IsPerpetual\":false,\"PlanckTime\":null,\"TotalNanoseconds\":0,\"TotalYoctoseconds\":0,\"Years\":0}", json);
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
        Assert.AreEqual("{\"Aeons\":null,\"IsNegative\":false,\"IsPerpetual\":false,\"PlanckTime\":\"14\",\"TotalNanoseconds\":183845006007008,\"TotalYoctoseconds\":9010011012013,\"Years\":1}", json);
        Assert.AreEqual(value, JsonSerializer.Deserialize<Duration>(json));

        var relativeValue = RelativeDuration.FromProportionOfDay(0.25m);

        json = JsonSerializer.Serialize(relativeValue);
        Assert.AreEqual("{\"Duration\":{\"Aeons\":null,\"IsNegative\":false,\"IsPerpetual\":false,\"PlanckTime\":null,\"TotalNanoseconds\":0,\"TotalYoctoseconds\":0,\"Years\":0},\"Proportion\":0.25,\"Relativity\":1}", json);
        Assert.AreEqual(relativeValue, JsonSerializer.Deserialize<RelativeDuration>(json));

        relativeValue = RelativeDuration.FromProportionOfYear(0.25m);

        json = JsonSerializer.Serialize(relativeValue);
        Assert.AreEqual("{\"Duration\":{\"Aeons\":null,\"IsNegative\":false,\"IsPerpetual\":false,\"PlanckTime\":null,\"TotalNanoseconds\":0,\"TotalYoctoseconds\":0,\"Years\":0},\"Proportion\":0.25,\"Relativity\":2}", json);
        Assert.AreEqual(relativeValue, JsonSerializer.Deserialize<RelativeDuration>(json));

        json = JsonSerializer.Serialize(Duration.PositiveInfinity);
        Assert.AreEqual("{\"Aeons\":null,\"IsNegative\":false,\"IsPerpetual\":true,\"PlanckTime\":null,\"TotalNanoseconds\":0,\"TotalYoctoseconds\":0,\"Years\":0}", json);
        Assert.IsTrue(JsonSerializer.Deserialize<Duration>(json).IsPositiveInfinity);

        json = JsonSerializer.Serialize(Duration.NegativeInfinity);
        Assert.AreEqual("{\"Aeons\":null,\"IsNegative\":true,\"IsPerpetual\":true,\"PlanckTime\":null,\"TotalNanoseconds\":0,\"TotalYoctoseconds\":0,\"Years\":0}", json);
        Assert.IsTrue(JsonSerializer.Deserialize<Duration>(json).IsNegativeInfinity);
    }
}
