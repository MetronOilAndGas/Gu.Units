namespace Gu.Units.Tests.Internals.Parsing
{
    using NUnit.Framework;

    public class PowerParserTests
    {
        private const string Superscripts = "⋅⁺⁻⁰¹²³⁴⁵⁶⁷⁸⁹";

        [TestCase("mm^2", 2, 2, 4)]
        [TestCase("mm⁰", 2, 2, 4)]
        [TestCase("mm²", 2, 2, 4)]
        [TestCase("mm⁺²", 2, 2, 4)]
        [TestCase("mm⁻¹²", 2, 2, 4)]
        [TestCase("mm", 2, 1)]
        public void ParseSuccess(string text, int start, int expected, int expectedEnd)
        {
            var end = start;
            var p = PowerParser.Parse(text, ref end);
            Assert.AreEqual(expected, p);
            Assert.AreEqual(expectedEnd, end);

            PowerParser.tr
        }
    }
}
