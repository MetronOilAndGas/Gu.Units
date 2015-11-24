namespace Gu.Units
{
    using System;

    internal static class PowerReader
    {
        private const string Superscripts = "⁺⁻⁰¹²³⁴⁵⁶⁷⁸⁹";
        internal const string SuperscriptDigits = "⁰¹²³⁴⁵⁶⁷⁸⁹";

        internal static int Read(string text, ref int pos)
        {
            if (text[pos] == '^')
            {
                int power;
                if (TryReadHatPower(text, ref pos, out power))
                {
                    return power;
                }

                var message = $"Expected to find ^n at position {pos} in {text}.";
                throw new FormatException(message);
            }

            if (Superscripts.IndexOf(text[pos]) == -1)
            {
                return 1;
            }
            {
                int power;
                if (TryReadSuperScriptPower(text, ref pos, out power))
                {
                    return power;
                }

                var message = $"Expected to find superscript power at position {pos} in {text}.";
                throw new FormatException(message);
            }
        }

        internal static bool TryRead(string text, ref int pos, out int result)
        {
            int start = pos;
            if (text[pos] == '^')
            {
                var success = TryReadHatPower(text, ref pos, out result);
                pos = success
                    ? pos
                    : start;
                return success;
            }

            if (Superscripts.IndexOf(text[pos]) == -1)
            {
                result = 1;
                return true;
            }
            {
                var success = TryReadSuperScriptPower(text, ref pos, out result);
                pos = success
                    ? pos
                    : start;
                return success;
            }
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

        private static bool TryReadSuperScriptPower(string s, ref int pos, out int power)
        {
            if (Superscripts.IndexOf(s[pos]) == -1)
            {
                throw new InvalidOperationException();
            }

            var sign = TryReadSuperScriptSign(s, ref pos);
            if (sign == Sign.None)
            {
                sign = Sign.Positive;
            }

            s.ReadWhiteSpace(ref pos);
            if (TryReadSuperScriptInt(s, ref pos, out power))
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