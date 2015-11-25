﻿namespace Gu.Units
{
    using System;

    internal static class PowerReader
    {
        private const string Superscripts = "⁺⁻⁰¹²³⁴⁵⁶⁷⁸⁹";
        internal const string SuperscriptDigits = "⁰¹²³⁴⁵⁶⁷⁸⁹";

        internal static int Read(string text, ref int pos)
        {
            int result;
            if (TryRead(text, ref pos, out result))
            {
                return result;
            }

            var message = $"Expected to find power at position {pos} in {text}.";
            throw new FormatException(message);
        }

        internal static bool TryRead(string text, ref int pos, out int result)
        {
            if (pos == text.Length)
            {
                result = 1;
                return true;
            }

            int start = pos;
            if (text[pos] == '^')
            {
                var success = TryReadHatPower(text, ref pos, out result);
                pos = success
                    ? pos
                    : start;
                return success;
            }

            if (Superscripts.IndexOf(text[pos]) >= 0)
            {
                var success = TryReadSuperScriptPower(text, ref pos, out result);
                pos = success
                    ? pos
                    : start;
                return success;
            }

            result = 1;
            return true;
        }

        private static bool TryReadHatPower(string text, ref int pos, out int power)
        {
            if (text[pos] != '^')
            {
                // leaving this even if it is a try method. Getting here means something should have been checked before
                throw new InvalidOperationException("Check that there is a hat power to read before calling this");
            }

            pos++;
            text.ReadWhiteSpace(ref pos);
            var ps = OperatorReader.TryReadSign(text, ref pos);
            if (ps == Sign.None)
            {
                ps = Sign.Positive;
            }

            text.ReadWhiteSpace(ref pos);
            if (IntReader.TryReadInt32(text, ref pos, out power))
            {
                power *= (int)ps;
                return true;
            }

            power = 0;
            return false;
        }

        private static bool TryReadSuperScriptPower(string text, ref int pos, out int power)
        {
            if (Superscripts.IndexOf(text[pos]) < 0)
            {
                power = 0;
                return false;
            }

            var sign = TryReadSuperScriptSign(text, ref pos);
            if (sign == Sign.None)
            {
                sign = Sign.Positive;
            }

            text.ReadWhiteSpace(ref pos);
            if (TryReadSuperScriptInt(text, ref pos, out power))
            {
                power *= (int)sign;
                return true;
            }

            power = 0;
            return false;
        }

        private static Sign TryReadSuperScriptSign(string text, ref int pos)
        {
            var sign = Sign.None;
            if (text[pos] == '⁺')
            {
                pos++;
                sign = Sign.Positive;
            }
            else if (text[pos] == '⁻')
            {
                pos++;
                sign = Sign.Negative;
            }

            return sign;
        }

        private static bool TryReadSuperScriptInt(string text, ref int pos, out int result)
        {
            result = SuperscriptDigits.IndexOf(text[pos]);
            if (result < 0)
            {
                result = 0;
                return false;
            }

            pos++;
            int d;
            while (pos < text.Length &&
                  (d = SuperscriptDigits.IndexOf(text[pos])) > -1)
            {
                result *= 10;
                result += d;
                pos++;
            }

            return true;
        }
    }
}