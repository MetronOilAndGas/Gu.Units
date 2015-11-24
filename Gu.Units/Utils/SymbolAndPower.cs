namespace Gu.Units
{
    using System;

    internal struct SymbolAndPower : IEquatable<SymbolAndPower>
    {
        public readonly string Symbol;
        public readonly int Power;

        internal SymbolAndPower(string symbol, int power)
        {
            if (String.IsNullOrEmpty(symbol))
            {
                throw new ArgumentNullException("symbol");
            }
            Symbol = symbol;
            Power = power;
        }

        public static bool operator ==(SymbolAndPower left, SymbolAndPower right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SymbolAndPower left, SymbolAndPower right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            string p;
            if (Power == 1)
            {
                p = "";
            }
            else if (Power > 1)
            {
                p = new string(PowerParser.SuperscriptDigits[Power], 1);
            }
            else
            {
                p = new string(new[] { '⁻', PowerParser.SuperscriptDigits[-1 * Power] });
            }
            return $"{this.Symbol}{p}";
        }

        public bool Equals(SymbolAndPower other)
        {
            return String.Equals(Symbol, other.Symbol) && Power == other.Power;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            return obj is SymbolAndPower && Equals((SymbolAndPower)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Symbol.GetHashCode() * 397) ^ Power;
            }
        }

        internal static SymbolAndPower Parse(string text, ref int pos, ref Sign sign)
        {
            if (sign == Sign.None)
            {
                throw new ArgumentException("Sign cannot be none", nameof(sign));
            }

            text.ReadWhiteSpace(ref pos);

            if (pos == text.Length)
            {
                throw new FormatException($"Expected symbol or operator. {text} position: end");
            }

            var op = OperatorParser.Parse(text, ref pos);
            if (op != Operator.None)
            {
                text.ReadWhiteSpace(ref pos);
                if (OperatorParser.Parse(text, ref pos) != Operator.None)
                {
                    var message = $"Cannot have multiple operators in a row. {text} position: {pos}";
                    throw new FormatException(message);
                }

                if (op == Operator.Division)
                {
                    if (sign == Sign.Negative)
                    {
                        throw new FormatException($"String cannot contain / twice. String is: {text}");
                    }

                    sign = Sign.Negative;
                }
            }

            text.ReadWhiteSpace(ref pos);
            return ReadSymbolAndPower(text, ref pos, sign);
        }

        internal static bool TryParse(string text, ref int pos, ref Sign sign, out SymbolAndPower result)
        {
            if (sign == Sign.None)
            {
                result = default(SymbolAndPower);
                return false;
            }

            text.ReadWhiteSpace(ref pos);
            if (pos == text.Length)
            {
                result = default(SymbolAndPower);
                return false;
            }

            var op = OperatorParser.Parse(text, ref pos);
            if (op != Operator.None)
            {
                text.ReadWhiteSpace(ref pos);
                if (OperatorParser.Parse(text, ref pos) != Operator.None)
                {
                    result = default(SymbolAndPower);
                    return false;
                }
                if (op == Operator.Division)
                {
                    if (sign == Sign.Negative)
                    {
                        result = default(SymbolAndPower);
                        return false;
                    }
                    sign = Sign.Negative;
                }
            }

            text.ReadWhiteSpace(ref pos);
            return TryReadSymbolAndPower(text, ref pos, sign, out result);
        }

        internal static bool CanRead(string text, ref int pos)
        {
            text.ReadWhiteSpace(ref pos);
            return pos < text.Length;
        }

        private static SymbolAndPower ReadSymbolAndPower(string s, ref int pos, Sign sign)
        {
            var start = pos;
            while (s.Length > pos && IsSymbol(s[pos]))
            {
                pos++;
            }
            if (pos == start)
            {
                throw new FormatException($"No symbol found at {pos} in {s}");
            }
            var symbol = s.Substring(start, pos - start);
            s.ReadWhiteSpace(ref pos);

            var power = s.Length == pos
                            ? 1
                            : PowerParser.Parse(s, ref pos);
            if (power == 0)
            {
                throw new FormatException($"Power cannot be 0, error at {start + symbol.Length} in {s}");
            }
            if (sign == Sign.Negative && power < 0)
            {
                throw new FormatException($"Power cannot be negative after / error at {start + symbol.Length} in {s}");
            }
            return new SymbolAndPower(symbol, (int)sign * power);
        }

        private static bool TryReadSymbolAndPower(string s, ref int pos, Sign sign, out SymbolAndPower result)
        {
            var start = pos;
            while (s.Length > pos && IsSymbol(s[pos]))
            {
                pos++;
            }
            if (pos == start)
            {
                result = default(SymbolAndPower);
                return false;
            }
            var symbol = s.Substring(start, pos - start);
            s.ReadWhiteSpace(ref pos);

            var power = s.Length == pos
                            ? 1
                            : PowerParser.Parse(s, ref pos);
            if (power == 0)
            {
                result = default(SymbolAndPower);
                return false;
            }
            if (sign == Sign.Negative && power < 0)
            {
                result = default(SymbolAndPower);
                return false;
            }
            result = new SymbolAndPower(symbol, (int)sign * power);
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
