namespace Gu.Units
{
    internal static class UnitFormatReader<TUnit>
        where TUnit : struct, IUnit
    {
        internal static PaddedFormat TryRead(
            string format,
            out TUnit unit)
        {
            int pos = 0;
            string prePadding;
            format.TryRead(ref pos, out prePadding);
            string symbolFormat;
            var success = TryRead(format, ref pos, out symbolFormat, out unit);
            if (!(success && WhiteSpaceReader.IsRestWhiteSpace(format, pos)))
            {
                return PaddedFormat.CreateUnknown(prePadding, null);
            }

            string postPadding;
            format.TryRead(ref pos, out postPadding);
            return new PaddedFormat(prePadding, symbolFormat, postPadding);
        }

        internal static bool TryRead(
            string format,
            ref int pos,
            out string unitFormat,
            out TUnit unit)
        {
            var start = pos;
            if (pos == format.Length)
            {
                unit = Unit<TUnit>.Default;
                unitFormat = FormatCache.UnknownFormat;
                return false;
            }

            if (UnitParser<TUnit>.TryParse(format, ref pos, out unit))
            {
                unitFormat = format.Substring(start, pos - start);
                return true;
            }

            unit = Unit<TUnit>.Default;
            unitFormat = FormatCache.UnknownFormat;
            return false;
        }
    }
}