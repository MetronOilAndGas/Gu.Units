namespace Gu.Units
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class Symbol
    {
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

        internal static IReadOnlyList<SymbolAndPower> TokenizeUnit(string s)
        {
            int pos = 0;
            var sign = Sign.Positive;
            var tokens = new List<SymbolAndPower>();
            while (SymbolAndPower.CanRead(s, ref pos))
            {
                var sap = SymbolAndPower.Parse(s, ref pos, ref sign);
                if (tokens.Any(t => t.Symbol == sap.Symbol))
                {
                    var message =
                        $"Cannot contain the same symbol more than once {s} contains {sap.Symbol} more than one time";
                    throw new FormatException(message);
                }
                tokens.Add(sap);
            }
            return tokens;
        }

        internal static bool TryTokenizeUnit(string s, out IReadOnlyList<SymbolAndPower> tokens)
        {
            int pos = 0;
            var sign = Sign.Positive;
            var temp = new List<SymbolAndPower>();
            while (SymbolAndPower.CanRead(s, ref pos))
            {
                SymbolAndPower sap;
                if (!SymbolAndPower.TryParse(s, ref pos, ref sign, out sap))
                {
                    tokens = null;
                    return false;
                }
                if (temp.Any(t => t.Symbol == sap.Symbol))
                {
                    tokens = null;
                    return false;
                }
                temp.Add(sap);
            }
            tokens = temp;
            return true;
        }
    }
}