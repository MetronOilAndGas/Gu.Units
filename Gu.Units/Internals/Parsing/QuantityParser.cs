namespace Gu.Units
{
    using System;
    using System.Globalization;

    internal static class QuantityParser
    {
        internal static TQuantity Parse<TUnit, TQuantity>(string s,
            Func<double, TUnit, TQuantity> creator,
            NumberStyles style,
            IFormatProvider provider)
            where TUnit : IUnit
            where TQuantity : IQuantity
        {
            try
            {
                int end;
                var d = DoubleReader.Read(s, 0, style, provider, out end);
                var us = s.Substring(end, s.Length - end);
                var unit = UnitParser.Parse<TUnit>(us);
                return creator(d, unit);
            }
            catch (Exception e)
            {
                throw new FormatException("Could not parse the unit value from: " + s, e);
            }
        }

        internal static bool TryParse<TUnit, TQuantity>(string text,
            Func<double, TUnit, TQuantity> creator,
            NumberStyles style,
            IFormatProvider provider,
            out TQuantity value)
        {
            if (provider == null)
            {
                provider = NumberFormatInfo.GetInstance(CultureInfo.CurrentCulture);
            }

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

            value = creator(d, unit);
            return true;
        }
    }
}