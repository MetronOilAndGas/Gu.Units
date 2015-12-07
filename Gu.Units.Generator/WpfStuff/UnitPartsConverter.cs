namespace Gu.Units.Generator.WpfStuff
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class UnitPartsConverter : TypeConverter
    {
        private static readonly string[] Superscripts = { "¹", "²", "³", "⁴" };

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var text = value as string;
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            var settings = Settings.Instance;
            IReadOnlyList<SymbolAndPower> result;
            int pos = 0;
            var indexOf = text.IndexOf("1/");
            if (indexOf >= 0)
            {
                pos = indexOf + 2;
            }
            if (SymbolAndPowerReader.TryRead(text, ref pos, out result))
            {
                if (WhiteSpaceReader.IsRestWhiteSpace(text, pos))
                {
                    var unitAndPowers = result.Select(sap => UnitAndPower.Create(settings.AllUnits.Single(x => x.Symbol == sap.Symbol), sap.Power))
                                          .ToList();
                    var unitParts = new UnitParts(unitAndPowers);
                    if (indexOf < 0)
                    {
                        return unitParts;
                    }

                    return unitParts.Inverse();
                }
            }

            return text;
            var matches = Parse(text);
            var parts = new List<UnitAndPower>();
            int sign = 1;
            bool expectsSymbol = true;
            foreach (Match match in matches)
            {
                if (expectsSymbol)
                {
                    var symbol = match.Groups["Symbol"].Value;
                    if (symbol == "1")
                    {
                        expectsSymbol = false;
                        continue;
                    }
                    var unit = settings.AllUnits.Single(x => x.Symbol == symbol);
                    int p = ParsePower(match.Groups["Power"].Value);
                    parts.Add(UnitAndPower.Create(unit, sign * p));
                    expectsSymbol = false;
                }
                else
                {
                    var op = match.Groups["Op"].Value;
                    if (op == "/")
                    {
                        if (sign < 0)
                        {
                            throw new FormatException("/ can't appear twice found at position:" + match.Index);
                        }
                        else
                        {
                            sign = -1;
                        }
                    }
                    expectsSymbol = true;
                }
            }
            return new UnitParts(parts);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            return ((UnitParts)value)?.Expression;
        }

        private static IEnumerable<Match> Parse(string s)
        {
            var settings = Persister.GetSettingsFromFile();
            var symbols = settings.AllUnits.Where(x => !string.IsNullOrEmpty(x.Symbol))
                                                    .Select(x => Regex.Escape(x.Symbol))
                                                    .ToArray();
            var symbolsPattern = string.Join("|", new[] { "1" }.Concat(symbols));
            var superscriptsPattern = string.Join("|", Superscripts);
            var pattern = $@"(?<Unit>
                                (?<Symbol>({symbolsPattern
                                                }))
                                (?<Power>
                                    ((?:\^)[\+\-]?\d+)
                                    |
                                    (⁻?({superscriptsPattern
                                                }))
                                )?
                                |
                                (?<Op>[⋅\*\/])?
                            )";
            var matches = Regex.Matches(s, pattern, RegexOptions.IgnorePatternWhitespace)
                               .OfType<Match>()
                               .ToArray();
            return matches;
        }

        private static int ParsePower(string power)
        {
            if (power == "")
            {
                return 1;
            }
            if (power[0] == '⁻')
            {
                var indexOf = Array.IndexOf(Superscripts, power.Substring(1));
                if (indexOf < 0)
                {
                    throw new FormatException();
                }
                return -1 * (indexOf + 1);
            }
            int p = Array.IndexOf(Superscripts, power) + 1;
            if (p > 0)
            {
                return p;
            }
            if (power[0] != '^')
            {
                throw new FormatException();
            }
            p = int.Parse(power.TrimStart('^'));
            return p;
        }
    }
}