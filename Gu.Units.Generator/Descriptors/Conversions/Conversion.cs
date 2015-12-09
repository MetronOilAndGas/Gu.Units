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
            var builder = new StringBuilder();
            if (conversion.Factor != 1)
            {
                builder.Append(conversion.Factor.ToString(CultureInfo.InvariantCulture) + "*");
            }
            builder.Append(conversion.ParameterName);
            if (conversion.Offset != 0)
            {
                if (conversion.Offset > 0)
                {
                    builder.AppendFormat(" + {0}", conversion.Offset.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    builder.AppendFormat(" - {0}", (-conversion.Offset).ToString(CultureInfo.InvariantCulture));
                }
            }
            return builder.ToString();
        }

        public static string GetFromSi(this IConversion conversion)
        {
            var builder = new StringBuilder();

            builder.Append(conversion.Unit.ParameterName);
            if (conversion.Factor != 1)
            {
                builder.Append("/" + conversion.Factor.ToString(CultureInfo.InvariantCulture));
            }
            if (conversion.Offset != 0)
            {
                if (conversion.Offset < 0)
                {
                    builder.AppendFormat(" + {0}", (-1 * conversion.Offset).ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    builder.AppendFormat(" - {0}", conversion.Offset.ToString(CultureInfo.InvariantCulture));
                }
            }
            return builder.ToString();
        }

        public static string GetSymbolConversion(this IConversion conversion)
        {
            var unit = conversion.Unit;
            var convert = ConvertToSi(1, conversion);
            return $"1 {conversion.Symbol} = {convert.ToString(CultureInfo.InvariantCulture)} {unit.Symbol}";
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
            return value * conversion.Factor + conversion.Offset;
        }

        internal static double ConvertFromSi(double value, IConversion conversion)
        {
            return (value - conversion.Offset) / conversion.Factor;
        }

        public static bool IsSymbolNameValid(this IConversion conversion) => CodeDomProvider.IsValidIdentifier(conversion.Symbol);
    }
}