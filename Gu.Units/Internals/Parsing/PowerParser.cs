namespace Gu.Units
{
    using System;

    internal static class PowerParser
    {
        private const string Superscripts = "⁺⁻⁰¹²³⁴⁵⁶⁷⁸⁹";
        internal const string SuperscriptDigits = "⁰¹²³⁴⁵⁶⁷⁸⁹";

        internal static int Parse(string s, ref int pos)
        {
            if (s[pos] == '^')
            {
                return ReadHatPower(s, ref pos);
            }

            if (Superscripts.IndexOf(s[pos]) == -1)
            {
                return 1;
            }

            return ReadSuperScriptPower(s, ref pos);
        }

        private static int ReadHatPower(string s, ref int pos)
        {
            if (s[pos] != '^')
            {
                var message = $"Expected to find ^ at position {pos} in {s}. Was {s[pos]}";
                throw new FormatException(message);
            }

            pos++;
            s.ReadWhiteSpace(ref pos);
            var ps = OperatorParser.ReadSign(s, ref pos);
            if (ps == Sign.None)
            {
                ps = Sign.Positive;
            }

            s.ReadWhiteSpace(ref pos);
            var i = ReadSingleCharInt(s, ref pos);
            return (int)ps * i;
        }

        private static int ReadSuperScriptPower(string s, ref int pos)
        {
            if (Superscripts.IndexOf(s[pos]) == -1)
            {
                throw new InvalidOperationException();
            }

            var ps = ReadSuperScriptSign(s, ref pos);
            if (ps == Sign.None)
            {
                ps = Sign.Positive;
            }

            s.ReadWhiteSpace(ref pos);
            var i = ReadSingleCharSuperScriptInt(s, ref pos);
            return (int)ps * i;
        }

        private static Sign ReadSuperScriptSign(string s, ref int pos)
        {
            var sign = Sign.None;
            if (s[pos] == '⁺')
            {
                pos++;
                sign = Sign.Positive;
            }
            else if (s[pos] == '⁻')
            {
                pos++;
                sign = Sign.Negative;
            }

            return sign;
        }

        private static int ReadSingleCharInt(string s, ref int pos)
        {
            if (!char.IsDigit(s[pos]))
            {
                throw new FormatException($"Expected digit at pos: {pos} in {s} was {s[pos]}");
            }

            int i = (int)char.GetNumericValue(s[pos]);
            pos++;
            if (pos < s.Length && char.IsDigit(s[pos]))
            {
                throw new FormatException($"Did not expect digit at pos: {pos} in {s} was {s[pos]}");
            }

            return i;
        }

        private static int ReadSingleCharSuperScriptInt(string s, ref int pos)
        {
            var indexOf = SuperscriptDigits.IndexOf(s[pos]);
            if (indexOf == -1)
            {
                throw new FormatException($"Expected digit at pos: {pos} in {s} was {s[pos]}");
            }

            int i = indexOf;
            pos++;
            if (pos < s.Length && SuperscriptDigits.IndexOf(s[pos]) != -1)
            {
                throw new FormatException($"Did not expect digit at pos: {pos} in {s} was {s[pos]}");
            }

            return i;
        }
    }
}