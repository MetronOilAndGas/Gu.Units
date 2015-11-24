namespace Gu.Units.Tests.Internals.Parsing
{
    using System;
    using NUnit.Framework;

    public class SymbolAndPowerParserTests
    {
        private const string Superscripts = "⁺⁻⁰¹²³⁴⁵⁶⁷⁸⁹";

        internal static readonly Sign[] Signs = { Sign.Positive, Sign.Negative };

        [TestCase("m", 0, "m", 1, 1)]
        [TestCase(" m", 1, "m", 1, 2)]
        [TestCase("m^1", 0, "m", 1, 3)]
        [TestCase(" m ^ 1", 1, "m", 1, 6)]
        [TestCase("m⁻¹", 0, "m", -1, 3)]
        [TestCase("m^-1", 0, "m", -1, 4)]
        [TestCase("m¹", 0, "m", 1, 2)]
        [TestCase("m^2", 0, "m", 2, 3)]
        [TestCase("m^-2", 0, "m", -2, 4)]
        [TestCase("m²", 0, "m", 2, 2)]
        [TestCase("m⁻²", 0, "m", -2, 3)]
        [TestCase("m¹", 0, "m", 1, 2)]
        [TestCase("m³", 0, "m", 3, 2)]
        [TestCase("m⁹", 0, "m", 9, 2)]
        [TestCase("kg⁹", 0, "kg", 9, 3)]
        [TestCase("°", 0, "°", 1, 1)]
        public void ParseSuccess(string text, int startPos, string symbol, int power, int endPos)
        {
            var pos = startPos;
            var sap = SymbolAndPowerParser.Parse(text, ref pos);
            Assert.AreEqual(symbol, sap.Symbol);
            Assert.AreEqual(power, sap.Power);
            Assert.AreEqual(endPos, pos);

            pos = startPos;
            var success = SymbolAndPowerParser.TryParse(text, ref pos, out sap);
            Assert.AreEqual(true, success);
            Assert.AreEqual(symbol, sap.Symbol);
            Assert.AreEqual(power, sap.Power);
            Assert.AreEqual(endPos, pos);
        }

        [TestCase("m¹²", 0, 0)]
        [TestCase("m⁻¹²", 0, 0)]
        [TestCase("m⁻⁻2", 0, 0)]
        [TestCase("m^12", 0, 0)]
        [TestCase("m^-12", 0, 0)]
        [TestCase("m^--2", 0, 0)]
        [TestCase("m-", 0, 0)]
        public void ParseFails(string text, int startPos, int endPos)
        {
            var pos = startPos;
            Assert.Throws<FormatException>(() => SymbolAndPowerParser.Parse(text, ref pos));
            Assert.AreEqual(endPos, pos);

            pos = startPos;
            SymbolAndPower sap;
            var success = SymbolAndPowerParser.TryParse(text, ref pos, out sap);
            Assert.AreEqual(true, success);
            Assert.AreEqual(null, sap.Symbol);
            Assert.AreEqual(0, sap.Power);
            Assert.AreEqual(endPos, pos);
        }
    }
}