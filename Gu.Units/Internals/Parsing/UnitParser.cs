namespace Gu.Units
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    internal static class UnitParser
    {
        private static readonly ConcurrentDictionary<Type, IReadOnlyList<Symbol>> SymbolCache = new ConcurrentDictionary<Type, IReadOnlyList<Symbol>>();

        internal static TUnit Parse<TUnit>(string text)
            where TUnit : IUnit
        {
            var type = typeof(TUnit);
            var symbols = SymbolCache.GetOrAdd(type, CreateSymbolsForType);
            var matches = symbols.Where(x => x.IsMatch(text)).ToArray();
            if (matches.Length == 0)
            {
                var message = $"Could not parse: '{text}' to {typeof (TUnit).Name}";
                throw new FormatException(message);
            }

            if (matches.Length > 1)
            {
                var patterns = string.Join(
                    Environment.NewLine,
                    matches.Select(x => $"Unit: {x.Unit.Symbol} with pattern: {x.Tokens}"));
                var message = string.Format(
                    "Could not parse: '{0}' to {1}{2}The following matches:{2}{3}",
                    text,
                    typeof(TUnit).Name,
                    Environment.NewLine,
                    patterns);
                throw new FormatException(message);
            }
            return (TUnit)matches[0].Unit;
        }

        internal static bool TryParse<TUnit>(string text, out TUnit value)
        {
            var type = typeof(TUnit);
            var symbols = SymbolCache.GetOrAdd(type, CreateSymbolsForType);
            var matches = symbols.Where(x => x.TryMatch(text)).ToArray();
            if (matches.Length != 1)
            {
                value = default(TUnit);
                return false;
            }

            value = (TUnit)matches[0].Unit;
            return true;
        }

        private static IReadOnlyList<Symbol> CreateSymbolsForType(Type type)
        {
            var symbols = type.GetUnitsForType()
                .Select(x => new Symbol(x))
                .ToArray();
            return symbols;
        }

        private static IEnumerable<IUnit> GetUnitsForType(this Type t)
        {
            var units = t.GetFields(BindingFlags.GetField | BindingFlags.Public | BindingFlags.Static)
                .Where(f => typeof(IUnit).IsAssignableFrom(f.FieldType))
                .Select(f => (IUnit)f.GetValue(null))
                .Distinct()
                .ToArray();
            return units;
        }
    }
}