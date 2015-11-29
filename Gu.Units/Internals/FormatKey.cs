namespace Gu.Units
{
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("CompositeFormat: {CompositeFormat}, ValueFormat: {ValueFormat}, SymbolFormat: {SymbolFormat}, Unit: {Unit}")]
    internal struct FormatKey<TUnit> : IEquatable<FormatKey<TUnit>> where TUnit: struct, IUnit
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

        public static bool operator ==(FormatKey<TUnit> left, FormatKey<TUnit> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(FormatKey<TUnit> left, FormatKey<TUnit> right)
        {
            return !left.Equals(right);
        }

        public bool Equals(FormatKey<TUnit> other)
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
            return obj is FormatKey<TUnit> && Equals((FormatKey<TUnit>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.CompositeFormat?.GetHashCode() ?? 0;
                hashCode = (hashCode*397) ^ (this.ValueFormat?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ (this.SymbolFormat?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ this.Unit.GetHashCode();
                return hashCode;
            }
        }
    }
}