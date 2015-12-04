namespace Gu.Units.Generator.Tests
{
    using System.Collections;
    using System.Collections.Generic;

    public class ConversionFormulaComparer : IComparer<ConversionFormula>, IComparer
    {
        public static readonly ConversionFormulaComparer Default = new ConversionFormulaComparer();

        private ConversionFormulaComparer()
        {
        }

        public int Compare(ConversionFormula x, ConversionFormula y)
        {
            if (x.ConversionFactor != y.ConversionFactor)
            {
                return -1;
            }

            if (x.Offset != y.Offset)
            {
                return -1;
            }

            return 0;
        }

        int IComparer.Compare(object x, object y)
        {
            return Compare((ConversionFormula) x, (ConversionFormula) y);
        }
    }
}