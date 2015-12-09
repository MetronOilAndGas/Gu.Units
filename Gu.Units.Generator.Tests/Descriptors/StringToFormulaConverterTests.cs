namespace Gu.Units.Generator.Tests.Descriptors
{
    using NUnit.Framework;
    using WpfStuff;

    public class StringToConversionConverterTests
    {
        [TestCase("10*x")]
        public void SimpleFactorTest(string text)
        {
            Assert.Inconclusive("Dunno if this is needed");
            var converter = new StringToFormulaConverter();
            Assert.True(converter.CanConvertFrom(null, typeof(string)));
            var convertFrom = (FactorConversion)converter.ConvertFrom(null, null, text);
            Assert.AreEqual(text, convertFrom.ToSi);
            Assert.AreEqual("x/10", convertFrom.FromSi);
        }
    }
}
