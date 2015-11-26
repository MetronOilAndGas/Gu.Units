namespace Gu.Units
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    internal class Symbol
    {
        private static readonly IReadOnlyList<SymbolAndPower> Empty = new SymbolAndPower[0];
        private readonly HashSet<SymbolAndPower> _tokens;
        private readonly ConcurrentDictionary<string, MatchAndLength> _matches = new ConcurrentDictionary<string, MatchAndLength>();

        /// <param name="unit">ref to avoid boxing</param>
        internal Symbol(ref IUnit unit)
        {
            Unit = unit;
            _tokens = new HashSet<SymbolAndPower>(TokenizeUnit(unit.Symbol));
            this._matches[unit.Symbol] = new MatchAndLength(true, unit.Symbol.Length);
        }

        internal IEnumerable<SymbolAndPower> Tokens => this._tokens;

        public IUnit Unit { get; }

        public bool TryMatch(string text, ref int pos)
        {
            var temp = pos;
            var matchAndLength = this._matches.GetOrAdd(text, s => TryMatchCore(s, temp));
            if (matchAndLength.IsMatch)
            {
                pos += matchAndLength.Length;
                return true;
            }

            return false;
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


                var op = OperatorReader.TryReadMultiplyOrDivide(text, ref pos);
                if (op != MultiplyOrDivide.None)
                {
                    text.ReadWhiteSpace(ref pos);
                    if (OperatorReader.TryReadMultiplyOrDivide(text, ref pos) != MultiplyOrDivide.None)
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

        internal static bool TryTokenizeUnit(string text, ref int pos, out IReadOnlyList<SymbolAndPower> result)
        {
            int start = pos;
            var sign = Sign.Positive;
            var tokens = new List<SymbolAndPower>();
            text.ReadWhiteSpace(ref pos);
            while (pos < text.Length)
            {
                SymbolAndPower sap;
                if (!SymbolAndPowerParser.TryParse(text, ref pos, out sap))
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

                if (tokens.Any(t => t.Symbol == sap.Symbol))
                {
                    pos = start;
                    result = Empty;
                    return false;
                }

                if (sign == Sign.Negative)
                {
                    sap = new SymbolAndPower(sap.Symbol, -1 * sap.Power);
                }

                tokens.Add(sap);

                var op = OperatorReader.TryReadMultiplyOrDivide(text, ref pos);
                if (op != MultiplyOrDivide.None)
                {
                    text.ReadWhiteSpace(ref pos);
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

                text.ReadWhiteSpace(ref pos);
            }

            result = tokens;
            return true;
        }

        private MatchAndLength TryMatchCore(string text, int pos)
        {
            var start = pos;
            IReadOnlyList<SymbolAndPower> saps;
            if (TryTokenizeUnit(text, ref pos, out saps))
            {
                return new MatchAndLength(_tokens.SetEquals(saps), pos - start);
            }

            return new MatchAndLength(false, 0);
        }

        private struct MatchAndLength
        {
            public readonly bool IsMatch;
            public readonly int Length;

            public MatchAndLength(bool isMatch, int length)
            {
                this.IsMatch = isMatch;
                this.Length = length;
            }
        }
    }
}