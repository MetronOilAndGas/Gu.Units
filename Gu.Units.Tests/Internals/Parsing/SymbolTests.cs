namespace Gu.Units.Tests.Internals.Parsing
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using Sources;

    public class SymbolTests
    {
        [TestCaseSource(typeof(TokenSource))]
        public void Tokenize(TokenSource.TokenData data)
        {
            var text = data.Text;
            if (data.Tokens == null)
            {
                Assert.Throws<FormatException>(() => Symbol.TokenizeUnit(text));
                IReadOnlyList<SymbolAndPower> actual;
                var success = Symbol.TryTokenizeUnit(text, out actual);
                Assert.AreEqual(false, success);
                CollectionAssert.IsEmpty(actual);
            }
            else
            {
                var actual = Symbol.TokenizeUnit(text);
                Console.WriteLine("expected: {0}", data.ToString(data.Tokens));
                Console.WriteLine("actual:   {0}", data.ToString(actual));
                CollectionAssert.AreEqual(data.Tokens, actual);

                var success = Symbol.TryTokenizeUnit(text, out actual);
                Assert.AreEqual(true, success);
                CollectionAssert.AreEqual(data.Tokens, actual);
            }
        }
    }
}
