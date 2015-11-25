namespace Gu.Units.Tests.Internals.Parsing
{
    using System;
    using System.Collections.Generic;
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
        public void ParseLengthSuccess(string s, string[] cultures, double expected)
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

        [TestCase("1.2m", "sv-se", 0.0)]
        public void TryParseLengthFails(string s, string culture, double expected)
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

        [TestCaseSource(nameof(SuccessSource))]
        public void ParseRoundtrip(ISuccessData data)
        {
            Thread.CurrentThread.CurrentCulture = data.CultureInfo;
            var actual = data.Parse(data.Text);
            Assert.AreEqual(data.Expected, actual);
            var toString = actual.ToString();
            var roundtripped = data.Parse(toString);
            Assert.AreEqual(data.Expected, roundtripped);
        }

        [TestCaseSource(nameof(SuccessSource))]
        public void ParseRoundtripWithCulture(ISuccessData data)
        {
            var actual = data.Parse(data.Text, data.CultureInfo);
            Assert.AreEqual(data.Expected, actual);
            var toString = ((IFormattable)actual).ToString(null, data.CultureInfo);
            var roundtripped = data.Parse(toString, data.CultureInfo);
            Assert.AreEqual(data.Expected, roundtripped);
        }

        [TestCaseSource(nameof(SuccessSource))]
        public void TryParseRoundtrip(ISuccessData data)
        {
            Thread.CurrentThread.CurrentCulture = data.CultureInfo;
            object actual;
            var success = data.TryParse(data.Text, out actual);
            Assert.AreEqual(true, success);
            Assert.AreEqual(data.Expected, actual);
            var toString = actual.ToString();
            success = data.TryParse(toString, out actual);
            Assert.AreEqual(true, success);
            Assert.AreEqual(data.Expected, actual);
        }

        [TestCaseSource(nameof(SuccessSource))]
        public void TryParseRoundtripWithCulture(ISuccessData data)
        {
            object actual;
            var success = data.TryParse(data.Text, data.CultureInfo, out actual);
            Assert.AreEqual(true, success);
            Assert.AreEqual(data.Expected, actual);
            var toString = ((IFormattable)actual).ToString(null, data.CultureInfo);
            success = data.TryParse(data.Text, data.CultureInfo, out actual);
            Assert.AreEqual(data.Expected, actual);
            Assert.AreEqual(true, success);
        }

        [TestCase("1.0cm", "sv-se")]
        [TestCase("1,0cm", "en-us")]
        public void Exceptions(string s, string culture)
        {
            var cultureInfo = CultureInfo.GetCultureInfo(culture);
            var ex = Assert.Throws<FormatException>(() => QuantityParser.Parse<LengthUnit, Length>(s, Length.From, NumberStyles.Float, cultureInfo));
            Console.Write(ex.Message);
        }

        private static readonly CultureInfo en = CultureInfo.GetCultureInfo("en-US");
        private static readonly CultureInfo sv = CultureInfo.GetCultureInfo("sv-SE");

        private static readonly IReadOnlyList<ISuccessData> SuccessSource = new ISuccessData[]
        {
            SuccessData.Create("1.2m^2", en, Area.FromSquareMetres(1.2)),
            SuccessData.Create("1.2m²", en, Area.FromSquareMetres(1.2)),
            SuccessData.Create("1,2m²", sv, Area.FromSquareMetres(1.2)),
            SuccessData.Create("1.2s", en, Time.FromSeconds(1.2)),
            SuccessData.Create("1.2h", en, Time.FromHours(1.2)),
            SuccessData.Create("1.2ms", en, Time.FromMilliseconds(1.2)),
            SuccessData.Create("1.2kg", en, Mass.FromKilograms(1.2)),
            SuccessData.Create("1.2g", en, Mass.FromGrams(1.2)),
            SuccessData.Create("1.2m³", en, Volume.FromCubicMetres(1.2)),
            SuccessData.Create("1.2m^3", en, Volume.FromCubicMetres(1.2)),
            SuccessData.Create("1.2m/s", en, Speed.FromMetresPerSecond(1.2)),
            SuccessData.Create("1.2m⋅s⁻¹", en, Speed.FromMetresPerSecond(1.2)),
            SuccessData.Create("1.2m*s⁻¹", en, Speed.FromMetresPerSecond(1.2)),
            SuccessData.Create("1.2m¹⋅s⁻¹", en, Speed.FromMetresPerSecond(1.2)),
            SuccessData.Create("1.2m^1⋅s⁻¹", en, Speed.FromMetresPerSecond(1.2)),
            SuccessData.Create("1.2m^1⋅s^-1", en, Speed.FromMetresPerSecond(1.2)),
            SuccessData.Create("1.2m^1/s^2", en, Acceleration.FromMetresPerSecondSquared(1.2)),
            SuccessData.Create("1.2m/s^2", en, Acceleration.FromMetresPerSecondSquared(1.2)),
            SuccessData.Create("1.2 m/s^2", en, Acceleration.FromMetresPerSecondSquared(1.2)),
            SuccessData.Create("1.2 m / s^2", en, Acceleration.FromMetresPerSecondSquared(1.2)),
            SuccessData.Create("1.2 m / s²", en, Acceleration.FromMetresPerSecondSquared(1.2)),
            SuccessData.Create("1.2 mm / s²", en, Acceleration.FromMillimetresPerSecondSquared(1.2)),
        };
    }
}
