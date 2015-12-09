namespace Gu.Units.Generator
{
    using System.Collections.Generic;
    using System.Linq;

    public static class OverloadFinder
    {
        public static void Find(IReadOnlyList<Unit> units)
        {
            var quantities = units.Select(u => u.Quantity).ToList();
            FindOperatorOverloads(units.ToList(), quantities);
            FindInverseOverloads(quantities);
        }

        private static void FindOperatorOverloads(IReadOnlyList<Unit> units, IReadOnlyList<Quantity> quantities)
        {
            foreach (var left in quantities)
            {
                left.OperatorOverloads.Clear();

                foreach (var right in quantities)
                {
                    OperatorOverload overload;
                    if (OperatorOverload.TryCreateMultiplication(left, right, units, out overload))
                    {
                        left.OperatorOverloads.Add(overload);
                    }

                    if (OperatorOverload.TryCreateDivision(left, right, units, out overload))
                    {
                        left.OperatorOverloads.Add(overload);
                    }
                }
            }
        }

        private static void FindInverseOverloads(IReadOnlyList<Quantity> quantities)
        {
            foreach (var quantity in quantities)
            {
                var match = quantities.FirstOrDefault(x => InverseOverload.IsInverse(quantity, x));

                if (match == null)
                {
                    quantity.Inverse = null;
                }
                else
                {
                    quantity.Inverse = new InverseOverload(quantity, match);
                }
            }
        }
    }
}
