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
            var valueFormat = string.IsNullOrEmpty(key.ValueFormat)
                ? PaddedFormat.NullFormat
                : DoubleFormatReader.TryRead(key.ValueFormat);
            return QuantityFormat<TUnit>.CreateFromValueFormatAndUnit(valueFormat, key.Unit.Value);
        }

        private static QuantityFormat<TUnit> CreateFromValueAndSymbolFormats(FormatKey<TUnit> key)
        {
            var valueFormat = string.IsNullOrEmpty(key.ValueFormat)
                ? PaddedFormat.NullFormat
                : DoubleFormatReader.TryRead(key.ValueFormat);

            TUnit unit = Unit<TUnit>.Default;
            var symbolFormat = string.IsNullOrEmpty(key.SymbolFormat)
                ? PaddedFormat.NullFormat
                : UnitFormatReader<TUnit>.TryRead(key.SymbolFormat, out unit);

            return QuantityFormat<TUnit>.CreateFromValueAndSymbolFormats(
                valueFormat,
                symbolFormat,
                unit);
        }
    }
}
