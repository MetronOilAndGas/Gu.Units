namespace Gu.Units.Generator.Tests
{
    using System.Collections;
    using System.Collections.Generic;

    public class ConversionComparer : IComparer<Conversion>, IComparer
    {
        public static readonly ConversionComparer Default = new ConversionComparer();

        private ConversionComparer()
        {
        }

        public int Compare(Conversion x, Conversion y)
        {
            if (ConversionFormulaComparer.Default.Compare(x.Formula, y.Formula) != 0)
            {
                return -1;
            }

            if (UnitComparer.Default.Compare(x, y) != 0)
            {
                return -1;
            }

            if (x.Symbol != y.Symbol)
            {
                return -1;
            }


            return 0;
        }

        int IComparer.Compare(object x, object y)
        {
            return Compare((Conversion)x, (Conversion)y);
        }
    }
}