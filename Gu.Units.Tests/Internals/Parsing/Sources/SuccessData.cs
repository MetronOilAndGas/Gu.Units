namespace Gu.Units.Tests.Internals.Parsing
{
    using System.Collections.Generic;

    public static class SuccessData
    {
        public static SuccessData<T> Create<T>(string text,
            int start,
            T expected,
            int expectedEnd)
        {
            return new SuccessData<T>(text, start, expected, expectedEnd);
        }

        internal static SuccessData<IReadOnlyList<SymbolAndPower>> Create(string text, params SymbolAndPower[] expected)
        {
            return new SuccessData<IReadOnlyList<SymbolAndPower>>(text, 0, expected, text.Length);
        }
    }
}