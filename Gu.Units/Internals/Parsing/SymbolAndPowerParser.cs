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

            return new SymbolAndPower(symbol, power);
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
            int power;
            if (!PowerReader.TryRead(text, ref pos, out power))
            {
                pos = start;
                result = default(SymbolAndPower);
                return false;
            }

            if (power == 0 || Math.Abs(power) > 5) // 5 > is most likely a typo right?
            {
                pos = start;
                result = default(SymbolAndPower);
                return false;
            }

            result = new SymbolAndPower(symbol, power);
            return true;
        }

        private static bool IsSymbol(char c)
        {
            if (c == '°' || c == '‰' || c == '%')
            {
                return true;
            }

            return char.IsLetter(c);
        }
    }
}
