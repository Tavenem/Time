using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;

namespace Tavenem.Time.Test
{
    [TestClass]
    public class SerializationTests
    {
        [TestMethod]
        public void JsonSerializationTest()
        {
            var json = System.Text.Json.JsonSerializer.Serialize(Duration.Zero);
            Assert.AreEqual("\"0-0:0:0\"", json);
            Assert.IsTrue(System.Text.Json.JsonSerializer.Deserialize<Duration>(json).IsZero);

            var value = new Duration(false, null, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14.15m);

            json = System.Text.Json.JsonSerializer.Serialize(value);
            Assert.AreEqual("\"1-183845006007008:9010011012013:14.15\"", json);
            Assert.AreEqual(value, System.Text.Json.JsonSerializer.Deserialize<Duration>(json));

            var relativeValue = RelativeDuration.FromProportionOfDay(0.25m);

            json = System.Text.Json.JsonSerializer.Serialize(relativeValue);
            Assert.AreEqual("\"Dx0.25\"", json);
            Assert.AreEqual(relativeValue, System.Text.Json.JsonSerializer.Deserialize<RelativeDuration>(json));

            relativeValue = RelativeDuration.FromProportionOfYear(0.25m);

            json = System.Text.Json.JsonSerializer.Serialize(relativeValue);
            Assert.AreEqual("\"Yx0.25\"", json);
            Assert.AreEqual(relativeValue, System.Text.Json.JsonSerializer.Deserialize<RelativeDuration>(json));

            json = System.Text.Json.JsonSerializer.Serialize(Duration.PositiveInfinity);
            Assert.AreEqual($"\"{NumberFormatInfo.InvariantInfo.PositiveInfinitySymbol}\"", json);
            Assert.IsTrue(System.Text.Json.JsonSerializer.Deserialize<Duration>(json).IsPositiveInfinity);

            json = System.Text.Json.JsonSerializer.Serialize(Duration.NegativeInfinity);
            Assert.AreEqual($"\"{NumberFormatInfo.InvariantInfo.NegativeInfinitySymbol}\"", json);
            Assert.IsTrue(System.Text.Json.JsonSerializer.Deserialize<Duration>(json).IsNegativeInfinity);
        }
    }
}
