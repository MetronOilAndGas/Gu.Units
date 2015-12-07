namespace Gu.Units.Generator
{
    using System.Collections.Generic;
    using System.Linq;

    public static class OverloadFinder
    {
        public static void Find(IReadOnlyList<BaseUnit> units)
        {
            var quantities = units.Select(u => u.Quantity).ToList();
            FindOperatorOverloads(units.ToList(), quantities);
            FindInverseOverloads(quantities);
        }

        private static void FindOperatorOverloads(IReadOnlyList<BaseUnit> units, IReadOnlyList<Quantity> quantities)
        {
            foreach (var quantity in quantities)
            {
                quantity.OperatorOverloads.Clear();
                var overloads = quantities.Where(x => x.Name != quantity.Name)
                    .Where(result => OperatorOverload.CanCreate(units, quantity, result))
                    .Select(result => new OperatorOverload(quantity, result, units));

                foreach (var overload in overloads)
                {
                    quantity.OperatorOverloads.Add(overload);
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
