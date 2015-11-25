namespace Gu.Units
{
    internal struct QuantityFormat<T> where T : IUnit
    {
        internal readonly string ValueFormat;

        internal readonly string UnitFormat;

        internal readonly T Unit;

        public QuantityFormat(string valueFormat,
            string unitFormat,
            T unit)
        {
            this.ValueFormat = valueFormat;
            this.UnitFormat = unitFormat;
            this.Unit = unit;
        }
    }
}