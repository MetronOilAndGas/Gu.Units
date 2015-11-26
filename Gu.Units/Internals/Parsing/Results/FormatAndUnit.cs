namespace Gu.Units
{
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("Format: {_format} Unit: {_unit}")]
    internal struct FormatAndUnit<TUnit> : IEquatable<FormatAndUnit<TUnit>> 
        where TUnit: struct, IUnit
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

        public static bool operator ==(FormatAndUnit<TUnit> left, FormatAndUnit<TUnit> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(FormatAndUnit<TUnit> left, FormatAndUnit<TUnit> right)
        {
            return !left.Equals(right);
        }

        public bool Equals(FormatAndUnit<TUnit> other)
        {
            return string.Equals(this._format, other._format) && this._unit.Equals(other._unit);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is FormatAndUnit<TUnit> && Equals((FormatAndUnit<TUnit>)obj);
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