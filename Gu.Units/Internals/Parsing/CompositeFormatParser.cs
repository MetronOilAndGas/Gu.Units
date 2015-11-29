namespace Gu.Units
{
    internal static class CompositeFormatParser
    {
        internal static QuantityFormat<TUnit> Create<TUnit>(FormatKey<TUnit> key) where TUnit : struct, IUnit
        {
            QuantityFormat<TUnit> result;
            TryParse(key.CompositeFormat, out result);
            return result;
        }

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

            string prePadding = null;
            format.TryReadPadding(ref pos, out prePadding);
            string valueFormat;
            if (!DoubleFormatReader.TryRead(format, ref pos, out valueFormat))
            {
                valueFormat = FormatCache.UnknownFormat;
            }

            string padding = null;
            format.TryReadPadding(ref pos, out padding);
            string symbolFormat;
            TUnit unit;
            if (!UnitFormatReader.TryRead(format, ref pos, out symbolFormat, out unit))
            {
                symbolFormat = FormatCache.UnknownFormat;
            }

            string postPadding = null;
            format.TryReadPadding(ref pos, out postPadding);
            if (!format.IsRestWhiteSpace(ref pos, end))
            {
                symbolFormat = FormatCache.UnknownFormat;
            }

            if (valueFormat == FormatCache.UnknownFormat &&
                symbolFormat == FormatCache.UnknownFormat)
            {
                result = QuantityFormat<TUnit>.CreateUnknown(format, unit);
            }
            else
            {
                result = QuantityFormat<TUnit>.CreateFromParsedCompositeFormat(prePadding, valueFormat, padding, symbolFormat, postPadding, unit);
            }

            return valueFormat != FormatCache.UnknownFormat && symbolFormat != FormatCache.UnknownFormat;
        }
   }
}
