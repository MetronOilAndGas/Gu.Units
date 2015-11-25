namespace Gu.Units.Tests.Internals.Parsing
{
    using System.Text.RegularExpressions;
    using NUnit.Framework;

    public class FormatParserTests
    {
        [Test]
        public void Parse()
        {
            var format = "F";
            QuantityFormat<LengthUnit> actual;
            var success = FormatParser.TryParse(format, out actual);

            Assert.AreEqual(true, success);
            Assert.AreEqual(format, actual.ValueFormat);
            Assert.AreEqual(LengthUnit.Metres, actual.Unit);
            Assert.AreEqual("m", actual.UnitFormat);
        }

        [Test]
        public void CheckDoubleFormats()
        {
            foreach (var pattern in FormatParser.DoubleFormatPatterns)
            {
                var match = Regex.Match(pattern, $@"^\^ \*\(\?\<format\>(?<format>[^)]+)\)$");
                Assert.AreEqual(true, match.Success);
                var format = match.Groups["format"].Value;
                var temp = 1.2.ToString(format); // checking that it is a valid format
            }
        }
    }
}
