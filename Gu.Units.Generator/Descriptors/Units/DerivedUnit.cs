namespace Gu.Units.Generator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// http://en.wikipedia.org/wiki/SI_derived_unit
    /// </summary>
    public class DerivedUnit : BaseUnit
    {
        public DerivedUnit(string name, string symbol, string quantityName, IReadOnlyList<UnitAndPower> parts)
            : base(name, symbol, quantityName)
        {
            if (parts.Count == 0)
            {
                throw new ArgumentException("No units", "units");
            }

            if (parts.Count != parts.Select(x => x.UnitName).Distinct().Count())
            {
                throw new ArgumentException("Units must be distinct", nameof(parts));
            }

            Parts = new UnitParts(parts);
        }

        public UnitParts Parts { get; }
    }
}