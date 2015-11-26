namespace Gu.Units
{
    internal class QuantityFormat<TUnit>  where TUnit : IUnit
    {
        public static readonly QuantityFormat<TUnit> Default = new QuantityFormat<TUnit>("", (TUnit)default(TUnit).SiUnit);

        internal QuantityFormat(string format, TUnit unit)
        {
            this.Format = format;
            this.Unit = unit;
        }

        public string Format { get; }

        internal TUnit Unit { get; }
    }
}