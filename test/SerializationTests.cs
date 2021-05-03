using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using System.Linq;

namespace Tavenem.Time.Test
{
    [TestClass]
    public class SerializationTests
    {
        [TestMethod]
        public void CosmicTimeTest()
        {
            var item = new CosmicTime();
            var json = System.Text.Json.JsonSerializer.Serialize(item);

            var deserialized = System.Text.Json.JsonSerializer.Deserialize<CosmicTime>(json);
            Assert.IsNotNull(deserialized);

            Assert.AreEqual(item.CurrentEpoch, deserialized.CurrentEpoch);

            CollectionAssert.AreEquivalent(item.Epochs.ToList(), deserialized.Epochs.ToList());

            Assert.AreEqual(item.Now, deserialized.Now);

            var reserialized = System.Text.Json.JsonSerializer.Serialize(deserialized);
            Assert.AreEqual(json, reserialized);
        }

        [TestMethod]
        public void JsonSerializationTest()
        {
            var json = System.Text.Json.JsonSerializer.Serialize(Duration.Zero);
            Assert.AreEqual("{\"AeonSequence\":null,\"IsNegative\":false,\"IsPerpetual\":false,\"PlanckTime\":0,\"TotalNanoseconds\":0,\"TotalYoctoseconds\":0,\"Years\":0}", json);
            Assert.IsTrue(System.Text.Json.JsonSerializer.Deserialize<Duration>(json).IsZero);

            var value = new Duration(false, null, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14.15m);

            json = System.Text.Json.JsonSerializer.Serialize(value);
            Assert.AreEqual("{\"AeonSequence\":null,\"IsNegative\":false,\"IsPerpetual\":false,\"PlanckTime\":14.15,\"TotalNanoseconds\":183845006007008,\"TotalYoctoseconds\":9010011012013,\"Years\":1}", json);
            Assert.AreEqual(value, System.Text.Json.JsonSerializer.Deserialize<Duration>(json));

            var relativeValue = RelativeDuration.FromProportionOfDay(0.25m);

            json = System.Text.Json.JsonSerializer.Serialize(relativeValue);
            Assert.AreEqual("{\"Duration\":{\"AeonSequence\":null,\"IsNegative\":false,\"IsPerpetual\":false,\"PlanckTime\":0,\"TotalNanoseconds\":0,\"TotalYoctoseconds\":0,\"Years\":0},\"Proportion\":0.25,\"Relativity\":1}", json);
            Assert.AreEqual(relativeValue, System.Text.Json.JsonSerializer.Deserialize<RelativeDuration>(json));

            relativeValue = RelativeDuration.FromProportionOfYear(0.25m);

            json = System.Text.Json.JsonSerializer.Serialize(relativeValue);
            Assert.AreEqual("{\"Duration\":{\"AeonSequence\":null,\"IsNegative\":false,\"IsPerpetual\":false,\"PlanckTime\":0,\"TotalNanoseconds\":0,\"TotalYoctoseconds\":0,\"Years\":0},\"Proportion\":0.25,\"Relativity\":2}", json);
            Assert.AreEqual(relativeValue, System.Text.Json.JsonSerializer.Deserialize<RelativeDuration>(json));

            json = System.Text.Json.JsonSerializer.Serialize(Duration.PositiveInfinity);
            Assert.AreEqual("{\"AeonSequence\":null,\"IsNegative\":false,\"IsPerpetual\":true,\"PlanckTime\":0,\"TotalNanoseconds\":0,\"TotalYoctoseconds\":0,\"Years\":0}", json);
            Assert.IsTrue(System.Text.Json.JsonSerializer.Deserialize<Duration>(json).IsPositiveInfinity);

            json = System.Text.Json.JsonSerializer.Serialize(Duration.NegativeInfinity);
            Assert.AreEqual("{\"AeonSequence\":null,\"IsNegative\":true,\"IsPerpetual\":true,\"PlanckTime\":0,\"TotalNanoseconds\":0,\"TotalYoctoseconds\":0,\"Years\":0}", json);
            Assert.IsTrue(System.Text.Json.JsonSerializer.Deserialize<Duration>(json).IsNegativeInfinity);
        }
    }
}
