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
            TUnit result;
            if (TryParse(text, out result))
            {
                return result;
            }

            var message = $"Could not parse: '{text}' to {typeof(TUnit).Name}";
            throw new FormatException(message);
        }

        internal static bool TryParse<TUnit>(string text, out TUnit value)
        {
            var temp = 0;
            return TryParse(text, ref temp, out value);
        }

        internal static bool TryParse<TUnit>(string text, ref int pos, out TUnit value)
        {
            var type = typeof(TUnit);
            var symbols = SymbolCache.GetOrAdd(type, CreateSymbolsForType);
            foreach (var symbol in symbols)
            {
                if (symbol.TryMatch(text, ref pos))
                {
                    value = (TUnit) symbol.Unit;
                    return true;
                }
            }

            value = default(TUnit);
            return false;
        }

        private static IReadOnlyList<Symbol> CreateSymbolsForType(Type type)
        {
            var symbols = type.GetUnitsForType()
                .Select(x => new Symbol(ref x))
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