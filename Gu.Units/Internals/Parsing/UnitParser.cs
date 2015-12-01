namespace Gu.Units
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    internal static class UnitParser<TUnit> where TUnit : struct, IUnit, IEquatable<TUnit>
    {
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
            SubstringCache<TUnit>.CachedItem cached;
            if (Cache.Value.TryGetForSymbol(text, pos, out cached))
            {
                if (IsEndOfSymbol(text, pos + cached.Key.Length))
                {
                    pos += cached.Key.Length;
                    result = cached.Value;
                    return true;
                }
            }

            ReadonlySet<SymbolAndPower> sapResult;
            if (SymbolAndPowerReader.TryRead(text, ref pos, out sapResult))
            {
                if (!WhiteSpaceReader.IsRestWhiteSpace(text, pos) ||
                    !sapResult.Any())
                {
                    result = Unit<TUnit>.Default;
                    pos = start;
                    return false;
                }

                if (Cache.Value.SymbolAndPowers.TryGetValue(sapResult, out result))
                {
                    var symbol = text.Substring(start, pos - start);
                    Cache.Value.CacheSymbol(symbol, result);
                    return true;
                }
            }

            pos = start;
            result = Unit<TUnit>.Default;
            return false;
        }

        internal static ReadonlySet<SymbolAndPower> GetSymbolParts(TUnit unit)
        {
            return Cache.Value.GetSymbolParts(unit);
        } 

        private static bool IsEndOfSymbol(string text, int pos)
        {
            if (pos == text.Length)
            {
                return true;
            }

            return text[pos] == '}' ||
                   char.IsWhiteSpace(text[pos]);
        }

        private class Caches
        {
            internal readonly Map<ReadonlySet<SymbolAndPower>, TUnit> SymbolAndPowers = new Map<ReadonlySet<SymbolAndPower>, TUnit>();
            private readonly SubstringCache<TUnit> subStrings = new SubstringCache<TUnit>();

            internal Caches()
            {
                var units = GetUnits();
                foreach (var unit in units)
                {
                    CacheSymbol(unit.Symbol, unit);

                    int pos = 0;
                    ReadonlySet<SymbolAndPower> result;
                    if (SymbolAndPowerReader.TryRead(unit.Symbol, ref pos, out result))
                    {
                        if (!WhiteSpaceReader.IsRestWhiteSpace(unit.Symbol, pos))
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
            }

            internal void CacheSymbol(string symbol, TUnit unit)
            {
                this.subStrings.Add(symbol, unit);
            }

            internal ReadonlySet<SymbolAndPower> GetSymbolParts(TUnit unit)
            {
                return this.SymbolAndPowers[unit];
            }

            internal bool TryGetForSymbol(string text, int pos, out SubstringCache<TUnit>.CachedItem item)
            {
                return this.subStrings.TryGetBySubString(text, pos, out item);
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