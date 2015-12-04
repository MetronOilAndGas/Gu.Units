namespace Gu.Units.Generator.Tests
{
    using System.Collections;
    using System.Collections.Generic;

    public class PrefixConversionVmComparer : IComparer<PrefixConversionVm>, IComparer
    {
        public static readonly PrefixConversionVmComparer Default = new PrefixConversionVmComparer();

        private PrefixConversionVmComparer()
        {
        }

        public int Compare(PrefixConversionVm x, PrefixConversionVm y)
        {
            if (x.Unit.ClassName != y.Unit.ClassName)
            {
                return -1;
            }
            return PrefixComparer.Default.Compare(x.Prefix, y.Prefix);
        }

        int IComparer.Compare(object x, object y)
        {
            return Compare((PrefixConversionVm)x, (PrefixConversionVm)y);
        }
    }
}