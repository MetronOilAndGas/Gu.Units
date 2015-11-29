namespace Gu.Units
{
    using System;
    using System.Collections.Generic;

    internal static class SymbolAndPowerReader
    {
        private static readonly ReadonlySet<SymbolAndPower> Empty = ReadonlySet<SymbolAndPower>.Empty;

        internal static SymbolAndPower Read(string text, ref int pos)
        {
            if (pos == text.Length)
            {
                throw new FormatException($"Expected symbol or operator. {text} position: end");
            }

            SymbolAndPower result;
            if (TryRead(text, ref pos, out result))
            {
                return result;
            }

            throw new FormatException($"No symbol found at {pos} in {text}");
        }

        internal static bool TryRead(string text, ref int pos, out ReadonlySet<SymbolAndPower> result)
        {
            int start = pos;
            var sign = Sign.Positive;
            var tokens = new SortedSet<SymbolAndPower>(SymbolComparer.Default);
            while (pos < text.Length)
            {
                text.TryReadWhiteSpace(ref pos);

                SymbolAndPower sap;
                if (!TryRead(text, ref pos, out sap))
                {
                    pos = start;
                    result = Empty;
                    return false;
                }

                if (sap.Power < 0 && sign == Sign.Negative)
                {
                    pos = start;
                    result = Empty;
                    return false;
                }

                if (sign == Sign.Negative)
                {
                    sap = new SymbolAndPower(sap.Symbol, -1 * sap.Power);
                }

                if (!tokens.Add(sap))
                {
                    pos = start;
                    result = Empty;
                    return false;
                }

                var op = OperatorReader.TryReadMultiplyOrDivide(text, ref pos);
                if (op != MultiplyOrDivide.None)
                {
                    text.TryReadWhiteSpace(ref pos);
                    if (OperatorReader.TryReadMultiplyOrDivide(text, ref pos) != MultiplyOrDivide.None)
                    {
                        pos = start;
                        result = Empty;
                        return false;
                    }

                    if (op == MultiplyOrDivide.Division)
                    {
                        if (sign == Sign.Negative)
                        {
                            pos = start;
                            result = Empty;
                            return false;
                        }

                        sign = Sign.Negative;
                    }
                }
            }

            result = tokens.AsReadOnly();
            return true;
        }

        internal static bool TryRead(string text, ref int pos, out SymbolAndPower result)
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
            text.TryReadWhiteSpace(ref pos);
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
