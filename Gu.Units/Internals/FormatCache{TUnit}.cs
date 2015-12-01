namespace Gu.Units
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics;

    internal class FormatCache<TUnit> where TUnit : struct, IUnit
    {
        internal static readonly QuantityFormat<TUnit> DefaultFormat = CreateFromValueFormatAndUnit(new FormatKey(null, Unit<TUnit>.Default));
        private static readonly ConcurrentDictionary<FormatKey, QuantityFormat<TUnit>> Cache = new ConcurrentDictionary<FormatKey, QuantityFormat<TUnit>>();

        internal static QuantityFormat<TUnit> GetOrCreate(string compositeFormat)
        {
            return Cache.GetOrAdd(new FormatKey(compositeFormat), _ => CompositeFormatParser.Create<TUnit>(compositeFormat));
        }

        internal static QuantityFormat<TUnit> GetOrCreate(string valueFormat, TUnit unit)
        {
            return Cache.GetOrAdd(new FormatKey(valueFormat, unit), CreateFromValueFormatAndUnit);
        }

        public static QuantityFormat<TUnit> GetOrCreate(string valueFormat, string symbolFormat)
        {
            return Cache.GetOrAdd(new FormatKey(valueFormat, symbolFormat), CreateFromValueAndSymbolFormats);
        }

        private static QuantityFormat<TUnit> CreateFromValueFormatAndUnit(FormatKey key)
        {
            var valueFormat = DoubleFormatCache.GetOrCreate(key.ValueFormat);
            var unit = (key.Unit ?? Unit<TUnit>.Default); // this is just to shr R# up
            var symbolFormat = new PaddedFormat(null, unit.Symbol, null);
            return QuantityFormat<TUnit>.Create(valueFormat, symbolFormat, unit);
        }

        private static QuantityFormat<TUnit> CreateFromValueAndSymbolFormats(FormatKey key)
        {
            var valueFormat = DoubleFormatCache.GetOrCreate(key.ValueFormat);

            TUnit unit;
            var symbolFormat = UnitFormatCache<TUnit>.GetOrCreate(key.SymbolFormat, out unit);

            return QuantityFormat<TUnit>.Create(valueFormat, symbolFormat, unit);
        }

        [DebuggerDisplay("CompositeFormat: {CompositeFormat}, ValueFormat: {ValueFormat}, SymbolFormat: {SymbolFormat}, Unit: {Unit}")]
        internal struct FormatKey : IEquatable<FormatKey>
        {
            internal readonly string CompositeFormat;
            internal readonly string ValueFormat;
            internal readonly string SymbolFormat;
            internal readonly TUnit? Unit;

            public FormatKey(string compositeFormat)
            {
                this.CompositeFormat = compositeFormat;
                this.ValueFormat = null;
                this.SymbolFormat = null;
                this.Unit = null;
            }

            public FormatKey(string valueFormat, TUnit unit)
            {
                this.CompositeFormat = null;
                this.ValueFormat = valueFormat;
                this.SymbolFormat = null;
                this.Unit = unit;
            }

            public FormatKey(string valueFormat, string symbolFormat)
            {
                this.CompositeFormat = null;
                this.ValueFormat = valueFormat;
                this.SymbolFormat = symbolFormat;
                this.Unit = null;
            }

            public static bool operator ==(FormatKey left, FormatKey right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(FormatKey left, FormatKey right)
            {
                return !left.Equals(right);
            }

            public bool Equals(FormatKey other)
            {
                return string.Equals(this.CompositeFormat, other.CompositeFormat) &&
                       string.Equals(this.ValueFormat, other.ValueFormat) &&
                       string.Equals(this.SymbolFormat, other.SymbolFormat) &&
                       this.Unit.Equals(other.Unit);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                    return false;
                return obj is FormatKey && Equals((FormatKey)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = this.CompositeFormat?.GetHashCode() ?? 0;
                    hashCode = (hashCode * 397) ^ (this.ValueFormat?.GetHashCode() ?? 0);
                    hashCode = (hashCode * 397) ^ (this.SymbolFormat?.GetHashCode() ?? 0);
                    hashCode = (hashCode * 397) ^ this.Unit.GetHashCode();
                    return hashCode;
                }
            }
        }
    }
}
