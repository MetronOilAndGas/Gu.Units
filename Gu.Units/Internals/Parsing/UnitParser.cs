namespace Gu.Units
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;

    internal static class UnitParser<TUnit> where TUnit : struct, IUnit
    {
        private static readonly TUnit Default = (TUnit)default(TUnit).SiUnit;
        private static readonly Lazy<Caches> Cache = new Lazy<Caches>(() => new Caches());

        internal static TUnit Parse(string text)
        {
            TUnit result;
            if (TryParse(text, out result))
            {
                return result;
            }

            var message = $"Could not parse: '{text}' to {typeof(TUnit).Name}";
            throw new FormatException(message);
        }

        internal static TUnit Parse(string text, ref int pos)
        {
            TUnit result;
            if (TryParse(text, ref pos, out result))
            {
                return result;
            }

            var message = $"Could not parse: '{text}' to {typeof(TUnit).Name}";
            throw new FormatException(message);
        }

        internal static bool TryParse(string text, out TUnit value)
        {
            var temp = 0;
            return TryParse(text, ref temp, out value);
        }

        internal static bool TryParse(string text, ref int pos, out TUnit result)
        {
            var start = pos;
            SymbolAndUnit resultSymbolAndUnit;
            if (Cache.Value.TryGetForSymbol(text, pos, out resultSymbolAndUnit))
            {
                pos += resultSymbolAndUnit.Symbol.Length;
                result = resultSymbolAndUnit.Unit;
                return true;
            }

            ReadonlySet<SymbolAndPower> sapResult;
            if (SymbolAndPowerParser.TryRead(text, ref pos, out sapResult))
            {
                if (!text.IsRestWhiteSpace(pos) ||
                    !sapResult.Any())
                {
                    result = Default;
                    pos = start;
                    return false;
                }

                if (Cache.Value.SymbolAndPowers.TryGetValue(sapResult, out result))
                {
                    var symbol = text.Substring(start, pos - start);
                    var symbolAndUnit = new SymbolAndUnit(symbol, result);
                    Cache.Value.Add(symbolAndUnit);
                    return true;
                }
            }

            pos = start;
            result = Default;
            return false;
        }

        private struct SymbolAndUnit : IComparable<SymbolAndUnit>
        {
            internal readonly string Symbol;
            internal readonly TUnit Unit;

            public SymbolAndUnit(TUnit unit)
            {
                this.Unit = unit;
                this.Symbol = unit.Symbol;
            }

            public SymbolAndUnit(string symbol, TUnit unit)
            {
                this.Symbol = symbol;
                this.Unit = unit;
            }

            public int CompareTo(SymbolAndUnit other)
            {
                if (this.Symbol.Length != other.Symbol.Length)
                {
                    return other.Symbol.Length.CompareTo(this.Symbol.Length); 
                }

                return string.CompareOrdinal(this.Symbol, other.Symbol);
            }

            public override string ToString() => this.Symbol;
        }

        private class Caches
        {
            private readonly List<SymbolAndUnit> symbolAndUnits = new List<SymbolAndUnit>();
            private readonly ReaderWriterLockSlim gate = new ReaderWriterLockSlim();
            internal readonly Dictionary<ReadonlySet<SymbolAndPower>, TUnit> SymbolAndPowers = new Dictionary<ReadonlySet<SymbolAndPower>, TUnit>();

            internal Caches()
            {
                var units = GetUnits();
                foreach (var unit in units)
                {
                    var stringAndIndex = new SymbolAndUnit(unit);
                    this.symbolAndUnits.Add(stringAndIndex);

                    int pos = 0;
                    ReadonlySet<SymbolAndPower> result;
                    if (SymbolAndPowerParser.TryRead(((IUnit)unit).Symbol, ref pos, out result))
                    {
                        if (!((IUnit)unit).Symbol.IsRestWhiteSpace(pos))
                        {
                            throw new InvalidOperationException($"Failed splitting {((IUnit)unit).Symbol} into {nameof(SymbolAndPower)}");
                        }

                        if (result.IsEmpty)
                        {
                            continue;
                        }

                        this.SymbolAndPowers.Add(result, unit);
                    }
                }

                this.symbolAndUnits.Sort();
            }

            internal bool TryGetForSymbol(string text, int pos, out SymbolAndUnit result)
            {
                this.gate.EnterReadLock();
                for (int i = 0; i < this.symbolAndUnits.Count; i++)
                {
                    var symbolAndUnit = this.symbolAndUnits[i];
                    if (IsSubstringEqual(symbolAndUnit.Symbol, text, pos))
                    {
                        this.gate.ExitReadLock();
                        result = symbolAndUnit;
                        return true;
                    }
                }

                this.gate.ExitReadLock();
                result = default(SymbolAndUnit);
                return false;
            }

            internal void Add(SymbolAndUnit symbolAndUnit)
            {
                this.gate.EnterWriteLock();
                this.symbolAndUnits.Add(symbolAndUnit);
                this.symbolAndUnits.Sort();
                this.gate.ExitWriteLock();
            }

            private static bool IsSubstringEqual(string cached, string key, int pos)
            {
                if (cached.Length > key.Length - pos)
                {
                    return false;
                }

                for (int i = 0; i < cached.Length; i++)
                {
                    if (cached[i] != key[i + pos])
                    {
                        return false;
                    }
                }

                return true;
            }

            private static IReadOnlyList<TUnit> GetUnits()
            {
                var units = typeof(TUnit).GetFields(BindingFlags.GetField | BindingFlags.Public | BindingFlags.Static)
                    .Where(f => f.FieldType == typeof(TUnit))
                    .Select(f => (TUnit)f.GetValue(null))
                    .Distinct()
                    .ToList();
                return units;
            }
        }
    }
}