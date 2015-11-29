namespace Gu.Units
{
    using System.Collections.Concurrent;

    internal class FormatCache<TUnit> where TUnit : struct, IUnit
    {

        internal static readonly QuantityFormat<TUnit> DefaultFormat = CreateFromValueAndUnit(new FormatKey<TUnit>(null, Unit<TUnit>.Default));
        private static readonly ConcurrentDictionary<FormatKey<TUnit>, QuantityFormat<TUnit>> Cache = new ConcurrentDictionary<FormatKey<TUnit>, QuantityFormat<TUnit>>();

        internal static QuantityFormat<TUnit> GetOrCreate(string compositeFormat)
        {
            return Cache.GetOrAdd(new FormatKey<TUnit>(compositeFormat), CompositeFormatParser.Create);
        }

        internal static QuantityFormat<TUnit> GetOrCreate(string valueFormat, TUnit unit)
        {
            return Cache.GetOrAdd(new FormatKey<TUnit>(valueFormat, unit), CreateFromValueAndUnit);
        }

        public static QuantityFormat<TUnit> GetOrCreate(string valueFormat, string symbolFormat)
        {
            return Cache.GetOrAdd(new FormatKey<TUnit>(valueFormat, symbolFormat), CreateFromValueAndSymbol);
        }

        private static QuantityFormat<TUnit> CreateFromValueAndUnit(FormatKey<TUnit> key)
        {
            int pos = 0;
            string prePadding;
            var format = key.ValueFormat;
            format.TryReadPadding(ref pos, out prePadding);

            string valueFormat;
            if (!DoubleFormatReader.TryReadDoubleFormat(format, ref pos, out valueFormat))
            {
                valueFormat = FormatCache.UnknownFormat;
            }

            if (!format.IsRestWhiteSpace(pos))
            {
                valueFormat = FormatCache.UnknownFormat;
            }

            string padding;
            format.TryReadPadding(ref pos, out padding);
            return QuantityFormat<TUnit>.CreateFromValueAndSymbolFormats(prePadding, valueFormat, padding, key.Unit.Value);
        }

        private static QuantityFormat<TUnit> CreateFromValueAndSymbol(FormatKey<TUnit> key)
        {
            int pos = 0;
            string prePadding;
            var format = key.ValueFormat;
            format.TryReadPadding(ref pos, out prePadding);

            string valueFormat;
            if (!DoubleFormatReader.TryReadDoubleFormat(format, ref pos, out valueFormat))
            {
                valueFormat = FormatCache.UnknownFormat;
            }

            string valuePadding;
            format.TryReadPadding(ref pos, out valuePadding);
            pos = 0;
            string symbolPadding;
            key.SymbolFormat.TryReadPadding(ref pos, out symbolPadding);
            string symbolFormat;
            TUnit resultUnit;
            if (!UnitParser<TUnit>.TryParse(key.SymbolFormat, ref pos, out resultUnit))
            {
                symbolFormat = FormatCache.UnknownFormat;
            }
            else
            {
                symbolFormat = key.SymbolFormat.Substring(symbolPadding?.Length ?? 0, pos);
            }

            if (!format.IsRestWhiteSpace(pos))
            {
                symbolFormat = FormatCache.UnknownFormat;
            }
            string postPadding = null;
            key.SymbolFormat.TryReadPadding(ref pos, out postPadding);
            return QuantityFormat<TUnit>.CreateFromValueAndSymbolFormats(
                prePadding,
                valueFormat, 
                valuePadding,
                symbolPadding, 
                symbolFormat, 
                postPadding, 
                resultUnit);
        }
    }
}
