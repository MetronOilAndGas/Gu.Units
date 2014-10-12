namespace Gu.Units
{
    public interface IAreaUnit : IValue1<I2<ILengthUnit>>
    {
    }
    public interface IVolumeUnit : IValue1<I3<ILengthUnit>>
    {
    }
    public struct MetresSquared : IAreaUnit
    {
        public string Symbol { get; private set; }

        public double ToSiUnit(double value)
        {
            throw new System.NotImplementedException();
        }
    }
}