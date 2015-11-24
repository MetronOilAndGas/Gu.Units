namespace Gu.Units.Tests.Internals.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;

    public class SuccessData<T> : ISuccessData
    {
        public SuccessData(string text, int start, T expected, int expectedEnd)
            : this(text, CultureInfo.InvariantCulture, start, expected, expectedEnd)
        {
        }

        public SuccessData(string text,
            CultureInfo cultureInfo,
            int start,
            T expected,
            int expectedEnd)
        {
            Text = text;
            CultureInfo = cultureInfo;
            Start = start;
            Expected = expected;
            ExpectedEnd = expectedEnd;
        }

        public string Text { get; }
        public object Parse(string text)
        {
            var parseMethod = typeof(T).GetMethod(
                nameof(Length.Parse),
                new[] { typeof(string) });
            return parseMethod.Invoke(null, new object[] { text });
        }

        public object Parse(string text,
            CultureInfo cultureInfo)
        {
            var name = nameof(Length.Parse);
            var parseMethod = typeof(T).GetMethod(
                name,
                new[] { typeof(string), typeof(IFormatProvider) });
            return parseMethod.Invoke(null, new object[] { text, cultureInfo });
        }

        public bool TryParse(string text,
            out object actual)
        {
            var tryParseMethod = typeof(T).GetMethod(
                nameof(Length.TryParse),
                new[] { typeof(string), typeof(T).MakeByRefType() });
            actual = null;
            var parameters = new[] { text, actual };
            var success = (bool) tryParseMethod.Invoke(null, parameters);
            actual = parameters[1];
            return success;
        }

        public bool TryParse(string text,
            CultureInfo cultureInfo,
            out object actual)
        {
            var parseMethod = typeof(T).GetMethod(
                nameof(Length.TryParse),
                new[] { typeof(string), typeof(IFormatProvider), typeof(T).MakeByRefType() });
            actual = null;
            var parameters = new object[] { text, cultureInfo, actual };
            var success = (bool) parseMethod.Invoke(null, parameters);
            actual = parameters[2];
            return success;
        }

        public CultureInfo CultureInfo { get; }

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