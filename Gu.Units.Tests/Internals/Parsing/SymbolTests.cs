namespace Gu.Units.Tests.Internals.Parsing
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using NUnit.Framework;

    public class SymbolTests
    {
        [TestCaseSource(nameof(SuccessSource))]
        public void TokenizeSuccess(ISuccessData data)
        {
            var actual = Symbol.TokenizeUnit(data.Text);
            //Console.WriteLine("expected: {0}", data.ToString(data.Tokens));
            //Console.WriteLine("actual:   {0}", data.ToString(actual));
            CollectionAssert.AreEqual((IEnumerable)data.Expected, actual);
        }

        [TestCaseSource(nameof(ErrorSource))]
        public void TokenizeError(IErrorData data)
        {
            Assert.Throws<FormatException>(() => Symbol.TokenizeUnit(data.Text));
        }

        [TestCaseSource(nameof(SuccessSource))]
        public void TryTokenizeSuccess(ISuccessData data)
        {
            var pos = data.Start;
            IReadOnlyList<SymbolAndPower> actual;
            var success = Symbol.TryTokenizeUnit(data.Text, ref pos, out actual);
            Assert.AreEqual(true, success);
            Assert.AreEqual(data.ExpectedEnd, pos);
            CollectionAssert.AreEqual((IEnumerable)data.Expected, actual);
        }

        [TestCaseSource(nameof(ErrorSource))]
        public void TryTokenizeError(IErrorData data)
        {
            var pos = data.Start;
            IReadOnlyList<SymbolAndPower> actual;
            var success = Symbol.TryTokenizeUnit(data.Text, ref pos, out actual);
            Assert.AreEqual(false, success);
            Assert.AreEqual(data.ExpectedEnd, pos);
            CollectionAssert.AreEqual((IEnumerable) data.Expected, actual);
        }

        private const string Superscripts = "⋅⁺⁻⁰¹²³⁴⁵⁶⁷⁸⁹";

        internal static readonly IReadOnlyList<SuccessData<IReadOnlyList<SymbolAndPower>>> SuccessSource = new[]
        {
            SuccessData.Create("m", new SymbolAndPower("m", 1)),
            SuccessData.Create(" m ", new SymbolAndPower("m", 1)),
            SuccessData.Create("m^2", new SymbolAndPower("m", 2)),
            SuccessData.Create(" m ^ 2", new SymbolAndPower("m", 2)),
            SuccessData.Create(" m ^ -2", new SymbolAndPower("m", -2)),
            SuccessData.Create("m^1/s^2", new SymbolAndPower("m", 1), new SymbolAndPower("s", -2)),
            SuccessData.Create("m¹/s²", new SymbolAndPower("m", 1), new SymbolAndPower("s", -2)),
            SuccessData.Create("m⁺¹/s²", new SymbolAndPower("m", 1), new SymbolAndPower("s", -2)),
            SuccessData.Create("m⁺¹/s²*g", new SymbolAndPower("m", 1), new SymbolAndPower("s", -2), new SymbolAndPower("g", -1)),
            SuccessData.Create("m¹⋅s⁻²", new SymbolAndPower("m", 1), new SymbolAndPower("s", -2)),
            SuccessData.Create("m⁻¹⋅s⁻²", new SymbolAndPower("m", -1), new SymbolAndPower("s", -2)),
        };

        internal static readonly IReadOnlyList<SuccessData<IReadOnlyList<SymbolAndPower>>> ErrorSource = new[]
        {
            ErrorData.CreateForSymbol("m⁻¹/s⁻²"),
            ErrorData.CreateForSymbol("m⁻⁻¹"),
            ErrorData.CreateForSymbol("m⁺⁻¹"),
            ErrorData.CreateForSymbol("m+⁻¹"),
            ErrorData.CreateForSymbol("m^¹/s⁻²"),
        };
    }
}
