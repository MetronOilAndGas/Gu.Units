namespace Gu.Units
{
    internal static class UnitFormatReader
    {
        internal static bool TryRead<TUnit>(
            string format,
            ref int pos,
            out string prePadding,
            out string symbolFormat,
            out string postPadding,
            out TUnit unit) where TUnit : struct, IUnit
        {
            format.TryReadPadding(ref pos, out prePadding);
            var success = TryRead(format, ref pos, out symbolFormat, out unit);
            format.TryReadPadding(ref pos, out postPadding);
            return success;
        }

        internal static bool TryRead<TUnit>(
            string format,
            ref int pos,
            out string unitFormat,
            out TUnit unit) where TUnit : struct, IUnit
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