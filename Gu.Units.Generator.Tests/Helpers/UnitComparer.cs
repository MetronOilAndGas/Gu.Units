namespace Gu.Units.Generator.Tests
{
    using System.Collections;
    using System.Collections.Generic;

    public class UnitComparer : IComparer<IUnit>, IComparer
    {
        public static readonly UnitComparer Default = new UnitComparer();

        private UnitComparer()
        {
        }

        public int Compare(IUnit x, IUnit y)
        {
            if (x.GetType() != y.GetType())
            {
                return -1;
            }

            if (x.Symbol != y.Symbol)
            {
                return -1;
            }

            if (x.ClassName != y.ClassName)
            {
                return -1;
            }

            if (x.Conversions.Count != 0 || y.Conversions.Count != 0)
            {

            }

            return 0;
        }

        int IComparer.Compare(object x, object y)
        {
            return Compare((IUnit)x, (IUnit)y);
        }
    }
}