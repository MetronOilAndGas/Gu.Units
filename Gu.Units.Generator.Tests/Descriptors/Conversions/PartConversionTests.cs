namespace Gu.Units.Generator.Tests.Descriptors.Conversions
{
    using NUnit.Framework;

    public class PartConversionTests
    {
        [Test]
        public void GramsPerCubicMetre()
        {
            var settings = MockSettings.Create();
            var gramPart = PartConversion.CreatePart(1, settings.Grams);
            var volumePart = PartConversion.CreatePart(-1, settings.CubicMetres);
            var conversion = PartConversion.Create(gramPart, volumePart);
            Assert.AreEqual("g/m^3", conversion.Symbol);
            Assert.AreEqual("GramsPerCubicMetre", conversion.Name);
            Assert.AreEqual(1E-3, conversion.Factor);
        }

        [Test]
        public void MilliGramsPerCubicMetre()
        {
            var settings = MockSettings.Create();
            var milligram = PrefixConversion.Create(settings.Grams, settings.Milli);
            settings.Grams.PrefixConversions.Add(milligram);
            var milliGramPart = PartConversion.CreatePart(1, milligram);
            var volumePart = PartConversion.CreatePart(-1, settings.CubicMetres);
            var conversion = PartConversion.Create(milliGramPart, volumePart);
            Assert.AreEqual("mg/m^3", conversion.Symbol);
            Assert.AreEqual("MilligramsPerCubicMetre", conversion.Name);
            Assert.AreEqual(1E-6, conversion.Factor);
        }
    }
}