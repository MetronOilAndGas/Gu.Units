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

            if (style.HasFlag(NumberStyles.AllowHexSpecifier))
            {
                return false;
            }

            if (style.HasFlag(NumberStyles.AllowLeadingWhite))
            {
                text.TryReadWhiteSpace(ref pos);
            }

            if (char.IsWhiteSpace(text[pos]))
            {
                return false;
            }

            if (TryParseDigits(text, ref pos, style, provider, out result))
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
        private static bool TryParseDigits(
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
                if (!style.HasFlag(NumberStyles.AllowLeadingSign))
                {
                    result = 0;
                    pos = start;
                    return false;
                }
            }

            TryReadDigits(text, ref pos, style, format);

            if (TryRead(text, ref pos, format.NumberDecimalSeparator))
            {
                if (!style.HasFlag(NumberStyles.AllowDecimalPoint))
                {
                    result = 0;
                    pos = start;
                    return false;
                }

                TryReadDigits(text, ref pos, style, format);
            }

            if (TryReadExponent(text, ref pos))
            {
                if (!style.HasFlag(NumberStyles.AllowExponent))
                {
                    result = 0;
                    pos = start;
                    return false;
                }

                TryReadSign(text, ref pos, format, out sign);
                if (TryReadDigits(text, ref pos, style, format))
                {
                    return TryParseSubString(text, start, ref pos, style, provider, out result);
                }

                // This is a tricky spot we read digits followed by (sign) exponent 
                // then no digits were thrown. I choose to return the double here.
                // Both alternatives will be wrong in some situations.
                // returning false here would make it impossible to parse 1.2eV
                var backStep = sign == Sign.None
                    ? 1
                    : 2;
                pos -= backStep;
                return TryParseSubString(text, start, ref pos, style, provider, out result);
            }

            return TryParseSubString(text, start, ref pos, style, provider, out result);
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

        private static bool TryReadDigits(string text, ref int pos, NumberStyles styles, NumberFormatInfo format)
        {
            var start = pos;
            bool readThousandSeparator = false;
            while (pos < text.Length)
            {
                if (IntReader.IsDigit(text[pos]))
                {
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
                break;
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
