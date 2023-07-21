using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json;

namespace Tavenem.Time.Test;

[TestClass]
public class SerializationTests
{
    [TestMethod]
    public void CosmicTimeTest()
    {
        var value = new CosmicTime();
        var json = JsonSerializer.Serialize(value);
        var deserialized = JsonSerializer.Deserialize<CosmicTime>(json);
        Assert.IsNotNull(deserialized);
        Assert.AreEqual(value, deserialized);
        Assert.AreEqual(json, JsonSerializer.Serialize(deserialized));

        json = JsonSerializer.Serialize(value, TimeSourceGenerationContext.Default.CosmicTime);
        Console.WriteLine();
        Console.WriteLine(json);
        deserialized = JsonSerializer.Deserialize(json, TimeSourceGenerationContext.Default.CosmicTime);
        Assert.IsNotNull(deserialized);
        Assert.AreEqual(value, deserialized);
        Assert.AreEqual(
            json,
            JsonSerializer.Serialize(deserialized, TimeSourceGenerationContext.Default.CosmicTime));
    }

    [TestMethod]
    public void DurationTest()
    {
        var json = JsonSerializer.Serialize(Duration.Zero);
        Assert.AreEqual("\"0-0:0:0\"", json);
        var deserialized = JsonSerializer.Deserialize<Duration>(json);
        Assert.IsTrue(deserialized.IsZero);
        Assert.AreEqual(json, JsonSerializer.Serialize(deserialized));

        json = JsonSerializer.Serialize(Duration.Zero, TimeSourceGenerationContext.Default.Duration);
        Assert.AreEqual("\"0-0:0:0\"", json);
        deserialized = JsonSerializer.Deserialize(json, TimeSourceGenerationContext.Default.Duration);
        Assert.IsTrue(deserialized.IsZero);
        Assert.AreEqual(
            json,
            JsonSerializer.Serialize(deserialized, TimeSourceGenerationContext.Default.Duration));

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
        deserialized = JsonSerializer.Deserialize<Duration>(json);
        Assert.AreEqual(value, deserialized);
        Assert.AreEqual(json, JsonSerializer.Serialize(deserialized));

        json = JsonSerializer.Serialize(value, TimeSourceGenerationContext.Default.Duration);
        Assert.AreEqual("\"1-183845006007008:9010011012013:14\"", json);
        deserialized = JsonSerializer.Deserialize(json, TimeSourceGenerationContext.Default.Duration);
        Assert.AreEqual(value, deserialized);
        Assert.AreEqual(
            json,
            JsonSerializer.Serialize(deserialized, TimeSourceGenerationContext.Default.Duration));

        value = value.Negate();
        json = JsonSerializer.Serialize(value);
        Assert.AreEqual("\"-1-183845006007008:9010011012013:14\"", json);
        deserialized = JsonSerializer.Deserialize<Duration>(json);
        Assert.AreEqual(value, deserialized);
        Assert.AreEqual(json, JsonSerializer.Serialize(deserialized));

        json = JsonSerializer.Serialize(value, TimeSourceGenerationContext.Default.Duration);
        Assert.AreEqual("\"-1-183845006007008:9010011012013:14\"", json);
        deserialized = JsonSerializer.Deserialize(json, TimeSourceGenerationContext.Default.Duration);
        Assert.AreEqual(value, deserialized);
        Assert.AreEqual(
            json,
            JsonSerializer.Serialize(deserialized, TimeSourceGenerationContext.Default.Duration));

        json = JsonSerializer.Serialize(Duration.PositiveInfinity);
        Assert.AreEqual($"\"{System.Globalization.CultureInfo.InvariantCulture.NumberFormat.PositiveInfinitySymbol}\"", json);
        deserialized = JsonSerializer.Deserialize<Duration>(json);
        Assert.IsTrue(deserialized.IsPositiveInfinity);
        Assert.AreEqual(json, JsonSerializer.Serialize(deserialized));

        json = JsonSerializer.Serialize(Duration.PositiveInfinity, TimeSourceGenerationContext.Default.Duration);
        Assert.AreEqual($"\"{System.Globalization.CultureInfo.InvariantCulture.NumberFormat.PositiveInfinitySymbol}\"", json);
        deserialized = JsonSerializer.Deserialize(json, TimeSourceGenerationContext.Default.Duration);
        Assert.IsTrue(deserialized.IsPositiveInfinity);
        Assert.AreEqual(
            json,
            JsonSerializer.Serialize(deserialized, TimeSourceGenerationContext.Default.Duration));

        json = JsonSerializer.Serialize(Duration.NegativeInfinity);
        Assert.AreEqual($"\"{System.Globalization.CultureInfo.InvariantCulture.NumberFormat.NegativeInfinitySymbol}\"", json);
        deserialized = JsonSerializer.Deserialize<Duration>(json);
        Assert.IsTrue(deserialized.IsNegativeInfinity);
        Assert.AreEqual(json, JsonSerializer.Serialize(deserialized));

        json = JsonSerializer.Serialize(Duration.NegativeInfinity, TimeSourceGenerationContext.Default.Duration);
        Assert.AreEqual($"\"{System.Globalization.CultureInfo.InvariantCulture.NumberFormat.NegativeInfinitySymbol}\"", json);
        deserialized = JsonSerializer.Deserialize(json, TimeSourceGenerationContext.Default.Duration);
        Assert.IsTrue(deserialized.IsNegativeInfinity);
        Assert.AreEqual(
            json,
            JsonSerializer.Serialize(deserialized, TimeSourceGenerationContext.Default.Duration));
    }

    [TestMethod]
    public void RelativeDurationTest()
    {
        var value = RelativeDuration.FromProportionOfDay(0.25m);

        var json = JsonSerializer.Serialize(value);
        Assert.AreEqual("\"Dx0.25\"", json);
        Assert.AreEqual(value, JsonSerializer.Deserialize<RelativeDuration>(json));
        var deserialized = JsonSerializer.Deserialize<RelativeDuration>(json);
        Assert.AreEqual(value, deserialized);
        Assert.AreEqual(json, JsonSerializer.Serialize(deserialized));

        json = JsonSerializer.Serialize(value, TimeSourceGenerationContext.Default.RelativeDuration);
        Assert.AreEqual("\"Dx0.25\"", json);
        deserialized = JsonSerializer.Deserialize(json, TimeSourceGenerationContext.Default.RelativeDuration);
        Assert.AreEqual(value, deserialized);
        Assert.AreEqual(
            json,
            JsonSerializer.Serialize(deserialized, TimeSourceGenerationContext.Default.RelativeDuration));

        value = RelativeDuration.FromProportionOfYear(0.25m);

        json = JsonSerializer.Serialize(value);
        Assert.AreEqual("\"Yx0.25\"", json);
        deserialized = JsonSerializer.Deserialize<RelativeDuration>(json);
        Assert.AreEqual(value, deserialized);
        Assert.AreEqual(json, JsonSerializer.Serialize(deserialized));

        json = JsonSerializer.Serialize(value, TimeSourceGenerationContext.Default.RelativeDuration);
        Assert.AreEqual("\"Yx0.25\"", json);
        deserialized = JsonSerializer.Deserialize(json, TimeSourceGenerationContext.Default.RelativeDuration);
        Assert.AreEqual(value, deserialized);
        Assert.AreEqual(
            json,
            JsonSerializer.Serialize(deserialized, TimeSourceGenerationContext.Default.RelativeDuration));
    }
}
