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
            var factor = conversion.Factor.ToString("R", CultureInfo.InvariantCulture);
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

            var offset = Math.Abs(conversion.Offset).ToString("R", CultureInfo.InvariantCulture);
            if (conversion.Factor != 1)
            {
                return $"{factor}*({conversion.ParameterName} {sign} {offset})";
            }

            return $"{conversion.ParameterName} {sign} {offset}";
        }

        public static string GetFromSi(this IConversion conversion)
        {
            var factor = (1 / conversion.Factor).ToString("R", CultureInfo.InvariantCulture);
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

            var offset = Math.Abs(conversion.Offset).ToString("R", CultureInfo.InvariantCulture);
            if (conversion.Factor != 1)
            {
                return $"{factor}*{conversion.Unit.ParameterName} {sign} {offset}";
            }

            return $"{conversion.Unit.ParameterName} {sign} {offset}";
        }

        public static string GetSymbolConversion(this IConversion conversion)
        {
            var unit = conversion.Unit;
            var convert = ConvertToSi(1, conversion);
            return $"1 {conversion.Symbol.NormalizeSymbol()} = {convert.ToString(CultureInfo.InvariantCulture)} {unit.Symbol.NormalizeSymbol()}";
        }

        public static bool CanRoundtrip(this IConversion conversion)
        {
            foreach (var value in new[] { 0, 100 })
            {
                var si = ConvertToSi(value, conversion);
                var roundtrip = ConvertFromSi(si, conversion);
                if (roundtrip != value)
                {
                    return false;
                }
            }

            return true;
        }

        internal static double ConvertToSi(double value, IConversion conversion)
        {
            return conversion.Factor * (value + conversion.Offset);
        }

        internal static double ConvertFromSi(double value, IConversion conversion)
        {
            return value / conversion.Factor - conversion.Offset;
        }

        public static bool IsSymbolNameValid(this IConversion conversion) => CodeDomProvider.IsValidIdentifier(conversion.Symbol);
    }
}