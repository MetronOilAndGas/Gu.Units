namespace Gu.Units.Generator.Tests.Views
{
    using NUnit.Framework;

    public class PrefixConversionVmTests
    {
        [Test]
        public void SetIsUsedToTrueAddsConversion()
        {
            var settings = MockSettings.Create();
            var prefixConversion = PrefixConversion.Create(settings.Amperes, settings.Milli);
            var conversionVm = PrefixConversionVm.Create(settings.Amperes, settings.Milli);
            Assert.IsFalse(conversionVm.IsUsed);
            Assert.IsEmpty(settings.Amperes.PrefixConversions);

            conversionVm.IsUsed = true;
            var expected = new[] {prefixConversion};
            CollectionAssert.AreEqual(expected, settings.Amperes.PrefixConversions, PrefixConversionComparer.Default);
        }

        [Test]
        public void MilliAmpereExpression()
        {
            var settings = MockSettings.Create();
            var prefixConversionVm = PrefixConversionVm.Create(settings.Amperes, settings.Milli);
            Assert.AreEqual("1 mA = 1E-3 A", prefixConversionVm.Formula);
        }

        [Test]
        public void MilliGramExpression()
        {
            var settings = MockSettings.Create();
            var prefixConversionVm = PrefixConversionVm.Create(settings.Grams, settings.Milli);
            Assert.AreEqual("1 mg = 1E-3 g (1E-6 kg)", prefixConversionVm.Formula);
        }
    }
}