namespace Gu.Units
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.RegularExpressions;

    internal static class FormatParser
    {
        internal static readonly IReadOnlyList<string> DoubleFormatPatterns = new[]
        {
            CreateDoubleFormat(@"(E|e)\d*"),
            CreateDoubleFormat(@"(F|f)\d*"),
            CreateDoubleFormat(@"(G|g)\d*"),
            CreateDoubleFormat(@"(N|n)\d*"),
            CreateDoubleFormat(@"(R|r)"),
            CreateDoubleFormat(@"[0,#]*(\.[0,#]+)?"),
        };

        internal static bool TryParse<TUnit>(
            string format,
            out QuantityFormat<TUnit> result) where TUnit : IUnit
        {
            int pos = 0;
            string doubleFormat;
            string unitFormat;
            TUnit unit;
            format.ReadWhiteSpace(ref pos);
            var start = pos;
            var readDoubleFormat = TryReadDoubleFormat(format, ref pos, out doubleFormat);

            var spaceStart = pos;
            format.ReadWhiteSpace(ref pos);
            var spaceEnd = pos;

            var readUnitFormat = TryReadUnit(format, ref pos, out unitFormat, out unit);
            if (!(readDoubleFormat || readUnitFormat))
            {
                result = QuantityFormat<TUnit>.Default;
                return false;
            }

            var builder = new StringBuilder();
            for (int i = 0; i < start; i++)
            {
                builder.Append(format[i]);
            }

            if (!string.IsNullOrEmpty(doubleFormat))
            {
                builder.Append($"{{0:{doubleFormat}}}");
            }
            else
            {
                builder.Append($"{{0}}");
            }

            for (int i = spaceStart; i < spaceEnd; i++)
            {
                builder.Append(format[i]);
            }

            if (readUnitFormat)
            {
                builder.Append(unitFormat ?? unit.Symbol);
            }

            for (int i = pos; i < format.Length; i++)
            {
                builder.Append(format[i]);
            }
            result = new QuantityFormat<TUnit>(builder.ToString(), unit);
            return true;
        }

        private static bool TryReadDoubleFormat(string format, ref int pos, out string doubleFormat)
        {
            foreach (var pattern in DoubleFormatPatterns)
            {
                var match = Regex.Match(format, pattern, RegexOptions.Singleline | RegexOptions.ExplicitCapture);
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
            out string unitFormat,
            out TUnit unit) where TUnit : IUnit
        {
            var start = pos;
            if (pos == format.Length)
            {
                unit = (TUnit)default(TUnit).SiUnit;
                unitFormat = unit.Symbol;
                return true;
            }

            if (UnitParser.TryParse<TUnit>(format, ref pos, out unit))
            {
                unitFormat = format.Substring(start, pos - start);
                return true;
            }

            unit = (TUnit)default(TUnit).SiUnit;
            unitFormat = unit.Symbol;
            return false;
        }

        private static string CreateDoubleFormat(string format)
        {
            return $"^ *(?<format>{format})";
        }
    }
}
