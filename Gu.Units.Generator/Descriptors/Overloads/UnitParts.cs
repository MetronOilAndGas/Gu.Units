namespace Gu.Units.Generator
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    using Gu.Units.Generator.WpfStuff;

    [TypeConverter(typeof(UnitPartsConverter))]
    public class UnitParts : IReadOnlyList<UnitAndPower>
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        private ReadonlySet<UnitAndPower> baseParts;

        public UnitParts(IReadOnlyList<UnitAndPower> parts)
        {
            this.Parts = parts;
        }

        public IReadOnlyList<UnitAndPower> Parts { get; }

        public int Count => Parts.Count;

        public UnitAndPower this[int index] => Parts[index];

        internal ReadonlySet<UnitAndPower> BaseParts => this.baseParts ?? (this.baseParts = CreateBaseParts());

        public string Expression => CreateExpression(Parts);

        public string BaseUnitExpression => CreateExpression(BaseParts);

        public static UnitParts operator *(UnitParts left, UnitParts right)
        {
            var result = left.BaseParts.ToList();
            foreach (var rightPart in right.BaseParts)
            {
                var match = result.SingleOrDefault(x => x.UnitName == rightPart.UnitName);
                if (match != null)
                {
                    result.Remove(match);
                    var power = match.Power + rightPart.Power;
                    if (power != 0)
                    {
                        result.Add(new UnitAndPower(match.UnitName, power));
                    }
                }
                else
                {
                    result.Add(rightPart);
                }
            }

            return new UnitParts(result);
        }

        public static UnitParts operator /(UnitParts left, UnitParts right)
        {
            var inverse = right.Inverse();
            return left * inverse;
        }

        public static bool operator ==(UnitParts left, UnitParts right)
        {
            if (Equals(left, null) && Equals(right, null))
            {
                return true;
            }
            if (Equals(left, null) || Equals(right, null))
            {
                return false;
            }

            return left.BaseParts == right.BaseParts;
        }

        public static bool operator !=(UnitParts left, UnitParts right)
        {
            return !(left == right);
        }

        public IEnumerator<UnitAndPower> GetEnumerator() => Parts.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override string ToString()
        {
            return this.Expression;
        }

        private ReadonlySet<UnitAndPower> CreateBaseParts()
        {
            var all = new List<UnitAndPower>();
            foreach (var part in Parts)
            {
                GetBaseParts(part, part.Power, all);
            }

            var distinct = all.Select(x => x.Unit).Distinct().ToArray();
            var summed = new List<UnitAndPower>();
            foreach (BaseUnit unit in distinct)
            {
                var sum = all.Where(x => x.Unit.Name == unit.Name).Sum(x => x.Power);
                if (sum != 0)
                {
                    summed.Add(UnitAndPower.Create(unit, sum));
                }
            }

            return new ReadonlySet<UnitAndPower>(summed);
        }

        private void GetBaseParts(UnitAndPower up, int power, List<UnitAndPower> list)
        {
            if (list.Count > 100)
            {
                Debugger.Break(); // Looks like SO will happen
            }
            if (up.Unit.GetType() == typeof(BaseUnit))
            {
                list.Add(UnitAndPower.Create(up.Unit, power));
                return;
            }
            var derivedUnit = (DerivedUnit)up.Unit;
            foreach (var unitPart in derivedUnit.Parts)
            {
                GetBaseParts(unitPart, unitPart.Power * power, list);
            }
        }

        private string CreateExpression(IReadOnlyCollection<UnitAndPower> ups)
        {
            if (!this.Any())
            {
                return "";
            }
            var sb = new StringBuilder();
            UnitAndPower previous = null;
            foreach (var unitAndPower in ups.OrderBy(x => x, BaseUnitOrderComparer.Default).ToArray())
            {
                if (previous != null)
                {
                    sb.Append("⋅");
                }
                sb.Append(unitAndPower.Unit == null ? unitAndPower.Unit.Name : unitAndPower.Unit.Symbol);
                if (unitAndPower.Power < 0)
                {
                    sb.Append("⁻");
                    if (unitAndPower.Power == -1)
                    {
                        sb.Append("¹");
                    }
                }
                switch (Math.Abs(unitAndPower.Power))
                {
                    case 1:
                        break;
                    case 2:
                        sb.Append("²");
                        break;
                    case 3:
                        sb.Append("³");
                        break;
                    case 4:
                        sb.Append("⁴");
                        break;
                    default:
                        sb.Append("^")
                          .Append(Math.Abs(unitAndPower.Power));
                        break;
                }
                previous = unitAndPower;
            }
            return sb.ToString();
        }

        public class BaseUnitOrderComparer : IComparer<UnitAndPower>
        {
            public static readonly BaseUnitOrderComparer Default = new BaseUnitOrderComparer();

            private BaseUnitOrderComparer()
            {
            }

            private readonly string[] _order = { "kg", "m", "s", "A", "cd", "mol" };
            public int Compare(UnitAndPower x, UnitAndPower y)
            {
                var indexOfX = Array.IndexOf(_order, x.Unit.Symbol);
                var indexOfY = Array.IndexOf(_order, y.Unit.Symbol);
                if (indexOfX < 0 && indexOfY < 0)
                {
                    return String.Compare(x.Unit.Symbol, y.Unit.Symbol, StringComparison.Ordinal);
                }
                return indexOfX.CompareTo(indexOfY);
            }
        }

        public UnitParts Inverse()
        {
            var unitAndPowers = Parts.Select(x => UnitAndPower.Create(x.Unit, -1 * x.Power))
                                     .ToList();
            return new UnitParts(unitAndPowers);
        }
    }
}