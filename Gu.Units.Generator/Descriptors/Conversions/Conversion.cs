namespace Gu.Units.Generator
{
    using System;
    using System.CodeDom.Compiler;
    using System.Globalization;
    using System.Linq;

    public static class Conversion
    {
        private static readonly CodeDomProvider CodeDomProvider = CodeDomProvider.CreateProvider("C#");

        public static Unit GetUnit(this IConversion conversion)
        {
            var identityConversion = conversion as PartConversion.IdentityConversion;
            if (identityConversion != null)
            {
                var unit = Settings.Instance.AllUnits.Single(x => x.Symbol == identityConversion.Symbol);
                return unit;
            }

            foreach (var unit in Settings.Instance.AllUnits)
            {
                if (unit.AllConversions.Any(pc => pc == conversion))
                {
                    return unit;
                }
            }

            throw new ArgumentOutOfRangeException($"Could not find a unit matching symbol {conversion.Symbol} for conversion {conversion.Name}");
        }

        public static string GetToSi(this IConversion conversion)
        {
            if (conversion.Offset == 0)
            {
                if (conversion.Factor == 1)
                {
                    return conversion.ParameterName;
                }

                var exponent = conversion.Factor.Exponent();
                if (exponent == 0)
                {
                    return $"{conversion.Factor.ToString("G17", CultureInfo.InvariantCulture)}*{conversion.ParameterName}";
                }

                if (exponent < 0)
                {
                    return $"{conversion.ParameterName}/1{new string('0', Math.Abs(exponent))}";
                }

                return $"1{new string('0', exponent)}*{conversion.ParameterName}";
            }

            var sign = Math.Sign(conversion.Offset) == 1
                ? "+"
                : "-";

            var offset = Math.Abs(conversion.Offset).ToString("G17", CultureInfo.InvariantCulture);
            if (conversion.Factor == 1)
            {
                return $"{conversion.ParameterName} {sign} {offset}";
            }

            return $"{conversion.Factor.ToString("G17", CultureInfo.InvariantCulture)}*({conversion.ParameterName} {sign} {offset})";
        }

        public static string GetFromSi(this IConversion conversion)
        {
            var parameter = conversion.Unit.ParameterName;
            if (conversion.Offset == 0)
            {
                if (conversion.Factor == 1)
                {
                    return parameter;
                }

                var exponent = conversion.Factor.Exponent();
                if (exponent == 0)
                {
                    return $"{(1.0 / conversion.Factor).ToString("G17", CultureInfo.InvariantCulture)}*{parameter}";
                }

                if (exponent < 0)
                {
                    return $"1{new string('0', Math.Abs(exponent))}*{parameter}";
                }

                return $"{parameter}/1{new string('0', exponent)}";
            }

            var sign = Math.Sign(conversion.Offset) == -1
                ? "+"
                : "-";

            var offset = Math.Abs(conversion.Offset).ToString("G17", CultureInfo.InvariantCulture);
            if (conversion.Factor == 1)
            {
                return $"{parameter} {sign} {offset}";
            }

            return $"{(1.0 / conversion.Factor).ToString("G17", CultureInfo.InvariantCulture)}*{parameter} {sign} {offset}";
        }

        public static string GetSymbolConversion(this IConversion conversion)
        {
            var unit = conversion.Unit;
            var toSi = conversion.ToSi;
            var convert = ConvertToSi(1, toSi);

            return $"1 {conversion.Symbol.NormalizeSymbol()} = {convert.ToString(CultureInfo.InvariantCulture)} {unit.Symbol.NormalizeSymbol()}";
        }

        public static bool CanRoundtrip(this IConversion conversion)
        {
            var toSi = conversion.ToSi;
            var fromSi = conversion.FromSi;
            foreach (var value in new[] { 0, 100 })
            {
                var si = ConvertToSi(value, toSi);
                var roundtrip = ConvertFromSi(si, fromSi);
                if (roundtrip != value)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsSymbolNameValid(this IConversion conversion) => CodeDomProvider.IsValidIdentifier(conversion.Symbol);

        internal static double ConvertToSi(double value, string toSi)
        {
            var result = ExpressionParser.Evaluate(value, toSi);
            return result;
        }

        internal static double ConvertFromSi(double value, string fromSi)
        {
            var result = ExpressionParser.Evaluate(value, fromSi);
            return result;
        }

        private static int Exponent(this double factor)
        {
            for (int i = -12; i <= 12; i++)
            {
                if (Math.Abs(factor - Math.Pow(10, i)) < float.Epsilon)
                {
                    return i;
                }
            }

            return 0;
        }
    }
}