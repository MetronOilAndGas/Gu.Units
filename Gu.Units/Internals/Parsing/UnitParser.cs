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
            StringMap<TUnit>.CachedItem cached;
            if (Cache.Value.TryGetUnitForSymbol(text, pos, out cached))
            {
                if (IsEndOfSymbol(text, pos + cached.Key.Length))
                {
                    pos += cached.Key.Length;
                    result = cached.Value;
                    return true;
                }
            }

            IReadOnlyList<SymbolAndPower> sapResult;
            if (SymbolAndPowerReader.TryRead(text, ref pos, out sapResult))
            {
                if (!WhiteSpaceReader.IsRestWhiteSpace(text, pos) ||
                    !sapResult.Any())
                {
                    result = Unit<TUnit>.Default;
                    pos = start;
                    return false;
                }
                var symbolAndPowers = new ReadonlySet<SymbolAndPower>(sapResult);
                if (Cache.Value.SymbolAndPowersUnitMap.TryGetValue(symbolAndPowers, out result))
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

        internal static IReadOnlyList<SymbolAndPower> GetSymbolParts(TUnit unit)
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
            internal readonly Map<ReadonlySet<SymbolAndPower>, TUnit> SymbolAndPowersUnitMap = new Map<ReadonlySet<SymbolAndPower>, TUnit>();
            private readonly StringMap<TUnit> StringUnitMap = new StringMap<TUnit>();

            internal Caches()
            {
                var units = GetUnits();
                foreach (var unit in units)
                {
                    CacheSymbol(unit.Symbol, unit);

                    int pos = 0;
                    IReadOnlyList<SymbolAndPower> result;
                    if (SymbolAndPowerReader.TryRead(unit.Symbol, ref pos, out result))
                    {
                        if (!WhiteSpaceReader.IsRestWhiteSpace(unit.Symbol, pos))
                        {
                            throw new InvalidOperationException($"Failed splitting {((IUnit)unit).Symbol} into {nameof(SymbolAndPower)}");
                        }

                        if (result.Count == 0)
                        {
                            continue;
                        }

                        this.SymbolAndPowersUnitMap.TryAdd(new ReadonlySet<SymbolAndPower>(result), unit);
                    }
                }
            }

            internal void CacheSymbol(string symbol, TUnit unit)
            {
                this.StringUnitMap.Add(symbol, unit);
            }

            internal IReadOnlyList<SymbolAndPower> GetSymbolParts(TUnit unit)
            {
                IReadOnlyList<SymbolAndPower> result;
                if (this.SymbolAndPowersUnitMap.TryGetValue(unit, out result))
                {
                    return result;
                }

                int pos = 0;
                if (TryParse(unit.Symbol, ref pos, out result))
                {
                    this.SymbolAndPowersUnitMap.TryAdd(result, unit);
                    return result;
                }

                throw new NotSupportedException($"Could not get symbol parts for {unit.Symbol}");
            }

            internal bool TryGetUnitForSymbol(string text, int pos, out StringMap<TUnit>.CachedItem item)
            {
                return this.StringUnitMap.TryGetBySubString(text, pos, out item);
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