namespace Gu.Units
{
    public struct Area : IValue1<I2<ILengthUnit>>
    {
        /// <summary>
        /// The value in <see cref="T:Gu.Units.Area"/>.
        /// </summary>
        public readonly double MetresSquared;

        public Area(double metresSquared)
        {
            MetresSquared = metresSquared;
        }
    }
}