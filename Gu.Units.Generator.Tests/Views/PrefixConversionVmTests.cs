namespace Gu.Units.Generator.Tests.Views
{
    using NUnit.Framework;

    public class PrefixConversionVmTests
    {
        [Test]
        public void SetIsUsedToTrueAddsConversion()
        {
            var settings = new MockSettings();
            var prefixConversion = new PrefixConversion("Milliamperes", "mA", settings.Milli);
            var conversionVm = new PrefixConversionVm(settings.Amperes.PrefixConversions, settings.Amperes, settings.Milli);
            Assert.IsFalse(conversionVm.IsUsed);
            Assert.IsEmpty(settings.Amperes.PrefixConversions);

            conversionVm.IsUsed = true;
            var expected = new[] { prefixConversion };
            CollectionAssert.AreEqual(expected, settings.Amperes.PrefixConversions, PrefixConversionComparer.Default);
        }
    }
}