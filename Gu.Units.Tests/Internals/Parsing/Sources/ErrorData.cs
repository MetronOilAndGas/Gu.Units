namespace Gu.Units.Tests.Internals.Parsing
{
    using System.Collections.Generic;

    public static class ErrorData
    {
        public static ErrorData<T> Create<T>(string text,
            int start,
            T expected,
            string expectedMessage)
        {
            return new ErrorData<T>(text, start, expected, start, expectedMessage);
        }

        public static ErrorData<T> Create<T>(string text,
            int start,
            string expectedMessage)
        {
            return new ErrorData<T>(text, start, default(T), start, expectedMessage);
        }

        public static ErrorData<T> Create<T>(string text, int start)
        {
            return new ErrorData<T>(text, start, default(T), start, null);
        }

        internal static ErrorData<IReadOnlyList<SymbolAndPower>> CreateForSymbol(string text)
        {
            IReadOnlyList<SymbolAndPower> expected = null;
            return new ErrorData<IReadOnlyList<SymbolAndPower>>(text, 0, expected, text.Length, null);
        }
    }
}