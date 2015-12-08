namespace Gu.Units.Generator
{
    using System;
    using System.CodeDom.Compiler;
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
                return Settings.Instance.AllUnits.Single(x => x.Symbol == identityConversion.Symbol);
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
            builder.Append(conversion.GetUnit().Name);
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

            builder.Append(conversion.GetUnit().Name);
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

        public static bool CanRoundtrip(this IConversion conversion)
        {
            throw new NotImplementedException();
        }

        public static bool IsSymbolNameValid(this IConversion conversion) => CodeDomProvider.IsValidIdentifier(conversion.Symbol);
    }
}