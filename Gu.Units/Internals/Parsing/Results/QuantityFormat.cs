namespace Gu.Units
{
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("CompositeFormat: {CompositeFormat}")]
    internal class QuantityFormat<TUnit> : IEquatable<QuantityFormat<TUnit>> where TUnit : struct, IUnit
    {
        public static QuantityFormat<TUnit> Default => FormatParser<TUnit>.DefaultFormat;
        internal static readonly char NoBreakingSpace = '\u00A0';
        internal static readonly string NoBreakingSpaceString = "\u00A0";
        private string _compositeFormat;

        public QuantityFormat(string prePadding,
            string valueFormat,
            string padding,
            string symbolFormat,
            string postPadding,
            TUnit unit)
        {
            PrePadding = prePadding;
            ValueFormat = valueFormat;
            if ((valueFormat == null || symbolFormat == null) &&
                padding == null &&
                ShouldSpace(symbolFormat ?? unit.Symbol))
            {
                Padding = NoBreakingSpaceString;
            }
            else
            {
                Padding = padding;
            }

            SymbolFormat = symbolFormat;
            PostPadding = postPadding;
            ErrorFormat = null;
            Unit = unit;
        }

        public QuantityFormat(
            string errorFormat,
            TUnit unit)
        {
            ErrorFormat = errorFormat;
            Unit = unit;
        }

        internal string PrePadding { get; }

        internal string ValueFormat { get; }

        internal string Padding { get; }

        internal string SymbolFormat { get; }

        internal string PostPadding { get; }

        internal string ErrorFormat { get; }

        internal string CompositeFormat => _compositeFormat ?? (_compositeFormat = CreateCompositeFormat());

        internal TUnit Unit { get; }

        public static bool operator ==(QuantityFormat<TUnit> left, QuantityFormat<TUnit> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(QuantityFormat<TUnit> left, QuantityFormat<TUnit> right)
        {
            return !Equals(left, right);
        }

        public bool Equals(QuantityFormat<TUnit> other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(PrePadding, other.PrePadding) &&
                   string.Equals(ValueFormat, other.ValueFormat) &&
                   string.Equals(Padding, other.Padding) &&
                   string.Equals(SymbolFormat, other.SymbolFormat) &&
                   string.Equals(PostPadding, other.PostPadding) &&
                   string.Equals(ErrorFormat, other.ErrorFormat) &&
                   Unit.Equals(other.Unit);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((QuantityFormat<TUnit>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = PrePadding?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (ValueFormat?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (Padding?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (SymbolFormat?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (PostPadding?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (ErrorFormat?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ Unit.GetHashCode();
                return hashCode;
            }
        }

        internal static QuantityFormat<TUnit> CreateUnknown(string errorFormat, TUnit unit)
        {
            return new QuantityFormat<TUnit>(errorFormat, unit);
        }

        private static bool ShouldSpace(string symbol)
        {
            if (symbol.Length > 1)
            {
                return true;
            }
            return char.IsLetter(symbol[0]);
        }

        private string CreateCompositeFormat()
        {
            using (var builder = StringBuilderPool.Borrow())
            {
                if (ErrorFormat != null)
                {
                    builder.Append("Error: ");
                    builder.Append(ErrorFormat);
                }
                builder.Append(PrePadding);

                if (string.IsNullOrEmpty(ValueFormat))
                {
                    builder.Append("{0}");
                }
                else
                {
                    builder.Append("{0:");
                    builder.Append(ValueFormat);
                    builder.Append("}");
                }

                builder.Append(Padding);
                builder.Append(SymbolFormat ?? Unit.Symbol);
                builder.Append(PostPadding);
                var compositeFormat = builder.ToString();
                return compositeFormat;
            }
        }
    }
}