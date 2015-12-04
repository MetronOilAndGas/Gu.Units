namespace Gu.Units.Generator.Tests.Views
{
    using NUnit.Framework;

    public class PrefixConversionVmTests
    {
        [Test]
        public void SetIsUsedToTrueAddsConversion()
        {
            var settings = new MockSettings();
            var conversionVm = new PrefixConversionVm(settings.Milli, settings.Amperes);
            Assert.IsFalse(conversionVm.IsUsed);
            Assert.IsEmpty(settings.Amperes.Conversions);

            conversionVm.IsUsed = true;
            var actual = new[] { new Conversion("Milliamperes", "mA") };
            CollectionAssert.AreEqual(actual, settings.Amperes.Conversions, ConversionComparer.Default);
        }
    }
}