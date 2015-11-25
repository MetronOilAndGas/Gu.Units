namespace Gu.Units.Tests.Internals.Parsing
{
    using NUnit.Framework;

    public class FormatParserTests
    {
        [TestCase("E", "{0:E}m", "1.2m")]
        [TestCase("e", "{0:E}m", "1.2m")]
        [TestCase("E5", "{0:E}m", "1.2m")]
        [TestCase("e5", "{0:E}m", "1.2m")]
        [TestCase("F", "{0:E}m", "1.2m")]
        [TestCase("f", "{0:E}m", "1.2m")]
        [TestCase("F5", "{0:E}m", "1.2m")]
        [TestCase("f5", "{0:E}m", "1.2m")]
        [TestCase("G", "{0:E}m", "1.2m")]
        [TestCase("g", "{0:E}m", "1.2m")]
        [TestCase("G5", "{0:E}m", "1.2m")]
        [TestCase("g5", "{0:E}m", "1.2m")]
        [TestCase("N", "{0:E}m", "1.2m")]
        [TestCase("n", "{0:E}m", "1.2m")]
        [TestCase("N5", "{0:E}m", "1.2m")]
        [TestCase("n5", "{0:E}m", "1.2m")]
        [TestCase("R", "{0:E}m", "1.2m")]
        [TestCase("r", "{0:E}m", "1.2m")]
        [TestCase("0", "{0:E}m", "1.2m")]
        [TestCase("0.00", "{0:E}m", "1.2m")]
        [TestCase("#", "{0:E}m", "1.2m")]
        [TestCase("#.#", "{0:E}m", "1.2m")]
        [TestCase("#.0#", "{0:E}m", "1.2m")]
        [TestCase("#0.0#", "{0:E}m", "1.2m")]
        public void TryParseDoubleFormatOnly(string format, string expectedFormat, string expectedFormatted)
        {
            QuantityFormat<LengthUnit> actual;
            var success = FormatParser.TryParse(format, LengthUnit.Metres, out actual);

            Assert.AreEqual(true, success);
            Assert.AreEqual(expectedFormat, actual.Format);
            Assert.AreEqual(LengthUnit.Metres, actual.Unit);
            var actualFormatted = string.Format(actual.Format, 1.2, LengthUnit.Metres);
            Assert.AreEqual(expectedFormatted, actualFormatted);
        }

        [TestCase("Emm", "mm")]
        [TestCase("E mm", "mm")]
        [TestCase("e")]
        [TestCase("E5")]
        [TestCase("e5")]
        [TestCase("f")]
        [TestCase("F5")]
        [TestCase("f5")]
        [TestCase("G")]
        [TestCase("g")]
        [TestCase("G5")]
        [TestCase("g5")]
        [TestCase("N")]
        [TestCase("n")]
        [TestCase("N5")]
        [TestCase("n5")]
        [TestCase("R")]
        [TestCase("r")]
        [TestCase("0")]
        [TestCase("0.00")]
        [TestCase("#")]
        [TestCase("#.#")]
        [TestCase("#.0#")]
        [TestCase("#0.0#")]
        public void TryParseWithUnit(string format)
        {
            QuantityFormat<LengthUnit> actual;
            var success = FormatParser.TryParse(format, LengthUnit.Metres, out actual);

            Assert.AreEqual(true, success);
            Assert.AreEqual(format, actual.ValueFormat);
            Assert.AreEqual(LengthUnit.Metres, actual.Unit);
            Assert.AreEqual("m", actual.UnitFormat);
            Assert.DoesNotThrow(() => 1.2.ToString(actual.ValueFormat));
        }
    }
}
