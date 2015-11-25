namespace Gu.Units
{
    internal class QuantityFormat<T>
        where T : IUnit
    {
        public static readonly QuantityFormat<T> Default = new QuantityFormat<T>("", default(T).Symbol);
        internal readonly string Format;

        internal readonly T Unit;

        public QuantityFormat(string format, T unit)
        {
            this.Format = format;
            this.Unit = unit;
        }
    }
}