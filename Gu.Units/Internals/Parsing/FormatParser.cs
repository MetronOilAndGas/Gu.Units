namespace Gu.Units
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    internal static class FormatParser
    {
        internal static readonly IReadOnlyList<string> DoubleFormatPatterns = new[]
        {
            CreateDoubleFormat(@"(?:E|e)\d*"),
            CreateDoubleFormat(@"(?:F|f)\d*"),
            CreateDoubleFormat(@"(?:G|g)\d*"),
            CreateDoubleFormat(@"(?:N|n)\d*"),
            CreateDoubleFormat(@"(?:R|r)"),
            CreateDoubleFormat(@"[0,#]*(?:\.[0,#]+)?"),
        };

        internal static bool TryParse<TUnit>(string format, TUnit @default, out QuantityFormat<TUnit> actual)
            where TUnit : IUnit
        {
            int pos = 0;
            string doubleFormat;
            if (!TryReadDoubleFormat(format, ref pos, out doubleFormat))
            {
                actual = new QuantityFormat<TUnit>("");
                return false;
            }

            string unitFormat;
            TUnit unit;
            if (!TryReadUnit(format, ref pos, @default, out unitFormat, out unit))
            {
                actual = new QuantityFormat<TUnit>();
                return false;
            }

            actual = new QuantityFormat<TUnit>(doubleFormat, unitFormat, unit);
            return true;
        }

        private static bool TryReadDoubleFormat(string format, ref int pos, out string doubleFormat)
        {
            foreach (var pattern in DoubleFormatPatterns)
            {
                var match = Regex.Match(format, pattern);
                if (match.Success && match.Value != string.Empty)
                {
                    pos = match.Index + match.Length;
                    doubleFormat = match.Value;
                    return true;
                }
            }

            doubleFormat = null;
            return false;
        }

        private static bool TryReadUnit<TUnit>(string format,
            ref int pos,
            TUnit @default,
            out string unitFormat,
            out TUnit unit) where TUnit : IUnit
        {
            var start = pos;
            format.ReadWhiteSpace(ref pos);
            if (pos == format.Length)
            {
                unit = @default;
                unitFormat = unit.Symbol;
                return true;
            }

            if (UnitParser.TryParse<TUnit>(format, ref pos, out unit))
            {
                unitFormat = format.Substring(start, pos - start);
                return true;
            }

            unit = @default;
            unitFormat = unit.Symbol;
            return false;
        }

        private static string CreateDoubleFormat(string format)
        {
            return $"^ *(?<format>{format})";
        }
    }
}
