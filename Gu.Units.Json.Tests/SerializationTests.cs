namespace Gu.Units.Json.Tests
{
    using System.Globalization;
    using Newtonsoft.Json;
    using NUnit.Framework;

    public class SerializationTests
    {
        [Test]
        public void ToJsonDegrees()
        {
            var dummyClass = new DummyClass { Angle = Angle.FromDegrees(1.23) };
            JsonConvert.DefaultSettings = () => CreateSettings(AngleConverter.Degrees, "sv-SE");
            var actual = JsonConvert.SerializeObject(dummyClass);
            Assert.AreEqual("{\"Angle\":\"1,23°\"}", actual);
        }

        [Test]
        public void ToJsonRadians()
        {
            var dummyClass = new DummyClass { Angle = Angle.FromRadians(1.23) };
            JsonConvert.DefaultSettings = () => CreateSettings(AngleConverter.Radians, "en-US");
            var actual = JsonConvert.SerializeObject(dummyClass);
            Assert.AreEqual("{\"Angle\":\"1.23\u00A0rad\"}", actual);
        }

        [TestCase("{\"Angle\":\"1,23°\"}", "sv-SE")]
        [TestCase("{\"Angle\":\"1.23°\"}", "en-US")]
        public void FromJsonDefault(string json, string culture)
        {
            JsonConvert.DefaultSettings = () => CreateSettings(AngleConverter.Default, culture);
            var actual = JsonConvert.DeserializeObject<DummyClass>(json);
            Assert.AreEqual(Angle.FromDegrees(1.23), actual.Angle);
        }

        [TestCase("{\"Angle\":\"1,23°\"}", "sv-SE")]
        [TestCase("{\"Angle\":\"1.23°\"}", "en-US")]
        public void FromJsonDegrees(string json, string culture)
        {
            JsonConvert.DefaultSettings = () => CreateSettings(AngleConverter.Degrees, culture);
            var actual = JsonConvert.DeserializeObject<DummyClass>(json);
            Assert.AreEqual(Angle.FromDegrees(1.23), actual.Angle);
        }

        [TestCase("{\"Angle\":\"1,23°\"}", "sv-SE")]
        [TestCase("{\"Angle\":\"1.23°\"}", "en-US")]
        public void FromJsonRadians(string json, string culture)
        {
            JsonConvert.DefaultSettings = () => CreateSettings(AngleConverter.Radians, culture);
            var actual = JsonConvert.DeserializeObject<DummyClass>(json);
            Assert.AreEqual(Angle.FromDegrees(1.23), actual.Angle);
        }

        private JsonSerializerSettings CreateSettings(AngleConverter converter, string culture)
        {
            return new JsonSerializerSettings
            {
                Converters = new[] { converter },
                Culture = CultureInfo.GetCultureInfo(culture)
            };
        }
    }
}