namespace Gu.Units
{
    internal class QuantityFormat<TUnit>
        where TUnit : IUnit
    {
        //public static readonly QuantityFormat<TUnit> Default = new QuantityFormat<TUnit>("", (TUnit) default(TUnit).SiUnit);
        internal readonly string Format;

        internal readonly TUnit Unit;

        public QuantityFormat(string format, TUnit unit)
        {
            this.Format = format;
            this.Unit = unit;
        }
    }
}