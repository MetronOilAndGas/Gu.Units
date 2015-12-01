namespace Gu.Units
{
    internal static class UnitFormatCache<TUnit>
        where TUnit : struct, IUnit
    {
        internal static PaddedFormat GetOrCreate(string format, out TUnit unit)
        {
            if (string.IsNullOrEmpty(format))
            {
                unit = Unit<TUnit>.Default;
                return PaddedFormat.NullFormat;
            }

            int pos = 0;
            return GetOrCreate(format, ref pos, out unit);
        }

        internal static PaddedFormat GetOrCreate(string format, ref int pos, out TUnit unit)
        {
            string prePadding;
            WhiteSpaceReader.TryRead(format, ref pos, out prePadding);
            var start = pos;
            if (format == null || pos == format.Length)
            {
                unit = Unit<TUnit>.Default;
                return PaddedFormat.CreateUnknown(prePadding, null);
            }

            if (UnitParser<TUnit>.TryParse(format, ref pos, out unit))
            {
                var symbolFormat = format.Substring(start, pos - start);
                string postPadding;
                WhiteSpaceReader.TryRead(format, ref pos, out postPadding);
                if (!WhiteSpaceReader.IsRestWhiteSpace(format, pos))
                {
                    unit = Unit<TUnit>.Default;
                    return PaddedFormat.CreateUnknown(prePadding, format.Substring(start));
                }

                return new PaddedFormat(prePadding, symbolFormat, postPadding);
            }

            unit = Unit<TUnit>.Default;
            return PaddedFormat.CreateUnknown(prePadding, format.Substring(pos));
        }
    }
}