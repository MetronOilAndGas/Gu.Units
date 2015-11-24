namespace Gu.Units.Tests.Internals.Parsing
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class SuccessData<T> : ISuccessData
    {
        public SuccessData(string text, int start, T expected, int expectedEnd)
        {
            Text = text;
            Start = start;
            Expected = expected;
            ExpectedEnd = expectedEnd;
        }

        public string Text { get; }

        public int Start { get; }

        public T Expected { get; }

        object ISuccessData.Expected => Expected;

        public int ExpectedEnd { get; }

        public override string ToString()
        {
            return $"Text: {Text}, Start: {Start}, Expected {ToString(typeof(T))}: {ToString(Expected)}, ExpectedEnd: {ExpectedEnd}";
        }

        private static string ToString(Type type)
        {
            if (type == typeof(IReadOnlyList<SymbolAndPower>))
            {
                return string.Empty;
            }

            return $"({type.Name})";
        }

        private static string ToString(T expected)
        {
            if (expected == null)
            {
                return "null";
            }

            var saps = expected as IEnumerable<SymbolAndPower>;
            if (saps != null)
            {
                return $"{{{string.Join(", ", saps)}}}";
            }

            return expected.ToString();
        }
    }
}