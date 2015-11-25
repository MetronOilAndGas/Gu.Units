namespace Gu.Units
{
    using System;
    using System.Globalization;

    internal static class QuantityParser
    {
        internal static TQuantity Parse<TUnit, TQuantity>(string text,
            Func<double, TUnit, TQuantity> creator,
            NumberStyles style,
            IFormatProvider provider)
            where TUnit : IUnit
            where TQuantity : IQuantity
        {
            int end;
            double d;
            if (!DoubleReader.TryRead(text, 0, style, provider, out d, out end))
            {
                throw new FormatException("Could not parse the scalar value from: " + text);
            }

            TUnit unit;
            if (!UnitParser.TryParse(text, ref end, out unit))
            {
                throw new FormatException("Could not parse the unit value from: " + text);
            }

            text.ReadWhiteSpace(ref end);
            if (end != text.Length)
            {
                throw new FormatException("Could not parse the unit value from: " + text);
            }

            return creator(d, unit);
        }

        internal static bool TryParse<TUnit, TQuantity>(string text,
            Func<double, TUnit, TQuantity> creator,
            NumberStyles style,
            IFormatProvider provider,
            out TQuantity value)
        {
            int end;
            double d;
            if (!DoubleReader.TryRead(text, 0, style, provider, out d, out end))
            {
                value = default(TQuantity);
                return false;
            }

            TUnit unit;
            if (!UnitParser.TryParse(text, ref end, out unit))
            {
                value = default(TQuantity);
                return false;
            }

            text.ReadWhiteSpace(ref end);
            if (end != text.Length)
            {
                value = default(TQuantity);
                return false;
            }

            value = creator(d, unit);
            return true;
        }
    }
}