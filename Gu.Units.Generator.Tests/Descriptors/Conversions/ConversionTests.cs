namespace Gu.Units.Generator.Tests.Descriptors.Conversions
{
    using NUnit.Framework;

    public class ConversionTests
    {
        [Test]
        public void IdentityConversion()
        {
            var settings = MockSettings.Create();
            var conversion = new PartConversion.IdentityConversion(settings.Metres);
            Assert.AreEqual("Metres", conversion.ToSi);
            Assert.AreEqual("Metres", conversion.FromSi);
            Assert.IsTrue(conversion.CanRoundtrip);
        }

        [Test]
        public void FactorConversion()
        {
            var settings = MockSettings.Create();
            var conversion = new FactorConversion("Inches", "in", 0.254);
            settings.Metres.FactorConversions.Add(conversion);
            Assert.AreEqual("0.0254*Metres", conversion.ToSi);
            Assert.AreEqual("Metres/1000", conversion.FromSi);
        }

        [Test]
        public void OffsetAndFactorConversion()
        {
            Assert.Fail();
            //var settings = new MockSettings();
            //var conversionFormula = new ConversionFormula(settings.Kelvins)
            //{
            //    ConversionFactor = 1.8,
            //    Offset = -459.67
            //};
            //Assert.AreEqual("1.8*Kelvin - 459.67", conversionFormula.ToSi);
            //Assert.AreEqual("Kelvin/1.8 + 459.67", conversionFormula.FromSi);
        }

        [Test]
        public void NestedFactorConversion()
        {
            Assert.Fail();
            //var settings = new MockSettings();
            //var conversionFormula = new ConversionFormula(settings.Grams)
            //{
            //    ConversionFactor = 0.001
            //};

            //Assert.AreEqual("1E-06*Kilograms", conversionFormula.ToSi);
            //Assert.AreEqual("Kilograms/1E-06", conversionFormula.FromSi);
        }
    }
}
