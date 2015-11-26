namespace Gu.Units.Tests.Internals.Parsing
{
    using System.Globalization;
    using NUnit.Framework;

    public class FormatParserTests
    {
        [TestCase("", "{0}m", "1.2m")]
        [TestCase("E", "{0:E}m", "1.200000E+000m")]
        [TestCase("e", "{0:e}m", "1.200000e+000m")]
        [TestCase("E5", "{0:E5}m", "1.20000E+000m")]
        [TestCase("e5", "{0:e5}m", "1.20000e+000m")]
        [TestCase("F", "{0:F}m", "1.20m")]
        [TestCase("f", "{0:f}m", "1.20m")]
        [TestCase("F5", "{0:F5}m", "1.20000m")]
        [TestCase("f5", "{0:f5}m", "1.20000m")]
        [TestCase("G", "{0:G}m", "1.2m")]
        [TestCase("g", "{0:g}m", "1.2m")]
        [TestCase("G5", "{0:G5}m", "1.2m")]
        [TestCase("g5", "{0:g5}m", "1.2m")]
        [TestCase("N", "{0:N}m", "1.20m")]
        [TestCase("n", "{0:n}m", "1.20m")]
        [TestCase("N5", "{0:N5}m", "1.20000m")]
        [TestCase("n5", "{0:n5}m", "1.20000m")]
        [TestCase("R", "{0:R}m", "1.2m")]
        [TestCase("r", "{0:r}m", "1.2m")]
        [TestCase("0", "{0:0}m", "1m")]
        [TestCase("0.00", "{0:0.00}m", "1.20m")]
        [TestCase("#", "{0:#}m", "1m")]
        [TestCase("#.#", "{0:#.#}m", "1.2m")]
        [TestCase("#.0#", "{0:#.0#}m", "1.2m")]
        [TestCase("#0.00#", "{0:#0.00#}m", "1.20m")]
        public void TryParseDoubleFormatOnly(string format, string expectedFormat, string expectedFormatted)
        {
            QuantityFormat<LengthUnit> actual;
            var success = FormatParser.TryParse(format, out actual);

            Assert.AreEqual(true, success);
            Assert.AreEqual(expectedFormat, actual.Format);
            Assert.AreEqual(LengthUnit.Metres, actual.Unit);
            var actualFormatted = string.Format(CultureInfo.InvariantCulture, actual.Format, 1.2, LengthUnit.Metres);
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
            Assert.Fail();
            //QuantityFormat<LengthUnit> actual;
            //var success = FormatParser.TryParse(format, LengthUnit.Metres, out actual);

            //Assert.AreEqual(true, success);
            //Assert.AreEqual(format, actual.ValueFormat);
            //Assert.AreEqual(LengthUnit.Metres, actual.Unit);
            //Assert.AreEqual("m", actual.UnitFormat);
            //Assert.DoesNotThrow(() => 1.2.ToString(actual.ValueFormat));
        }
    }
}
