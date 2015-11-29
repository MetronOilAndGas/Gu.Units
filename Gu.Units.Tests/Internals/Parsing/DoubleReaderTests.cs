namespace Gu.Units.Tests.Internals.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using NUnit.Framework;

    public class DoubleReaderTests
    {
        private static readonly string[] padFormat = { "abc{0}def", "abcd{0}ef" };

        [TestCaseSource(nameof(DoubleParseHappyPathSource))]
        public void ReadSuccess(DoubleData data)
        {
            var culture = data.Culture;
            var style = data.Styles;
            var s = data.Text;
            foreach (var format in padFormat)
            {
                var ns = string.Format(format, s);
                var pos = format.IndexOf('{');
                var start = pos;
                double expected = double.Parse(s, style, culture);
                var actual = DoubleReader.Read(ns, ref pos, style, culture);
                Assert.AreEqual(expected, actual);
                var expectedEnd = start + s.Length;
                Assert.AreEqual(expectedEnd, pos);
            }
        }

        [TestCaseSource(nameof(DoubleParseErrorSource))]
        public void ReadError(DoubleData data)
        {
            var culture = data.Culture;
            var style = data.Styles;
            var s = data.Text;
            foreach (var format in padFormat)
            {
                var ns = string.Format(format, s);
                var pos = format.IndexOf('{');
                var start = pos;
                Assert.Throws<FormatException>(() => double.Parse(s, style, culture));
                Assert.Throws<FormatException>(() => DoubleReader.Read(ns, ref pos, style, culture));
                Assert.AreEqual(start, pos);
            }
        }

        [Test]
        public void ReadException()
        {
            int endPos;
            var text = "abcdef";
            var culture = CultureInfo.InvariantCulture;
            var pos = 3;
            var e = Assert.Throws<FormatException>(() => DoubleReader.Read(text, ref pos, NumberStyles.Float, culture));
            var expected = "Expected to find a double starting at index 3\r\n" +
                           "String: abcdef\r\n" +
                           "           ^";
            Assert.AreEqual(expected, e.Message);
        }

        [TestCaseSource(nameof(DoubleParseHappyPathSource))]
        public void TryReadSuccess(DoubleData data)
        {
            var culture = data.Culture;
            var style = data.Styles;
            foreach (var format in padFormat)
            {
                var text = string.Format(format, data.Text);
                var pos = format.IndexOf('{');
                var start = pos;
                double expected;
                Assert.IsTrue(double.TryParse(data.Text, style, culture, out expected));
                double actual;
                Assert.IsTrue(DoubleReader.TryRead(text, ref pos, style, culture, out actual));
                Assert.AreEqual(expected, actual);
                var expectedEnd = start + data.Text.Length;
                Assert.AreEqual(expectedEnd, pos);
            }
        }

        [TestCaseSource(nameof(DoubleParseErrorSource))]
        public void TryReadError(DoubleData data)
        {
            var culture = data.Culture;
            var style = data.Styles;
            var s = data.Text;
            foreach (var format in padFormat)
            {
                var ns = string.Format(format, s);
                var pos = format.IndexOf('{');
                var start = pos;
                double expected;
                Assert.IsFalse(double.TryParse(s, style, culture, out expected));
                double actual;
                Assert.IsFalse(DoubleReader.TryRead(ns, ref pos, style, culture, out actual));
                Assert.AreEqual(expected, actual);
                Assert.AreEqual(start, pos);
            }
        }

        #region TestData

        private static readonly CultureInfo en = CultureInfo.GetCultureInfo("en-US");
        private static readonly CultureInfo sv = CultureInfo.GetCultureInfo("sv-SE");

        private static readonly IReadOnlyList<DoubleData> DoubleParseHappyPathSource = new[]
        {
            CreateParseData("0", NumberStyles.Float, en),
            CreateParseData("0.", NumberStyles.Float, en),
            CreateParseData(".0", NumberStyles.Float, en),
            CreateParseData("0.0", NumberStyles.Float, en),
            CreateParseData("1.2", NumberStyles.Float, en),
            CreateParseData("0.012", NumberStyles.Float, en),
            CreateParseData("0.001", NumberStyles.Float, en),
            CreateParseData("1", NumberStyles.Float, en),
            CreateParseData(" 1", NumberStyles.Float, en),
            CreateParseData("-1", NumberStyles.Float, en),
            CreateParseData("+1", NumberStyles.Float, en),
            CreateParseData(".1", NumberStyles.Float, en),
            CreateParseData("-.1", NumberStyles.Float, en),
            CreateParseData("1.", NumberStyles.Float, en),
            CreateParseData("-1.", NumberStyles.Float, en),
            CreateParseData("12,345.67", NumberStyles.Float | NumberStyles.AllowThousands, en),
            CreateParseData("-12,345.67", NumberStyles.Float | NumberStyles.AllowThousands, en),
            CreateParseData("+1.2", NumberStyles.Float, en),
            CreateParseData("+1,2", NumberStyles.Float, sv),
            CreateParseData("+1.2e3", NumberStyles.Float, en),
            CreateParseData("-1.2E3", NumberStyles.Float, en),
            CreateParseData("+1.2e-3", NumberStyles.Float, en),
            CreateParseData("+1.2E-3", NumberStyles.Float, en),
            CreateParseData("-1.2e+3", NumberStyles.Float, en),
            CreateParseData(-12345.678910, NumberStyles.Float, en, "e"),
            CreateParseData(12345.678910, NumberStyles.Float, en, "E"),
            CreateParseData(12345.678910, NumberStyles.Float, en, "E5"),
            CreateParseData(-12345.678910, NumberStyles.Float, en, "f"),
            CreateParseData(12345.678910, NumberStyles.Float, en, "F"),
            CreateParseData(12345.678912, NumberStyles.Float, en, "F20"),
            CreateParseData(12345.678910, NumberStyles.Float, en, "G"),
            CreateParseData(-12345.678910, NumberStyles.Float, en, "g"),
            CreateParseData(12345.678910, NumberStyles.Float, en, "g5"),
            CreateParseData(12345.678910, NumberStyles.Float |NumberStyles.AllowThousands, en, "n"),
            CreateParseData(12345.678910, NumberStyles.Float |NumberStyles.AllowThousands, en, "N"),
            CreateParseData(12345.678910, NumberStyles.Float |NumberStyles.AllowThousands, en, "N5"),
            CreateParseData(12345.678910, NumberStyles.Float, en, "R"),
            CreateParseData(-12345.678910, NumberStyles.Float, en, "r"),
            CreateParseData(-Math.PI, NumberStyles.Float, en, "f15"),
            CreateParseData(-Math.PI, NumberStyles.Float, en, "f16"),
            CreateParseData(-Math.PI, NumberStyles.Float, en, "f17"),
            CreateParseData("3.141592653589793238", NumberStyles.Float, en),
            CreateParseData("-3.141592653589793238", NumberStyles.Float, en),
            CreateParseData(-Math.PI, NumberStyles.Float, en, "r"),
            CreateParseData(sv.NumberFormat.NaNSymbol, NumberStyles.Float, sv),
            CreateParseData(sv.NumberFormat.PositiveInfinitySymbol, NumberStyles.Float, sv),
            CreateParseData(sv.NumberFormat.NegativeInfinitySymbol, NumberStyles.Float, sv),
        };

        private static DoubleData CreateParseData(double value,
            NumberStyles styles,
            IFormatProvider culture,
            string format)
        {
            return new DoubleData(value.ToString(format, culture), styles, culture);
        }

        private static readonly IReadOnlyList<DoubleData> DoubleParseErrorSource = new[]
        {
            CreateParseData("e1", NumberStyles.Float, en),
            CreateParseData(" 1", NumberStyles.None, en),
            CreateParseData("-1", NumberStyles.None, en),
            CreateParseData(".1", NumberStyles.None, en),
            CreateParseData(",.1", NumberStyles.Float, en),
            //Add("1.", NumberStyles.Float | NumberStyles.AllowHexSpecifier, en),
            CreateParseData(".", NumberStyles.Float, en),
            //Add("+1,2", NumberStyles.Float, en),
            //Add("+1.2", NumberStyles.Float, sv),
            CreateParseData("+1.2e3", NumberStyles.None | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, en),
        };

        private static DoubleData CreateParseData(string text,
            NumberStyles styles,
            CultureInfo culture)
        {
            return new DoubleData(text, styles, culture);
        }

        public class DoubleData
        {
            public readonly string Text;
            public readonly NumberStyles Styles;
            public readonly IFormatProvider Culture;

            public DoubleData(string text,
                NumberStyles styles,
                IFormatProvider culture)
            {
                this.Text = text;
                this.Styles = styles;
                this.Culture = culture;
            }

            public override string ToString()
            {
                return $"Text: {this.Text}, Styles: {this.Styles}, Culture: {this.Culture}";
            }
        }

        #endregion  TestData
    }
}
