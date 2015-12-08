namespace Gu.Units.Generator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class UnitPartsExt
    {
        public static string ToUnitString(this IEnumerable<UnitAndPower> unitAndPowers)
        {
            return unitAndPowers.ToList().ToUnitString();
        }

        public static string ToUnitString(this IReadOnlyList<UnitAndPower> unitAndPowers)
        {
            if (!unitAndPowers.Any())
            {
                return "";
            }

            var sb = new StringBuilder();
            UnitAndPower previous = null;
            foreach (var unitAndPower in unitAndPowers.OrderBy(x => x, UnitParts.BaseUnitOrderComparer.Default).ToArray())
            {
                if (previous != null)
                {
                    sb.Append("⋅");
                }

                sb.Append(unitAndPower.Unit == null
                    ? unitAndPower.Unit.Name
                    : unitAndPower.Unit.Symbol);
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
    }
}