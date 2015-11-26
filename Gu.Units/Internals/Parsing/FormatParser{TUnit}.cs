namespace Gu.Units
{
    using System;
    using System.Collections.Concurrent;

    internal class FormatParser<TUnit> where TUnit : struct, IUnit
    {
        private static readonly TUnit DefaultUnit = (TUnit)default(TUnit).SiUnit;
        private static readonly ConcurrentDictionary<FormatAndUnit, QuantityFormat<TUnit>> Cache = new ConcurrentDictionary<FormatAndUnit, QuantityFormat<TUnit>>();

        internal static QuantityFormat<TUnit> GetOrCreate(string format)
        {
            return Cache.GetOrAdd(new FormatAndUnit(format), Create);
        }

        internal static QuantityFormat<TUnit> GetOrCreate(string format, TUnit unit)
        {
            return Cache.GetOrAdd(new FormatAndUnit(format, unit), Create);
        }

        private static QuantityFormat<TUnit> Create(FormatAndUnit fau)
        {
            if (string.IsNullOrEmpty(fau._format))
            {
                var unit = fau._unit ?? DefaultUnit;
                if (ShouldSpace(unit.Symbol))
                {
                    return new QuantityFormat<TUnit>($"{0}\u00A0{unit.Symbol}", unit);
                }

                return new QuantityFormat<TUnit>($"{0}{unit.Symbol}", unit);
            }

            QuantityFormat<TUnit> result;
            FormatParser.TryParse(fau._format, out result);
            return result;
        }

        private static bool ShouldSpace(string symbol)
        {
            if (symbol.Length > 1)
            {
                return true;
            }
            return !char.IsLetter(symbol[0]);
        }

        private struct FormatAndUnit : IEquatable<FormatAndUnit>
        {
            internal readonly string _format;
            internal readonly TUnit? _unit;

            public FormatAndUnit(string format)
            {
                this._format = format;
                this._unit = null;
            }

            public FormatAndUnit(string format, TUnit unit)
            {
                this._format = format;
                this._unit = unit;
            }

            public static bool operator ==(FormatAndUnit left, FormatAndUnit right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(FormatAndUnit left, FormatAndUnit right)
            {
                return !left.Equals(right);
            }

            public bool Equals(FormatAndUnit other)
            {
                return string.Equals(this._format, other._format) && this._unit.Equals(other._unit);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                    return false;
                return obj is FormatAndUnit && Equals((FormatAndUnit)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((this._format?.GetHashCode() ?? 0) * 397) ^ this._unit.GetHashCode();
                }
            }
        }
    }
}
