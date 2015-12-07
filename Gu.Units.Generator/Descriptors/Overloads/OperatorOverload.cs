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
                throw new ArgumentException($"Cannot create overload for {left.Name} * x^y = {Result.Name}");
            }

            var power = this.FindPower(Left, Right, Result);
            switch (power)
            {
                case Power.None:
                    throw new InvalidOperationException($"Could not create overload for {left} * x^y = {result}");
                case Power.NegOne:
                    Operator = Divide;
                    break;
                case Power.PlusOne:
                    Operator = Multiply;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public Quantity Left { get; private set; }

        public Quantity Right { get; private set; }

        public Quantity Result { get; private set; }

        public string Operator { get; private set; }

        public static bool CanCreate(IReadOnlyList<Unit> units,
            Quantity left,
            Quantity right)
        {
            return FindRight(units, left, right) != null;
        }

        [Obsolete("This is strange")]
        private static Quantity FindRight(IReadOnlyList<Unit> units,
            Quantity left,
            Quantity result)
        {
            var right = left.Unit.Parts / result.Unit.Parts;
            return Find(units, right);
        }

        public override string ToString()
        {
            return $"{Left.Name} {Operator} {Right.Name} = {Result.Name}";
        }

        private static Quantity Find(IReadOnlyList<Unit> units,
            UnitParts parts)
        {
            return units.SingleOrDefault(u => u.Parts == parts)?.Quantity;
        }

        /// <summary>
        /// Solves left * right^x = result
        /// Where x =±1
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private Power FindPower(Quantity left,
            Quantity right,
            Quantity result)
        {
            var leftParts = left.Unit.Parts;
            var rightParts = right.Unit.Parts;
            var resultParts = result.Unit.Parts;

            if (leftParts * rightParts == resultParts)
            {
                return Power.PlusOne;
            }

            if (leftParts / rightParts == resultParts)
            {
                return Power.NegOne;
            }

            return Power.None;
        }
    }
}