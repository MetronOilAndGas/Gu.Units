namespace Gu.Units
{
    using System.Collections.Generic;
    using System.Text;
    using System.Text.RegularExpressions;

    internal static class FormatParser
    {
        internal static readonly IReadOnlyList<string> DoubleFormatPatterns = new[]
        {
            CreateDoubleFormatPattern(@"(E|e)\d*"),
            CreateDoubleFormatPattern(@"(F|f)\d*"),
            CreateDoubleFormatPattern(@"(G|g)\d*"),
            CreateDoubleFormatPattern(@"(N|n)\d*"),
            CreateDoubleFormatPattern(@"(R|r)"),
            CreateDoubleFormatPattern(@"[0,#]*(\.[0,#]+)?"),
        };

        internal static bool TryParse<TUnit>(string format, out QuantityFormat<TUnit> result) 
            where TUnit : IUnit
        {
            int pos = 0;
            string doubleFormat;
            format.ReadWhiteSpace(ref pos);
            var prePaddingEnd = pos;
            var readDoubleFormat = TryReadDoubleFormat(format, ref pos, out doubleFormat);

            var spaceStart = pos;
            format.ReadWhiteSpace(ref pos);
            var spaceEnd = pos;
            string unitFormat;
            TUnit unit;
            var readUnitFormat = TryReadUnit(format, ref pos, out unitFormat, out unit);
            var unitEnd = pos;
            format.ReadWhiteSpace(ref pos);

            if (!(readDoubleFormat || readUnitFormat) || pos != format.Length)
            {
                result = QuantityFormat<TUnit>.Default;
                return false;
            }

            var compositeFormat = CreateCompositeFormat(format, prePaddingEnd, doubleFormat, spaceStart, spaceEnd, unitFormat ?? unit.Symbol, unitEnd);
            result = new QuantityFormat<TUnit>(compositeFormat, unit);
            return true;
        }

        internal static bool TryParse<TUnit>(string format, TUnit unit, out QuantityFormat<TUnit> result)
            where TUnit : IUnit
        {
            int pos = 0;
            string doubleFormat;
            format.ReadWhiteSpace(ref pos);
            var prePaddingEnd = pos;
            var readDoubleFormat = TryReadDoubleFormat(format, ref pos, out doubleFormat);
            var spaceStart = pos;
            format.ReadWhiteSpace(ref pos);
            var spaceEnd = pos;
            string unitFormat;
            TUnit readUnit;
            var readUnitFormat = TryReadUnit(format, ref pos, out unitFormat, out readUnit);
            var unitEnd = pos;

            format.ReadWhiteSpace(ref pos);

            if (!(readDoubleFormat || readUnitFormat) || pos != format.Length)
            {
                result = QuantityFormat<TUnit>.Default;
                return false;
            }

            if (readUnitFormat && !Equals(readUnit, unit))
            {
                // choosing the format unit here
                var cf = CreateCompositeFormat(format, prePaddingEnd, doubleFormat, spaceStart, spaceEnd, unitFormat, unitEnd);
                result = new QuantityFormat<TUnit>(cf, readUnit);
                return false;
            }

            var compositeFormat = CreateCompositeFormat(format, prePaddingEnd, doubleFormat, spaceStart, spaceEnd, unitFormat, unitEnd);
            result = new QuantityFormat<TUnit>(compositeFormat, unit);
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
                return false;
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

        private static string CreateDoubleFormatPattern(string format)
        {
            return $"^ *(?<format>{format})";
        }

        private static string CreateCompositeFormat(string format,
            int prePaddingEnd,
            string doubleFormat,
            int spaceStart,
            int spaceEnd,
            string unitFormat,
            int unitEnd)
        {
            var builder = new StringBuilder();
            for (int i = 0; i < prePaddingEnd; i++)
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

            builder.Append(unitFormat);

            for (int i = unitEnd; i < format.Length; i++)
            {
                builder.Append(format[i]);
            }
            var compositeFormat = builder.ToString();
            return compositeFormat;
        }
    }
}
