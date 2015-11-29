namespace Gu.Units
{
    using System.Collections.Concurrent;

    internal class FormatCache<TUnit> where TUnit : struct, IUnit
    {
        internal static readonly QuantityFormat<TUnit> DefaultFormat = CreateFromValueFormatAndUnit(new FormatKey<TUnit>(null, Unit<TUnit>.Default));
        private static readonly ConcurrentDictionary<FormatKey<TUnit>, QuantityFormat<TUnit>> Cache = new ConcurrentDictionary<FormatKey<TUnit>, QuantityFormat<TUnit>>();

        internal static QuantityFormat<TUnit> GetOrCreate(string compositeFormat)
        {
            return Cache.GetOrAdd(new FormatKey<TUnit>(compositeFormat), CompositeFormatParser.Create);
        }

        internal static QuantityFormat<TUnit> GetOrCreate(string valueFormat, TUnit unit)
        {
            return Cache.GetOrAdd(new FormatKey<TUnit>(valueFormat, unit), CreateFromValueFormatAndUnit);
        }

        public static QuantityFormat<TUnit> GetOrCreate(string valueFormat, string symbolFormat)
        {
            return Cache.GetOrAdd(new FormatKey<TUnit>(valueFormat, symbolFormat), CreateFromValueAndSymbolFormats);
        }

        private static QuantityFormat<TUnit> CreateFromValueFormatAndUnit(FormatKey<TUnit> key)
        {
            int pos = 0;
            var format = key.ValueFormat;

            string prePadding;
            string valueFormat;
            string postPadding;
            if (!DoubleFormatReader.TryRead(format, ref pos, out prePadding, out valueFormat, out postPadding))
            {
                valueFormat = FormatCache.UnknownFormat;
            }

            if (!format.IsRestWhiteSpace(pos))
            {
                valueFormat = FormatCache.UnknownFormat;
            }

            return QuantityFormat<TUnit>.CreateFromValueFormatAndUnit(prePadding, valueFormat, postPadding, key.Unit.Value);
        }

        private static QuantityFormat<TUnit> CreateFromValueAndSymbolFormats(FormatKey<TUnit> key)
        {
            int pos = 0;
            string valuePrePadding;
            string valueFormat;
            string valuePostPadding;
            if (!DoubleFormatReader.TryRead(
                key.ValueFormat,
                ref pos,
                out valuePrePadding,
                out valueFormat,
                out valuePostPadding))
            {
                valueFormat = FormatCache.UnknownFormat;
            }

            pos = 0;
            string symbolPrePadding;
            string symbolFormat;
            string symbolPostPadding;
            TUnit resultUnit;
            if (!UnitFormatReader.TryRead(
                key.SymbolFormat,
                ref pos,
                out symbolPrePadding,
                out symbolFormat,
                out symbolPostPadding,
                out resultUnit))
            {
                symbolFormat = FormatCache.UnknownFormat;
            }

            if (!symbolFormat.IsRestWhiteSpace(pos))
            {
                symbolFormat = FormatCache.UnknownFormat;
            }

            return QuantityFormat<TUnit>.CreateFromValueAndSymbolFormats(
                valuePrePadding,
                valueFormat,
                valuePostPadding,
                symbolPrePadding,
                symbolFormat,
                symbolPostPadding,
                resultUnit);
        }
    }
}
