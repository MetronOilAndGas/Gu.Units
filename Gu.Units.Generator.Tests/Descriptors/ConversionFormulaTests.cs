namespace Gu.Units.Generator.Tests.Descriptors
{
    using NUnit.Framework;

    public class ConversionFormulaTests
    {
        [Test]
        public void TrivialConversion()
        {
            var settings = new MockSettings();
            var conversionFormula = new ConversionFormula(settings.Metres)
            {
                ConversionFactor = 1
            };
            Assert.AreEqual("Metres", conversionFormula.ToSi);
            Assert.AreEqual("Metres", conversionFormula.FromSi);
        }

        [Test]
        public void FactorConversion()
        {
            var settings = new MockSettings();
            var conversionFormula = new ConversionFormula(settings.Metres)
            {
                ConversionFactor = 1000
            };
            Assert.AreEqual("1000*Metres", conversionFormula.ToSi);
            Assert.AreEqual("Metres/1000", conversionFormula.FromSi);
        }

        [Test]
        public void OffsetAndFactorConversion()
        {
            var settings = new MockSettings();
            var conversionFormula = new ConversionFormula(settings.Kelvins)
            {
                ConversionFactor = 1.8,
                Offset = -459.67
            };
            Assert.AreEqual("1.8*Kelvin - 459.67", conversionFormula.ToSi);
            Assert.AreEqual("Kelvin/1.8 + 459.67", conversionFormula.FromSi);
        }


        [Test]
        public void NestedFactorConversion()
        {
            var settings = new MockSettings();
            var conversionFormula = new ConversionFormula(settings.Grams)
            {
                ConversionFactor = 0.001
            };

            Assert.AreEqual("1E-06*Kilograms", conversionFormula.ToSi);
            Assert.AreEqual("Kilograms/1E-06", conversionFormula.FromSi);
        }

    }
}
