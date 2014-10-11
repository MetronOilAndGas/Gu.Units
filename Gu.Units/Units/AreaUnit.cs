namespace Gu.Units
{
    public class AreaUnit : CompositeUnit
    {
        public AreaUnit()
            : base(new PowerUnit<ILengthUnit>(LengthUnit.Metres, 2))
        {
        }
    }
}