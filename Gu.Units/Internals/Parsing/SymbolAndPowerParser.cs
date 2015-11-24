namespace Gu.Units
{
    using System;

    internal static class SymbolAndPowerParser
    {
        internal static SymbolAndPower Parse(string text, ref int pos)
        {
            if (pos == text.Length)
            {
                throw new FormatException($"Expected symbol or operator. {text} position: end");
            }

            var start = pos;
            while (text.Length > pos && IsSymbol(text[pos]))
            {
                pos++;
            }

            if (pos == start)
            {
                throw new FormatException($"No symbol found at {pos} in {text}");
            }

            var symbol = text.Substring(start, pos - start);
            text.ReadWhiteSpace(ref pos);

            var power = text.Length == pos
                            ? 1
                            : PowerReader.Read(text, ref pos);
            if (power == 0)
            {
                throw new FormatException($"Power cannot be 0, error at {start + symbol.Length} in {text}");
            }

            return new SymbolAndPower(symbol,  power);
        }

        internal static bool TryParse(string text, ref int pos, out SymbolAndPower result)
        {
            if (pos == text.Length)
            {
                result = default(SymbolAndPower);
                return false;
            }

            var start = pos;
            while (text.Length > pos && IsSymbol(text[pos]))
            {
                pos++;
            }

            if (pos == start)
            {
                result = default(SymbolAndPower);
                return false;
            }

            var symbol = text.Substring(start, pos - start);
            text.ReadWhiteSpace(ref pos);

            var power = text.Length == pos
                            ? 1
                            : PowerReader.Read(text, ref pos);
            if (power == 0)
            {
                result = default(SymbolAndPower);
                return false;
            }

            result = new SymbolAndPower(symbol, power);
            return true;
        }

        internal static bool CanRead(string text, ref int pos)
        {
            text.ReadWhiteSpace(ref pos);
            return pos < text.Length;
        }

        private static bool IsSymbol(char c)
        {
            if (c == '°' || c == '‰' || c == '%')
            {
                return true;
            }

            return char.IsLetter(c);
        }

        private static bool Read(string s, ref int pos, string toRead)
        {
            int start = pos;
            while (s.Length < pos && pos - start < toRead.Length)
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
    }
}
