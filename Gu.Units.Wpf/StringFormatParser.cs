namespace Gu.Units.Wpf
{
    using System;

    internal static class StringFormatParser
    {
        internal static bool TryParse<TUnit>(string format,
            out QuantityFormat<TUnit> result)
            where TUnit : struct, IUnit, IEquatable<TUnit>
        {
            if (string.IsNullOrWhiteSpace(format))
            {
                result = QuantityFormat<TUnit>.Default;
                return false;
            }

            int pos = 0;
            WhiteSpaceReader.TryRead(format, ref pos);
            int end = format.Length;
            if (TryReadPrefix(format, ref pos))
            {
                end = format.LastIndexOf('}') - 1;
                if (end < 0)
                {
                    result = QuantityFormat<TUnit>.Default;
                    return false;
                }

                if (!WhiteSpaceReader.IsRestWhiteSpace(format, end + 2))
                {
                    result = QuantityFormat<TUnit>.Default;
                    return false;
                }
            }

            return CompositeFormatParser.TryParse(format, ref pos, end, out result);
        }

        private static bool TryReadPrefix(string format,
            ref int pos)
        {
            var start = pos;
            if (TryReadChar(format, ref pos, '{') &&
                TryReadChar(format, ref pos, '0') &&
                TryReadChar(format, ref pos, ':'))
            {
                return true;
            }
            pos = start;
            return false;
        }

        private static bool TryReadChar(string format,
            ref int pos,
            char c)
        {
            var start = pos;
            if (format[pos] != c)
            {
                pos = start;
                return false;
            }
            pos++;
            return true;
        }
    }
}
