namespace Gu.Units
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class Symbol
    {
        private static readonly IReadOnlyList<SymbolAndPower> Empty = new SymbolAndPower[0];

        public Symbol(IUnit unit)
        {
            Unit = unit;
            Tokens = new HashSet<SymbolAndPower>(TokenizeUnit(unit.Symbol));
        }

        internal HashSet<SymbolAndPower> Tokens { get; }

        public IUnit Unit { get; }

        public bool IsMatch(string text)
        {
            var saps = TokenizeUnit(text);
            return Tokens.SetEquals(saps);
        }

        public override string ToString()
        {
            return $"Unit: {this.Unit}, Pattern: {string.Join(", ", this.Tokens)}";
        }

        internal static IReadOnlyList<SymbolAndPower> TokenizeUnit(string text)
        {
            int pos = 0;
            var sign = Sign.Positive;
            var tokens = new List<SymbolAndPower>();
            text.ReadWhiteSpace(ref pos);
            while (pos < text.Length)
            {
                var sap = SymbolAndPowerParser.Parse(text, ref pos);
                if (sap.Power < 0 && sign == Sign.Negative)
                {
                    throw new FormatException($"Power cannot be negative after / error at {pos} in {text}");
                }

                if (tokens.Any(t => t.Symbol == sap.Symbol))
                {
                    var message = $"Cannot contain the same symbol more than once {text} contains {sap.Symbol} more than one time";
                    throw new FormatException(message);
                }

                if (sign == Sign.Negative)
                {
                    sap = new SymbolAndPower(sap.Symbol, -1 * sap.Power);
                }

                tokens.Add(sap);


                var op = OperatorParser.TryReadMultiplyOrDivide(text, ref pos);
                if (op != MultiplyOrDivide.None)
                {
                    text.ReadWhiteSpace(ref pos);
                    if (OperatorParser.TryReadMultiplyOrDivide(text, ref pos) != MultiplyOrDivide.None)
                    {
                        var message = $"Cannot have multiple operators in a row. {text} position: {pos}";
                        throw new FormatException(message);
                    }

                    if (op == MultiplyOrDivide.Division)
                    {
                        if (sign == Sign.Negative)
                        {
                            throw new FormatException($"String cannot contain / twice. String is: {text}");
                        }

                        sign = Sign.Negative;
                    }
                }

                text.ReadWhiteSpace(ref pos);
            }

            return tokens;
        }

        internal static bool TryTokenizeUnit(string text, out IReadOnlyList<SymbolAndPower> result)
        {
            int pos = 0;
            var sign = Sign.Positive;
            var tokens = new List<SymbolAndPower>();
            text.ReadWhiteSpace(ref pos);
            while (pos < text.Length)
            {
                var sap = SymbolAndPowerParser.Parse(text, ref pos);
                if (sap.Power < 0 && sign == Sign.Negative)
                {
                    result = Empty;
                    return false;
                }

                if (tokens.Any(t => t.Symbol == sap.Symbol))
                {
                    result = Empty;
                    return false;
                }

                if (sign == Sign.Negative)
                {
                    sap = new SymbolAndPower(sap.Symbol, -1 * sap.Power);
                }

                tokens.Add(sap);


                var op = OperatorParser.TryReadMultiplyOrDivide(text, ref pos);
                if (op != MultiplyOrDivide.None)
                {
                    text.ReadWhiteSpace(ref pos);
                    if (OperatorParser.TryReadMultiplyOrDivide(text, ref pos) != MultiplyOrDivide.None)
                    {
                        result = Empty;
                        return false;
                    }

                    if (op == MultiplyOrDivide.Division)
                    {
                        if (sign == Sign.Negative)
                        {
                            result = Empty;
                            return false;
                        }

                        sign = Sign.Negative;
                    }
                }

                text.ReadWhiteSpace(ref pos);
            }

            result = tokens;
            return true;
        }
    }
}