namespace Gu.Units
{
    internal static class CompositeFormatParser
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
                result = QuantityFormat<TUnit>.Default;
                return true;
            }

            var prePaddingStart = pos;
            format.TryReadWhiteSpace(ref pos);
            var prePaddingEnd = pos;
            string valueFormat;
            if (!DoubleFormatReader.TryReadDoubleFormat(format, ref pos, out valueFormat))
            {
                valueFormat = null;
                prePaddingEnd = prePaddingStart;
            }

            var padStart = pos;
            format.TryReadWhiteSpace(ref pos);
            var padEnd = pos;
            string symbolFormat;
            TUnit unit;
            if (!TryReadUnit(format, ref pos, out symbolFormat, out unit))
            {
                symbolFormat = null;
                padEnd = padStart;
            }
            var symbolEnd = pos;

            if (!format.IsRestWhiteSpace(ref pos, end))
            {
                result = QuantityFormat<TUnit>.CreateUnknown(format, Unit<TUnit>.Default);
                pos = padStart;
                return false;
            }

            var prePadding = GetPrePadding(format, prePaddingStart, prePaddingEnd, valueFormat);
            var padding = GetPadding(format, valueFormat, padStart, padEnd, symbolFormat);
            var postPadding = GetPostPadding(format, symbolEnd, pos);
            result = QuantityFormat<TUnit>.CreateFromParsedCompositeFormat(prePadding, valueFormat, padding, symbolFormat, postPadding, unit);
            return valueFormat != null && symbolFormat != null;
        }

        private static bool TryParse<TUnit>(string format, TUnit unit, out QuantityFormat<TUnit> result)
            where TUnit : struct, IUnit
        {
            int pos = 0;
            string valueFormat;
            var prePaddingStart = pos;
            format.TryReadWhiteSpace(ref pos);
            var prePaddingEnd = pos;
            if (!DoubleFormatReader.TryReadDoubleFormat(format, ref pos, out valueFormat))
            {
                valueFormat = null;
                prePaddingEnd = prePaddingStart;
            }

            var padStart = pos;
            format.TryReadWhiteSpace(ref pos);
            var padEnd = pos;
            string symbolFormat;
            TUnit readUnit;
            if (TryReadUnit(format, ref pos, out symbolFormat, out readUnit))
            {
                if (!Equals(readUnit, unit))
                {
                    symbolFormat = null;
                    padEnd = padStart;
                }
            }
            else
            {
                symbolFormat = unit.Symbol;
            }

            var symbolEnd = pos;

            if (!format.IsRestWhiteSpace(pos))
            {
                result = QuantityFormat<TUnit>.CreateUnknown(format, unit);
                return false;
            }

            var prePadding = GetPrePadding(format, prePaddingStart, prePaddingEnd, valueFormat);
            var padding = GetPadding(format, valueFormat, padStart, padEnd, symbolFormat);
            var postPadding = GetPostPadding(format, symbolEnd, pos);



            result = QuantityFormat<TUnit>.CreateFromParsedCompositeFormat(prePadding, valueFormat, padding, symbolFormat, postPadding, unit);
            return valueFormat != null && symbolFormat != null;
        }

        internal static QuantityFormat<TUnit> Create<TUnit>(FormatAndUnit<TUnit> fau)
            where TUnit : struct, IUnit
        {
            if (string.IsNullOrEmpty(fau._format))
            {
                var unit = fau._unit ?? Unit<TUnit>.Default;
                return new QuantityFormat<TUnit>(null, null, null, null, null, unit);
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
