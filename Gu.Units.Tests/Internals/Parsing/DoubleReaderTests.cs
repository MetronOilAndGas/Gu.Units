namespace Gu.Units.Tests.Internals.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using NUnit.Framework;

    public class DoubleReaderTests
    {
        private static readonly string[] Formats = { "abc{0}def", "abcd{0}ef" };

        [TestCaseSource(nameof(DoubleParseHappyPathSource))]
        public void ReadSuccess(DoubleData data)
        {
            var culture = data.Culture;
            var style = data.Styles;
            var s = data.Text;
            foreach (var format in Formats)
            {
                var ns = string.Format(format, s);
                var start = format.IndexOf('{');
                int end;
                double expected = double.Parse(s, style, culture);
                var actual = DoubleReader.Read(ns, start, style, culture, out end);
                Assert.AreEqual(expected, actual);
                var expectedEnd = start + s.Length;
                Assert.AreEqual(expectedEnd, end);
            }
        }

        [TestCaseSource(nameof(DoubleParseErrorSource))]
        public void ReadError(DoubleData data)
        {
            var culture = data.Culture;
            var style = data.Styles;
            var s = data.Text;
            foreach (var format in Formats)
            {
                var ns = string.Format(format, s);
                var start = format.IndexOf('{');
                int end = -1;
                Assert.Throws<FormatException>(() => double.Parse(s, style, culture));
                Assert.Throws<FormatException>(() => DoubleReader.Read(ns, start, style, culture, out end));
                Assert.AreEqual(start, end);
            }
        }

        [Test]
        public void ReadException()
        {
            int endPos;
            var text = "abcdef";
            var culture = CultureInfo.InvariantCulture;
            var e = Assert.Throws<FormatException>(() => DoubleReader.Read(text, 3, NumberStyles.Float, culture, out endPos));
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
            var s = data.Text;
            foreach (var format in Formats)
            {
                var ns = string.Format(format, s);
                var start = format.IndexOf('{');
                int end;
                double expected;
                Assert.IsTrue(double.TryParse(s, style, culture, out expected));
                double actual;
                Assert.IsTrue(DoubleReader.TryRead(ns, start, style, culture, out actual, out end));
                Assert.AreEqual(expected, actual);
                var expectedEnd = start + s.Length;
                Assert.AreEqual(expectedEnd, end);
            }
        }

        [TestCaseSource(nameof(DoubleParseErrorSource))]
        public void TryReadError(DoubleData data)
        {
            var culture = data.Culture;
            var style = data.Styles;
            var s = data.Text;
            foreach (var format in Formats)
            {
                var ns = string.Format(format, s);
                var start = format.IndexOf('{');
                int end;
                double expected;
                Assert.IsFalse(double.TryParse(s, style, culture, out expected));
                double actual;
                Assert.IsFalse(DoubleReader.TryRead(ns, start, style, culture, out actual, out end));
                Assert.AreEqual(expected, actual);
                Assert.AreEqual(start, end);
            }
        }

        [Test]
        public void Reminder()
        {
            Assert.Fail("Max digits");
            Assert.Fail("Max mantissa");
            Assert.Fail("Max exp");
            Assert.Fail("Min exp");
        }

        #region TestData

        private static readonly CultureInfo en = CultureInfo.GetCultureInfo("en-US");
        private static readonly CultureInfo sv = CultureInfo.GetCultureInfo("sv-SE");

        private static readonly IReadOnlyList<DoubleData> DoubleParseHappyPathSource = new[]
        {
            CreateParseData("1", NumberStyles.Float, en),
            CreateParseData(" 1", NumberStyles.Float, en),
            CreateParseData("-1", NumberStyles.Float, en),
            CreateParseData("+1", NumberStyles.Float, en),
            CreateParseData(".1", NumberStyles.Float, en),
            CreateParseData("1.", NumberStyles.Float, en),
            CreateParseData("+1.2", NumberStyles.Float, en),
            CreateParseData("+1,2", NumberStyles.Float, sv),
            CreateParseData("+1.2e3", NumberStyles.Float, en),
            CreateParseData("+1.2E3", NumberStyles.Float, en),
            CreateParseData("+1.2e-3", NumberStyles.Float, en),
            CreateParseData("+1.2E-3", NumberStyles.Float, en),
            CreateParseData("+1.2e+3", NumberStyles.Float, en),
            CreateParseData(sv.NumberFormat.NaNSymbol, NumberStyles.Float, sv),
            CreateParseData(sv.NumberFormat.PositiveInfinitySymbol, NumberStyles.Float, sv),
            CreateParseData(sv.NumberFormat.NegativeInfinitySymbol, NumberStyles.Float, sv),
        };

        private static readonly IReadOnlyList<DoubleData> DoubleParseErrorSource = new[]
        {
            CreateParseData("e1", NumberStyles.Float, en),
            CreateParseData(" 1", NumberStyles.None, en),
            CreateParseData("-1", NumberStyles.None, en),
            CreateParseData(".1", NumberStyles.None, en),
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
            public readonly CultureInfo Culture;

            public DoubleData(string text,
                NumberStyles styles,
                CultureInfo culture)
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
