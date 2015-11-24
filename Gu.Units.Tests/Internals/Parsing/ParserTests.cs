namespace Gu.Units.Tests.Internals.Parsing
{
    using System;
    using System.Globalization;
    using System.Threading;
    using NUnit.Framework;
    using Sources;

    public class ParserTests
    {
        [TestCase("1m", new[] { "sv-se", "en-us" }, 1)]
        [TestCase("-1m", new[] { "sv-se", "en-us" }, -1)]
        [TestCase("1.2m", new[] { "en-us" }, 1.2)]
        [TestCase("1.2m", new[] { "en-us" }, 1.2)]
        [TestCase("1,2m", new[] { "sv-se" }, 1.2)]
        [TestCase("-1m", new[] { "sv-se", "en-us" }, -1)]
        [TestCase("1e3m", new[] { "sv-se", "en-us" }, 1e3)]
        [TestCase("1E3m", new[] { "sv-se", "en-us" }, 1e3)]
        [TestCase("1e+3m", new[] { "sv-se", "en-us" }, 1e+3)]
        [TestCase("1E+3m", new[] { "sv-se", "en-us" }, 1E+3)]
        [TestCase("1.2e-3m", new[] { "en-us" }, 1.2e-3)]
        [TestCase("1.2E-3m", new[] { "en-us" }, 1.2e-3)]
        [TestCase(" 1m", new[] { "sv-se", "en-us" }, 1)]
        [TestCase("1 m", new[] { "sv-se", "en-us" }, 1)]
        [TestCase("1m ", new[] { "sv-se", "en-us" }, 1)]
        [TestCase("1mm", new[] { "sv-se", "en-us" }, 1e-3)]
        [TestCase("1,2mm", new[] { "sv-se" }, 1.2e-3)]
        [TestCase("1cm", new[] { "sv-se", "en-us" }, 1e-2)]
        public void ParseLength(string s, string[] cultures, double expected)
        {
            foreach (var culture in cultures)
            {
                var cultureInfo = CultureInfo.GetCultureInfo(culture);
                var numberStyles = NumberStyles.Float;

                var length = QuantityParser.Parse<LengthUnit, Length>(s, Length.From, numberStyles, cultureInfo);
                Assert.AreEqual(expected, length.Metres);

                length = Length.Parse(s, numberStyles, cultureInfo);
                Assert.AreEqual(expected, length.Metres);

                length = Length.Parse(s, cultureInfo);
                Assert.AreEqual(expected, length.Metres);
            }
        }

        [TestCase("1,2m", new[] { "en-us" }, "Could not parse the unit value from: 1,2m")]
        public void ParseLengthThrows(string s, string[] cultures, string expectedMessage)
        {
            foreach (var culture in cultures)
            {
                var cultureInfo = CultureInfo.GetCultureInfo(culture);
                var numberStyles = NumberStyles.Float;
                var ex1 = Assert.Throws<FormatException>(() => QuantityParser.Parse<LengthUnit, Length>(s, Length.From, numberStyles, cultureInfo));
                var ex2 = Assert.Throws<FormatException>(() => Length.Parse(s, numberStyles, cultureInfo));
                var ex3 = Assert.Throws<FormatException>(() => Length.Parse(s, cultureInfo));

                foreach (var ex in new[] { ex1, ex2, ex3 })
                {
                    Assert.AreEqual(expectedMessage, ex.Message);
                }
            }
        }

        [TestCase("1m", new[] { "sv-se", "en-us" }, 1)]
        [TestCase("-1m", new[] { "sv-se", "en-us" }, -1)]
        [TestCase("1.2m", new[] { "en-us" }, 1.2)]
        [TestCase("1.2m", new[] { "en-us" }, 1.2)]
        [TestCase("1,2m", new[] { "sv-se" }, 1.2)]
        [TestCase("-1m", new[] { "sv-se", "en-us" }, -1)]
        [TestCase("1e3m", new[] { "sv-se", "en-us" }, 1e3)]
        [TestCase("1E3m", new[] { "sv-se", "en-us" }, 1e3)]
        [TestCase("1e+3m", new[] { "sv-se", "en-us" }, 1e+3)]
        [TestCase("1E+3m", new[] { "sv-se", "en-us" }, 1E+3)]
        [TestCase("1.2e-3m", new[] { "en-us" }, 1.2e-3)]
        [TestCase("1.2E-3m", new[] { "en-us" }, 1.2e-3)]
        [TestCase(" 1m", new[] { "sv-se", "en-us" }, 1)]
        [TestCase("1 m", new[] { "sv-se", "en-us" }, 1)]
        [TestCase("1m ", new[] { "sv-se", "en-us" }, 1)]
        [TestCase("1mm", new[] { "sv-se", "en-us" }, 1e-3)]
        [TestCase("1cm", new[] { "sv-se", "en-us" }, 1e-2)]
        public void TryParseLengthSuccess(string s, string[] cultures, double expected)
        {
            foreach (var culture in cultures)
            {
                var cultureInfo = CultureInfo.GetCultureInfo(culture);
                Length length;
                var success = QuantityParser.TryParse<LengthUnit, Length>(
                    s,
                    Length.From,
                    NumberStyles.Float,
                    cultureInfo,
                    out length);
                Assert.AreEqual(true, success);
                Assert.AreEqual(expected, length.Metres);

                success = Length.TryParse(s, NumberStyles.Float, cultureInfo, out length);
                Assert.AreEqual(true, success);
                Assert.AreEqual(expected, length.Metres);

                success = Length.TryParse(s, cultureInfo, out length);
                Assert.AreEqual(true, success);
                Assert.AreEqual(expected, length.Metres);
            }
        }

        [TestCase("1.2m", new[] { "sv-se" }, 0.0)]
        public void TryParseLengthFails(string s, string[] cultures, double expected)
        {
            foreach (var culture in cultures)
            {
                var cultureInfo = CultureInfo.GetCultureInfo(culture);
                Length length;
                var success = QuantityParser.TryParse<LengthUnit, Length>(
                    s,
                    Length.From,
                    NumberStyles.Float,
                    cultureInfo,
                    out length);
                Assert.AreEqual(false, success);
                Assert.AreEqual(expected, length.Metres);

                success = Length.TryParse(s, NumberStyles.Float, cultureInfo, out length);
                Assert.AreEqual(false, success);
                Assert.AreEqual(expected, length.Metres);

                success = Length.TryParse(s, cultureInfo, out length);
                Assert.AreEqual(false, success);
                Assert.AreEqual(expected, length.Metres);
            }
        }

        [TestCaseSource(typeof(ParseProvider))]
        public void Roundtrip(ParseProvider.ParseData data)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            var actual = data.ParseMethod(data.StringValue);
            var expected = data.Quantity;
            Assert.AreEqual(expected, actual);
            var s = actual.ToString();
            var roundtripped = data.ParseMethod(s);
            Assert.AreEqual(expected, roundtripped);
        }

        [TestCase("1.0cm", "sv-se")]
        [TestCase("1,0cm", "en-us")]
        public void Exceptions(string s, string culture)
        {
            var cultureInfo = CultureInfo.GetCultureInfo(culture);
            Assert.Throws<FormatException>(() => QuantityParser.Parse<LengthUnit, Length>(s, Length.From, NumberStyles.Float, cultureInfo));
        }
    }
}
