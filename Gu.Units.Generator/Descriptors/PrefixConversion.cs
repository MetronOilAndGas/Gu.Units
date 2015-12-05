namespace Gu.Units.Generator
{
    public class PrefixConversion
    {
        public PrefixConversion(IUnit unit, Prefix prefix)
        {
            this.Unit = unit;
            this.Prefix = prefix;
            this.Formula = new ConversionFormula(unit);
        }

        public IUnit Unit { get; }

        public Prefix Prefix { get; }

        public ConversionFormula Formula { get; }
    }
}
