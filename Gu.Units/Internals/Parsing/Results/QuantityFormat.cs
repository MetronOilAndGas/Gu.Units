namespace Gu.Units
{
    using System.Diagnostics;

    [DebuggerDisplay("{Format}")]
    internal class QuantityFormat<TUnit>  where TUnit : struct, IUnit
    {
        public static QuantityFormat<TUnit> Default => FormatParser<TUnit>.DefaultFormat;

        internal QuantityFormat(string format, TUnit unit)
        {
            this.Format = format;
            this.Unit = unit;
        }

        public string Format { get; }

        internal TUnit Unit { get; }
    }
}