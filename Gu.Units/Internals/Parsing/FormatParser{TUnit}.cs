namespace Gu.Units
{
    using System.Collections.Concurrent;

    internal class FormatParser<TUnit> where TUnit : struct, IUnit
    {
        internal static readonly QuantityFormat<TUnit> DefaultFormat = CompositeFormatParser.Create(new FormatAndUnit<TUnit>(null, Unit<TUnit>.Default));
        private static readonly ConcurrentDictionary<FormatAndUnit<TUnit>, QuantityFormat<TUnit>> Cache = new ConcurrentDictionary<FormatAndUnit<TUnit>, QuantityFormat<TUnit>>();

        internal static QuantityFormat<TUnit> GetOrCreate(string format)
        {
            return Cache.GetOrAdd(new FormatAndUnit<TUnit>(format), CompositeFormatParser.Create);
        }

        internal static QuantityFormat<TUnit> GetOrCreate(string format, TUnit unit)
        {
            return Cache.GetOrAdd(new FormatAndUnit<TUnit>(format, unit), CompositeFormatParser.Create);
        }
    }
}
