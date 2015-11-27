namespace Gu.Units.Wpf.Tests
{
    using NUnit.Framework;

    public class StringFormatParserTests
    {
        [Test]
        public void TryParse()
        {
            var format = "{0:F1 mm}";
            QuantityFormat<LengthUnit> actual;
            var success = StringFormatParser.TryParse(format, out actual);
            Assert.AreEqual(true, success);
            Assert.AreEqual(null, actual.PrePadding);
            Assert.AreEqual("F1", actual.ValueFormat);
            Assert.AreEqual(" ", actual.Padding);
            Assert.AreEqual("mm", actual.SymbolFormat);
            Assert.AreEqual(null, actual.PostPadding);
        }
    }
}
