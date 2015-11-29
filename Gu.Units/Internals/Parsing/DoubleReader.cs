namespace Gu.Units
{
    using System;
    using System.Globalization;

    internal static class DoubleReader
    {
        private const NumberStyles InvalidNumberStyles = ~(NumberStyles.AllowLeadingWhite |
                                                           NumberStyles.AllowTrailingWhite |
                                                           NumberStyles.AllowLeadingSign |
                                                           NumberStyles.AllowTrailingSign |
                                                           NumberStyles.AllowParentheses |
                                                           NumberStyles.AllowDecimalPoint |
                                                           NumberStyles.AllowThousands |
                                                           NumberStyles.AllowExponent |
                                                           NumberStyles.AllowCurrencySymbol |
                                                           NumberStyles.AllowHexSpecifier);

        private const string LeadingSignNotAllowed = "Leading sign not allowed";
        private const string ExponentNotAllowed = "Exponent not allowed";
        private const string DecimalPointNotAllowed = "Decimal point not allowed";

        internal static double Read(
            string text,
            ref int pos,
            NumberStyles style,
            IFormatProvider provider)
        {
            if (!IsValidFloatingPointStyle(style))
            {
                throw new ArgumentException("Invalid NumberStyles", nameof(style));
            }

            if (style.HasFlag(NumberStyles.AllowHexSpecifier))
            {
                throw new ArgumentException("Hex not supported", nameof(style));
            }

            double result;
            if (TryRead(text, ref pos, style, provider, out result))
            {
                return result;
            }

            var message = $"Expected to find a double starting at index {pos}\r\n" +
                          $"String: {text}\r\n" +
                          $"        {new string(' ', pos)}^";
            throw new FormatException(message);
        }

        internal static bool TryRead(
            string text,
            ref int pos,
            NumberStyles style,
            IFormatProvider provider,
            out double result)
        {
            result = 0;
            var start = pos;
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            if (!IsValidFloatingPointStyle(style))
            {
                return false;
            }

            if ((style & NumberStyles.AllowHexSpecifier) != 0)
            {
                return false;
            }

            if ((style & NumberStyles.AllowLeadingWhite) != 0)
            {
                text.TryReadWhiteSpace(ref pos);
            }

            if (char.IsWhiteSpace(text[pos]))
            {
                return false;
            }

            if (TryReadDigitsOnly(text, ref pos, style, provider, out result))
            {
                return true;
            }

            if (provider == null)
            {
                provider = CultureInfo.CurrentCulture;
            }

            var format = NumberFormatInfo.GetInstance(provider);
            if (TryRead(text, ref pos, format.NaNSymbol))
            {
                result = double.NaN;
                return true;
            }

            if (TryRead(text, ref pos, format.PositiveInfinitySymbol))
            {
                result = double.PositiveInfinity;
                return true;
            }

            if (TryRead(text, ref pos, format.NegativeInfinitySymbol))
            {
                result = double.NegativeInfinity;
                return true;
            }

            pos = start;
            return false;
        }

        // Try parse a double from digits ignoring +-Inf and NaN
        private static bool TryReadDigitsOnly(
            string text,
            ref int pos,
            NumberStyles style,
            IFormatProvider provider,
            out double result)
        {
            var start = pos;
            var format = NumberFormatInfo.GetInstance(provider);
            Sign sign;
            if (TryReadSign(text, ref pos, format, out sign))
            {
                if ((style & NumberStyles.AllowDecimalPoint) == 0)
                {
                    result = 0;
                    pos = start;
                    return false;
                }
            }

            ulong integral = 0;
            if (!TryReadIntegerDigits(text, ref pos, style, format, ref integral))
            {
                pos = start;
                result = 0;
                return false;
            }

            ulong fraction = 0;
            if (TryRead(text, ref pos, format.NumberDecimalSeparator))
            {
                if ((style & NumberStyles.AllowDecimalPoint) == 0)
                {
                    result = 0;
                    pos = start;
                    return false;
                }

                TryReadFractionDigits(text, ref pos, ref fraction);
            }

            if (TryReadExponent(text, ref pos))
            {
                if ((style & NumberStyles.AllowExponent) == 0)
                {
                    result = 0;
                    pos = start;
                    return false;
                }
                Sign exponentSign;
                TryReadSign(text, ref pos, format, out exponentSign);
                if (TryReadExponentDigits(text, ref pos))
                {
                    return TryParseSubString(text, start, ref pos, style, provider, out result);
                }

                // This is a tricky spot we read digits followed by (sign) exponent 
                // then no digits were thrown. I choose to return the double here.
                // Both alternatives will be wrong in some situations.
                // returning false here would make it impossible to parse 1.2eV
                var backStep = exponentSign == Sign.None
                    ? 1
                    : 2;
                pos -= backStep;
                result = sign == Sign.Negative
                    ? -Combine(integral, fraction)
                    : Combine(integral, fraction);

                return true;
            }

            result = sign == Sign.Negative
                ? -Combine(integral, fraction)
                : Combine(integral, fraction);

            return true;
        }

        private static double Combine(ulong integral, ulong fraction)
        {
            if (fraction < 1E1)
                return integral + 1E-1 * fraction;
            if (fraction < 1E2)
                return integral + 1E-2 * fraction;
            if (fraction < 1E3)
                return integral + 1E-3 * fraction;
            if (fraction < 1E4)
                return integral + 1E-4 * fraction;
            if (fraction < 1E5)
                return integral + 1E-5 * fraction;
            if (fraction < 1E6)
                return integral + 1E-6 * fraction;
            if (fraction < 1E7)
                return integral + 1E-7 * fraction;
            if (fraction < 1E8)
                return integral + 1E-8 * fraction;
            if (fraction < 1E9)
                return integral + 1E-9 * fraction;
            if (fraction < 1E10)
                return integral + 1E-10 * fraction;
            if (fraction < 1E11)
                return integral + 1E-11 * fraction;
            if (fraction < 1E12)
                return integral + 1E-12 * fraction;
            if (fraction < 1E13)
                return integral + 1E-13 * fraction;
            if (fraction < 1E14)
                return integral + 1E-14 * fraction;
            if (fraction < 1E15)
                return integral + 1E-15 * fraction;
            if (fraction < 1E16)
                return integral + 1E-16 * fraction;
            if (fraction < 1E17)
                return integral + 1E-17 * fraction;
            throw new ArgumentOutOfRangeException("Fraction must be truncated before calling this");
        }

        private static bool TryParseSubString(
            string text,
            int start,
            ref int end,
            NumberStyles style,
            IFormatProvider provider,
            out double result)
        {
            var s = text.Substring(start, end - start);
            var success = double.TryParse(s, style, provider, out result);
            if (!success)
            {
                end = start;
            }
            return success;
        }

        private static bool TryReadSign(string s,
            ref int pos,
            NumberFormatInfo format,
            out Sign sign)
        {
            if (TryRead(s, ref pos, format.PositiveSign))
            {
                sign = Sign.Positive;
                return true;
            }

            if (TryRead(s, ref pos, format.NegativeSign))
            {
                sign = Sign.Negative;
                return true;
            }

            sign = Sign.None;
            return false;
        }

        private static bool TryReadExponent(
            string s,
            ref int pos)
        {
            if (TryRead(s, ref pos, "E"))
            {
                return true;
            }

            if (TryRead(s, ref pos, "e"))
            {
                return true;
            }

            return false;
        }

        private static bool TryReadIntegerDigits(string text, ref int pos, NumberStyles styles, NumberFormatInfo format, ref ulong result)
        {
            var start = pos;
            bool readThousandSeparator = false;
            while (pos < text.Length)
            {
                var i = IntReader.GetDigitOrMinusOne(text[pos]);
                if (i != -1)
                {
                    result *= 10;
                    result += (ulong)i;
                    pos++;
                    readThousandSeparator = false;
                    continue;
                }

                if ((styles & NumberStyles.AllowThousands) != 0 &&
                    format?.NumberGroupSeparator != null)
                {
                    if (TryRead(text, ref pos, format.NumberGroupSeparator))
                    {
                        readThousandSeparator = true;
                        continue;
                    }
                }

                if (readThousandSeparator)
                {
                    pos = start;
                    return false;
                }

                if (pos == start &&
                    i == -1)
                {
                    if (pos == text.Length)
                    {
                        return false;
                    }

                    if (!TryRead(text, ref pos, format.NumberDecimalSeparator))
                    {
                        return false;
                    }

                    if (IntReader.GetDigitOrMinusOne(text[pos]) == -1)
                    {
                        pos = start;
                        return false;
                    }

                    pos = start;
                }
                break;
            }

            return !readThousandSeparator;
        }

        private static bool TryReadFractionDigits(string text, ref int pos, ref ulong result)
        {
            var start = pos;
            while (pos < text.Length)
            {
                var i = IntReader.GetDigitOrMinusOne(text[pos]);
                if (i == -1)
                {
                    break;
                }

                pos++;
                if (pos - start < 18)
                {
                    result *= 10;
                    result += (uint)i;
                }
            }

            return pos != start;
        }

        private static bool TryReadExponentDigits(string text, ref int pos)
        {
            var start = pos;
            while (pos < text.Length &&
                   IntReader.IsDigit(text[pos]))
            {
                pos++;
            }

            return pos != start;
        }

        private static bool TryRead(string s, ref int pos, string toRead)
        {
            if (pos == s.Length)
            {
                return false;
            }

            int start = pos;
            while (pos < s.Length &&
                   pos - start < toRead.Length)
            {
                if (s[pos] != toRead[pos - start])
                {
                    pos = start;
                    return false;
                }
                pos++;
            }

            return true;
        }

        private static bool IsValidFloatingPointStyle(NumberStyles style)
        {
            // Check for undefined flags
            return (style & InvalidNumberStyles) == 0;
        }
    }
}
