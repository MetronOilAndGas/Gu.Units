namespace Gu.Units.Generator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class OperatorOverload
    {
        public static string Divide = "/";
        public static string Multiply = "*";
        public OperatorOverload(Quantity left, Quantity result, IReadOnlyList<BaseUnit> units)
        {
            Left = left;
            Result = result;
            Right = FindRight(units, left, Result);
            if (Right == null)
            {
                throw new ArgumentException($"Cannot create overload for {left.Name} * x = {Result.Name}");
            }
            var power = this.FindPower(Left, Right, Result);
            Operator = power > 0 ? Multiply : Divide;
        }

        public Quantity Left { get; private set; }

        public Quantity Right { get; private set; }

        public Quantity Result { get; private set; }

        public string Operator { get; private set; }

        public static bool CanCreate(IReadOnlyList<BaseUnit> units, Quantity left, Quantity right)
        {
            return FindRight(units, left, right) != null;
        }

        public static Quantity FindRight(IReadOnlyList<BaseUnit> units, Quantity left, Quantity result)
        {
            var derivedUnit = result.Unit as DerivedUnit;
            if (derivedUnit != null)
            {
                var right = UnitParts.CreateFrom(result) / UnitParts.CreateFrom(left);
                return Find(units, right.Flattened.ToArray());
            }
            else
            {
                var right = UnitParts.CreateFrom(left) / UnitParts.CreateFrom(result);
                return Find(units, right.Flattened.ToArray());
            }
        }

        public override string ToString()
        {
            return $"{Left.Name} {Operator} {Right.Name} = {Result.Name}";
        }

        private static Quantity Find(IReadOnlyList<BaseUnit> units, params UnitAndPower[] parts)
        {
            BaseUnit unit = null;
            if (parts.Length == 1 && Math.Abs(parts.Single().Power) == 1)
            {
                var part = parts.Single();
                unit = units.SingleOrDefault(u => u.Name == part.Unit.Name);
            }
            else
            {
                var unitAndPowers = parts.OrderBy(x => x.Unit.Name).ToArray();
                unit = units.OfType<DerivedUnit>().SingleOrDefault(u => u.Parts.OrderBy(x => x.Unit.Name).SequenceEqual(unitAndPowers, UnitAndPower.Comparer));
                if (unit == null)
                {
                    unitAndPowers = unitAndPowers.Select(x => UnitAndPower.Create(x.Unit, -1 * x.Power)).ToArray();
                    unit = units.OfType<DerivedUnit>().SingleOrDefault(u => u.Parts.OrderBy(x => x.Unit.Name).SequenceEqual(unitAndPowers, UnitAndPower.Comparer));
                }
            }
            return unit?.Quantity;
        }

        /// <summary>
        /// Solves left * right^x = result
        /// Where x =±1
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private int FindPower(Quantity left, Quantity right, Quantity result)
        {
            var leftParts = UnitParts.CreateFrom(left);
            var rightParts = UnitParts.CreateFrom(right);
            var resultParts = UnitParts.CreateFrom(result);
            if (leftParts * rightParts == resultParts)
            {
                return 1;
            }
            if (leftParts / rightParts == resultParts)
            {
                return -1;
            }
            else
            {
                throw new ArgumentException(
                    $"Cound not find power for {left.Name}*{right.Name}^x == {result.Name}");
            }
            //SiUnit siUnit = left.Unit as SiUnit;
            //if (siUnit != null)
            //{
            //    var unitAndPower = right.Single();
            //    if (Math.Abs(unitAndPower.Power) != 1)
            //    {
            //        throw new ArgumentException();
            //    }
            //    return unitAndPower.Power;
            //}
            //else
            //{
            //    DerivedUnit derivedUnit = (DerivedUnit)left.Unit;
            //    var unitAndPowers = derivedUnit.Parts.OrderBy(x => x.UnitName).ToArray();
            //    var andPowers = right.OrderBy(x => x.UnitName).ToArray();
            //    if (unitAndPowers.Select(x => x.Power).SequenceEqual(andPowers.Select(x => x.Power)))
            //    {
            //        return 1;
            //    }
            //    if (unitAndPowers.Select(x => x.Power).SequenceEqual(andPowers.Select(x => -1 * x.Power)))
            //    {
            //        return -1;
            //    }
            //    throw new ArgumentException("message");
            //}
        }
    }
}