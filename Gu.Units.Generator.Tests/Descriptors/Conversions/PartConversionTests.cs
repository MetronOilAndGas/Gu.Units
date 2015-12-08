namespace Gu.Units.Generator.Tests.Descriptors.Conversions
{
    using NUnit.Framework;

    public class PartConversionTests
    {
        private const string Superscripts = "⁺⁻⁰¹²³⁴⁵⁶⁷⁸⁹";
        private const char MultiplyDot = '⋅';

        [Test]
        public void SquareMillimetres()
        {
            var settings = MockSettings.Create();
            var millimetres = PrefixConversion.Create(settings.Metres, settings.Milli);
            settings.Metres.PrefixConversions.Add(millimetres);
            var millimetrePart = PartConversion.CreatePart(2, millimetres);
            var conversion = PartConversion.Create(millimetrePart);
            Assert.AreEqual("mm²", conversion.Symbol);
            Assert.AreEqual("SquareMillimetres", conversion.Name);
            Assert.AreEqual(1E-6, conversion.Factor);
        }

        [Test]
        public void GramsPerCubicMetre()
        {
            var settings = MockSettings.Create();
            var gramPart = PartConversion.CreatePart(1, settings.Grams);
            var volumePart = PartConversion.CreatePart(-1, settings.CubicMetres);
            var conversion = PartConversion.Create(gramPart, volumePart);
            Assert.AreEqual("g/m³", conversion.Symbol);
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
            Assert.AreEqual("mg/m³", conversion.Symbol);
            Assert.AreEqual("MilligramsPerCubicMetre", conversion.Name);
            Assert.AreEqual(1E-6, conversion.Factor);
        }
    }
}