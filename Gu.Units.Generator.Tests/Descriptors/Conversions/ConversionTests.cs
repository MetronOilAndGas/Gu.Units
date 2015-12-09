namespace Gu.Units.Generator.Tests.Descriptors.Conversions
{
    using System;
    using System.Globalization;
    using NUnit.Framework;

    public class ConversionTests
    {
        [Test]
        public void IdentityConversion()
        {
            var settings = MockSettings.Create();
            var conversion = new PartConversion.IdentityConversion(settings.Metres);
            Assert.AreEqual("metres", conversion.ToSi);
            Assert.AreEqual("metres", conversion.FromSi);
            Assert.AreEqual("1 m = 1 m", conversion.SymbolConversion);
            Assert.AreEqual(true, conversion.CanRoundtrip);
        }

        [Test]
        public void FactorConversion()
        {
            var settings = MockSettings.Create();
            var conversion = new FactorConversion("Inches", "in", 0.0254);
            settings.Metres.FactorConversions.Add(conversion);
            Assert.AreEqual("0.0254*inches", conversion.ToSi);
            Assert.AreEqual("39.370078740157481*metres", conversion.FromSi);
            Assert.AreEqual("1 in = 0.0254 m", conversion.SymbolConversion);
            Assert.AreEqual(true, conversion.CanRoundtrip);
        }

        [Test]
        public void OffsetAndFactorConversion()
        {
            Console.WriteLine((1/1.8).ToString("R", CultureInfo.InvariantCulture));
            var settings = MockSettings.Create();
            var conversion = new OffsetConversion("Farenheit", "°F", 5.0/9, 459.67);
            settings.Kelvins.OffsetConversions.Add(conversion);
            Assert.AreEqual("0.55555555555555558*(farenheit + 459.67)", conversion.ToSi);
            Assert.AreEqual("1.7999999999999998*kelvin - 459.67", conversion.FromSi);
            Assert.AreEqual("1 °F = 255.927777777778 K", conversion.SymbolConversion);
            Assert.AreEqual(true, conversion.CanRoundtrip);
        }

        [Test]
        public void NestedFactorConversion()
        {
            var settings = MockSettings.Create();
            var conversion = PrefixConversion.Create(settings.Grams, settings.Milli);
            settings.Grams.PrefixConversions.Add(conversion);
            Assert.AreEqual("1E-06*milligrams", conversion.ToSi);
            Assert.AreEqual("1000000*kilograms", conversion.FromSi);
            Assert.AreEqual("1 mg = 1E-06 kg", conversion.SymbolConversion);
            Assert.AreEqual(true, conversion.CanRoundtrip);
        }

        [Test]
        public void PartConversionIdentity()
        {
            var settings = MockSettings.Create();
            var metresPart = PartConversion.CreatePart(1, settings.Metres);
            var secondsPart = PartConversion.CreatePart(-1, settings.Seconds);
            var conversion = PartConversion.Create(settings.MetresPerSecond, metresPart, secondsPart);
            settings.MetresPerSecond.PartConversions.Add(conversion);
            Assert.AreEqual("metresPerSecond", conversion.ToSi);
            Assert.AreEqual("metresPerSecond", conversion.FromSi);
            Assert.AreEqual("1 m/s = 1 m/s", conversion.SymbolConversion);
            Assert.AreEqual(true, conversion.CanRoundtrip);
        }

        [Test]
        public void PartConversionPrefix()
        {
            var settings = MockSettings.Create();
            var millimetreConversion = PrefixConversion.Create(settings.Metres, settings.Milli);
            settings.Metres.PrefixConversions.Add(millimetreConversion);
            var milliMetresPart = PartConversion.CreatePart(1, millimetreConversion);
            var secondsPart = PartConversion.CreatePart(-1, settings.Seconds);
            var conversion = PartConversion.Create(settings.MetresPerSecond, milliMetresPart, secondsPart);
            settings.MetresPerSecond.PartConversions.Add(conversion);
            Assert.AreEqual("0.001*millimetresPerSecond", conversion.ToSi);
            Assert.AreEqual("1000*metresPerSecond", conversion.FromSi);
            Assert.AreEqual("1 mm/s = 0.001 m/s", conversion.SymbolConversion);
            Assert.AreEqual(true, conversion.CanRoundtrip);
        }
    }
}
