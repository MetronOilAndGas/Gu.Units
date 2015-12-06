namespace Gu.Units.Generator
{
    using System;
    using System.Linq;
    using Gu.Units.Generator.WpfStuff;
    /// <summary>
    /// http://en.wikipedia.org/wiki/SI_derived_unit
    /// </summary>
    [Serializable]
    public class DerivedUnit : BaseUnit
    {
        private readonly UnitParts parts;

        public DerivedUnit()
            : base(null, null, null)
        {
            this.parts = new UnitParts(this);
        }

        public DerivedUnit(string name, string symbol, string quantityName, params UnitAndPower[] parts)
            : base(name, symbol, quantityName)
        {
            if (parts.Length == 0)
            {
                throw new ArgumentException("No units", "units");
            }

            if (parts.Length != parts.Select(x => x.Unit.Name).Distinct().Count())
            {
                throw new ArgumentException("Units must be distinct", nameof(parts));
            }

            this.parts = new UnitParts(this);

            var unitAndPowers = parts.OrderBy(x => x.Unit.Name).ThenBy(x => x.Power).ToList();
            foreach (var unitAndPower in unitAndPowers)
            {
                this.parts.Add(unitAndPower);
            }
        }

        public UnitParts Parts
        {
            get
            {
                return this.parts;
            }
            set // Needed for the converter to work
            {
                this.parts.Clear();
                this.parts.InvokeAddRange(value);
            }
        }

        //[XmlIgnore]
        //public override string UiName => Parts.Expression;

        //public override string ToString()
        //{
        //    return $"{Symbol}  ({this.UiName}) ({(this.Quantity == null ? "null" : this.Quantity.ClassName)})";
        //}
    }
}