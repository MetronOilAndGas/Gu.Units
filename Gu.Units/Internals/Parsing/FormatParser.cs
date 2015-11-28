namespace Gu.Units
{
    internal static class FormatParser
    {
        internal static bool TryParse<TUnit>(string format, out QuantityFormat<TUnit> result)
            where TUnit : struct, IUnit
        {
            int pos = 0;
            return TryParse(format, ref pos, format?.Length ?? 0, out result);
        }

        internal static bool TryParse<TUnit>(string format, ref int pos, int end, out QuantityFormat<TUnit> result)
                where TUnit : struct, IUnit
        {
            if (string.IsNullOrWhiteSpace(format))
            {
                result = QuantityFormat<TUnit>.CreateUnknown("null", Unit<TUnit>.Default);
                return true;
            }

            var prePaddingStart = pos;
            string doubleFormat;
            format.ReadWhiteSpace(ref pos);
            var prePaddingEnd = pos;
            DoubleFormatReader.TryReadDoubleFormat(format, ref pos, out doubleFormat);

            var padStart = pos;
            format.ReadWhiteSpace(ref pos);
            var padEnd = pos;
            string symbolFormat;
            TUnit unit;
            TryReadUnit(format, ref pos, out symbolFormat, out unit);
            var symbolEnd = pos;

            if (!format.IsRestWhiteSpace(ref pos, end))
            {
                result = QuantityFormat<TUnit>.CreateUnknown(format, Unit<TUnit>.Default);
                pos = padStart;
                return false;
            }

            var prePadding = GetPrePadding(format, prePaddingStart, prePaddingEnd, doubleFormat);
            var padding = GetPadding(format, doubleFormat, padStart, padEnd, symbolFormat);
            var postPadding = GetPostPadding(format, symbolEnd, pos);
            result = new QuantityFormat<TUnit>(prePadding, doubleFormat, padding, symbolFormat, postPadding, unit);
            return true;
        }

        private static bool TryParse<TUnit>(string format, TUnit unit, out QuantityFormat<TUnit> result)
            where TUnit : struct, IUnit
        {
            int pos = 0;
            string doubleFormat;
            var prePaddingStart = pos;
            format.ReadWhiteSpace(ref pos);
            var prePaddingEnd = pos;
            DoubleFormatReader.TryReadDoubleFormat(format, ref pos, out doubleFormat);
            var padStart = pos;
            format.ReadWhiteSpace(ref pos);
            var padEnd = pos;
            string symbolFormat;
            TUnit readUnit;
            var readUnitFormat = TryReadUnit(format, ref pos, out symbolFormat, out readUnit);
            var symbolEnd = pos;

            if (!format.IsRestWhiteSpace(pos))
            {
                result = QuantityFormat<TUnit>.CreateUnknown(format, unit);
                return false;
            }

            var prePadding = GetPrePadding(format, prePaddingStart, prePaddingEnd, doubleFormat);
            var padding = GetPadding(format, doubleFormat, padStart, padEnd, symbolFormat);
            var postPadding = GetPostPadding(format, symbolEnd, pos);

            if (readUnitFormat && !Equals(readUnit, unit))
            {
                // choosing the parsed format for symbol
                result = new QuantityFormat<TUnit>(prePadding, doubleFormat, padding, symbolFormat, postPadding, unit);
                return false;
            }

            result = new QuantityFormat<TUnit>(prePadding, doubleFormat, padding, symbolFormat, postPadding, unit);
            return true;
        }

        internal static QuantityFormat<TUnit> Create<TUnit>(FormatAndUnit<TUnit> fau)
            where TUnit : struct, IUnit
        {
            if (string.IsNullOrEmpty(fau._format))
            {
                var unit = fau._unit ?? Unit<TUnit>.Default;
                return new QuantityFormat<TUnit>(null, null, null, unit.Symbol, null, unit);
            }

            QuantityFormat<TUnit> result;
            if (fau._unit.HasValue)
            {
                TryParse(fau._format, fau._unit.Value, out result);
            }
            else
            {
                TryParse(fau._format, out result);
            }
            return result;
        }

        private static bool TryReadUnit<TUnit>(string format,
            ref int pos,
            out string unitFormat,
            out TUnit unit) where TUnit : struct, IUnit
        {
            var start = pos;
            if (pos == format.Length)
            {
                unit = Unit<TUnit>.Default;
                unitFormat = null;
                return false;
            }

            if (UnitParser<TUnit>.TryParse(format, ref pos, out unit))
            {
                unitFormat = format.Substring(start, pos - start);
                return true;
            }

            unit = Unit<TUnit>.Default;
            unitFormat = null;
            return false;
        }

        private static string GetPrePadding(string format, int startPos, int endPos, string doubleFormat)
        {
            if (startPos == endPos || string.IsNullOrEmpty(doubleFormat))
            {
                return null;
            }

            return format.Substring(startPos, endPos);
        }

        private static string GetPadding(string format,
            string doubleFormat,
            int spaceStart,
            int spaceEnd,
            string unitFormat)
        {
            if (spaceStart == format.Length)
            {
                return null;
            }

            if (spaceStart == spaceEnd)
            {
                if (string.IsNullOrEmpty(doubleFormat) || string.IsNullOrEmpty(unitFormat))
                {
                    return null;
                }

                return string.Empty;
            }

            return format.Substring(spaceStart, spaceEnd - spaceStart);
        }

        private static string GetPostPadding(string format, int symbolEnd, int postPaddingEnd)
        {
            if (symbolEnd == postPaddingEnd || symbolEnd == format.Length)
            {
                return null;
            }

            return format.Substring(symbolEnd, postPaddingEnd - symbolEnd);
        }
    }
}
