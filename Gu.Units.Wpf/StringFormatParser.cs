namespace Gu.Units.Wpf
{
    using System;
    using System.Diagnostics;

    internal static class StringFormatParser
    {
        internal static bool TryParse<TUnit>(string format,
            out QuantityFormat<TUnit> result)
            where TUnit : struct, IUnit
        {
            int pos = 0;
            format.ReadWhiteSpace(ref pos);
            if (!TryReadPrefix(format, ref pos))
            {
                result = QuantityFormat<TUnit>.Default;
                return false;
            }

            var lastIndexOf = format.LastIndexOf('}');
            if (lastIndexOf < 0)
            {
                result = QuantityFormat<TUnit>.Default;
                return false;
            }

            if (!format.IsRestWhiteSpace(lastIndexOf + 1))
            {
                result = QuantityFormat<TUnit>.Default;
                return false;
            }

            return FormatParser.TryParse(format, ref pos, lastIndexOf - 1, out result);
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
