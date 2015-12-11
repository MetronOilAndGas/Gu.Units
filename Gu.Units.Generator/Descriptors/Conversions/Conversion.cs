namespace Gu.Units.Generator
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    [DebuggerDisplay("1*{ClassName} = {Formula.ToSi}")]
    public static class Conversion
    {
        private static readonly CodeDomProvider CodeDomProvider = CodeDomProvider.CreateProvider("C#");
        private static readonly IReadOnlyList<KeyValuePair<double, string>> EFormats = new[]
        {
            new KeyValuePair<double, string>(1E-18, "1E-18"),
            new KeyValuePair<double, string>(1E-17, "1E-17"),
            new KeyValuePair<double, string>(1E-16, "1E-16"),
            new KeyValuePair<double, string>(1E-15, "1E-15"),
            new KeyValuePair<double, string>(1E-14, "1E-14"),
            new KeyValuePair<double, string>(1E-13, "1E-13"),
            new KeyValuePair<double, string>(1E-12, "1E-12"),
            new KeyValuePair<double, string>(1E-11, "1E-11"),
            new KeyValuePair<double, string>(1E-10, "1E-10"),
            new KeyValuePair<double, string>(1E-9, "1E-9"),
            new KeyValuePair<double, string>(1E-8, "1E-8"),
            new KeyValuePair<double, string>(1E-7, "1E-7"),
            new KeyValuePair<double, string>(1E-6, "1E-6"),
            new KeyValuePair<double, string>(1E-5, "1E-5"),
            new KeyValuePair<double, string>(1E-4, "1E-4"),
            new KeyValuePair<double, string>(1E-3, "1E-3"),
            new KeyValuePair<double, string>(1E-2, "1E-2"),
            new KeyValuePair<double, string>(1E-1, "1E-1"),
            new KeyValuePair<double, string>(1E1, "1E1"),
            new KeyValuePair<double, string>(1E2, "1E2"),
            new KeyValuePair<double, string>(1E3, "1E3"),
            new KeyValuePair<double, string>(1E4, "1E4"),
            new KeyValuePair<double, string>(1E5, "1E5"),
            new KeyValuePair<double, string>(1E6, "1E6"),
            new KeyValuePair<double, string>(1E7, "1E7"),
            new KeyValuePair<double, string>(1E8, "1E8"),
            new KeyValuePair<double, string>(1E9, "1E9"),
            new KeyValuePair<double, string>(1E10, "1E10"),
            new KeyValuePair<double, string>(1E11, "1E11"),
            new KeyValuePair<double, string>(1E12, "1E12"),
            new KeyValuePair<double, string>(1E13, "1E13"),
            new KeyValuePair<double, string>(1E14, "1E14"),
            new KeyValuePair<double, string>(1E15, "1E15"),
            new KeyValuePair<double, string>(1E16, "1E16"),
            new KeyValuePair<double, string>(1E17, "1E17"),
            new KeyValuePair<double, string>(1E18, "1E18"),
        };

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

            throw new ArgumentOutOfRangeException();
        }

        public static string GetToSi(this IConversion conversion)
        {
            var factor = conversion.Factor.ToFactorString();
            if (conversion.Offset == 0)
            {
                if (conversion.Factor == 1)
                {
                    return conversion.ParameterName;
                }

                return $"{factor}*{conversion.ParameterName}";
            }

            var sign = Math.Sign(conversion.Offset) == 1
                ? "+"
                : "-";

            var offset = Math.Abs(conversion.Offset).ToString("G17", CultureInfo.InvariantCulture);
            if (conversion.Factor != 1)
            {
                return $"{factor}*({conversion.ParameterName} {sign} {offset})";
            }

            return $"{conversion.ParameterName} {sign} {offset}";
        }

        public static string GetFromSi(this IConversion conversion)
        {
            var factor = (1 / conversion.Factor).ToFactorString();
            if (conversion.Offset == 0)
            {
                if (conversion.Factor == 1)
                {
                    return conversion.Unit.ParameterName;
                }

                return $"{factor}*{conversion.Unit.ParameterName}";
            }

            var sign = Math.Sign(conversion.Offset) == -1
                ? "+"
                : "-";

            var offset = Math.Abs(conversion.Offset).ToString("G17", CultureInfo.InvariantCulture);
            if (conversion.Factor != 1)
            {
                return $"{factor}*{conversion.Unit.ParameterName} {sign} {offset}";
            }

            return $"{conversion.Unit.ParameterName} {sign} {offset}";
        }

        public static string GetSymbolConversion(this IConversion conversion)
        {
            var unit = conversion.Unit;
            var toSi = conversion.ToSi;
            var convert = ConvertToSi(1, toSi);
            return $"1 {conversion.Symbol.NormalizeSymbol()} = {convert.ToFactorString()} {unit.Symbol.NormalizeSymbol()}";
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

        private static string ToFactorString(this double factor)
        {
            var match = EFormats.SingleOrDefault(x => Math.Abs(x.Key - factor) < double.Epsilon);
            if (match.Value != null)
            {
                return match.Value;
            }

            return factor.ToString("G17", CultureInfo.InvariantCulture);
        }
    }
}