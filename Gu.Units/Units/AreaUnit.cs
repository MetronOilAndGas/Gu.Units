namespace Gu.Units.Tests
{
    public class AreaUnit : CompositeUnit
    {
        public AreaUnit()
            : base(new PowerUnit<ILengthUnit>(LengthUnit.Metres, 2))
        {
        }
    }
}