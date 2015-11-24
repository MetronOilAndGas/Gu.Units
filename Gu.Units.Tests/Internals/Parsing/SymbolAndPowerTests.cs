namespace Gu.Units.Tests.Internals.Parsing
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;

    public class SymbolAndPowerParserTests
    {
        [TestCaseSource(nameof(SuccessSource))]
        public void ParseSuccess(ISuccessData data)
        {
            var pos = data.Start;
            var actual = SymbolAndPowerParser.Parse(data.Text, ref pos);
            Assert.AreEqual(data.Expected, actual);
            Assert.AreEqual(data.ExpectedEnd, pos);
        }

        [TestCaseSource(nameof(ErrorSource))]
        public void ParseError(IErrorData data)
        {
            var pos = data.Start;
            Assert.Throws<FormatException>(() => SymbolAndPowerParser.Parse(data.Text, ref pos));
            Assert.AreEqual(data.ExpectedEnd, pos);
        }

        [TestCaseSource(nameof(SuccessSource))]
        public void TryParseSuccess(ISuccessData data)
        {
            var pos = data.Start;
            SymbolAndPower actual;
            var success = SymbolAndPowerParser.TryParse(data.Text, ref pos, out actual);
            Assert.AreEqual(true, success);
            Assert.AreEqual(data.Expected, actual);
            Assert.AreEqual(data.ExpectedEnd, pos);
        }

        [TestCaseSource(nameof(ErrorSource))]
        public void TryParseError(IErrorData data)
        {
            var pos = data.Start;
            SymbolAndPower sap;
            var success = SymbolAndPowerParser.TryParse(data.Text, ref pos, out sap);
            Assert.AreEqual(false, success);
            Assert.AreEqual(default(SymbolAndPower), sap);
            Assert.AreEqual(data.ExpectedEnd, pos);
        }

        private const string Superscripts = "⁺⁻⁰¹²³⁴⁵⁶⁷⁸⁹";

        private static readonly IReadOnlyList<SuccessData<SymbolAndPower>> SuccessSource = new[]
        {
            SuccessData.Create("m", 0, new SymbolAndPower("m", 1), 1),
            SuccessData.Create(" m", 1, new SymbolAndPower("m", 1), 2),
            SuccessData.Create("m^1", 0, new SymbolAndPower("m", 1), 3),
            SuccessData.Create(" m ^ 1", 1, new SymbolAndPower("m", 1), 6),
            SuccessData.Create("m⁻¹", 0, new SymbolAndPower("m", -1), 3),
            SuccessData.Create("m^-1", 0, new SymbolAndPower("m", -1), 4),
            SuccessData.Create("m¹", 0, new SymbolAndPower("m", 1), 2),
            SuccessData.Create("m^2", 0, new SymbolAndPower("m", 2), 3),
            SuccessData.Create("m^-2", 0, new SymbolAndPower("m", -2), 4),
            SuccessData.Create("m²", 0, new SymbolAndPower("m", 2), 2),
            SuccessData.Create("m⁻²", 0, new SymbolAndPower("m", -2), 3),
            SuccessData.Create("m¹", 0, new SymbolAndPower("m", 1), 2),
            SuccessData.Create("m³", 0, new SymbolAndPower("m", 3), 2),
            SuccessData.Create("m⁹", 0, new SymbolAndPower("m", 9), 2),
            SuccessData.Create("kg⁹", 0, new SymbolAndPower("kg", 9), 3),
            SuccessData.Create("°", 0, new SymbolAndPower("°", 1), 1)
        };

        private static readonly IReadOnlyList<SuccessData<SymbolAndPower>> ErrorSource = new[]
        {
            ErrorData.Create<SymbolAndPower>("m¹²", 0),
            ErrorData.Create<SymbolAndPower>("m⁻¹²", 0),
            ErrorData.Create<SymbolAndPower>("m⁻⁻2", 0),
            ErrorData.Create<SymbolAndPower>("m^12", 0),
            ErrorData.Create<SymbolAndPower>("m^-12", 0),
            ErrorData.Create<SymbolAndPower>("m^--2", 0),
            ErrorData.Create<SymbolAndPower>("m-", 0),
        };
    }
}