namespace Gu.Units.Generator.Tests.ToJson
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Linq;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using Serialization;
    using WpfStuff;

    public class ParseAndDump
    {
        private static List<Prefix> prefixes;

        public static IReadOnlyList<Prefix> Prefixes
        {
            get
            {
                if (prefixes != null)
                {
                    return prefixes;
                }
                var xDocument = XDocument.Parse(Properties.Resources.GeneratorSettings);
                var prefixesElements = xDocument.Root.Element("Prefixes");
                prefixes = new List<Prefix>();
                foreach (var prefixElement in prefixesElements.Elements())
                {
                    var name = prefixElement.Element("Name").Value;
                    var symbol = prefixElement.Element("Symbol").Value;
                    var power = int.Parse(prefixElement.Element("Power").Value);
                    prefixes.Add(new Prefix(name, symbol, power));
                }
                return prefixes;
            }
        }

        public static IReadOnlyList<BaseUnit> BaseUnits
        {
            get
            {
                var xDocument = XDocument.Parse(Properties.Resources.GeneratorSettings);
                var baseUnitsElement = xDocument.Root.Element("SiUnits");
                var units = new List<BaseUnit>();
                foreach (var unitElement in baseUnitsElement.Elements())
                {
                    var name = unitElement.Element("ClassName").Value;
                    var symbol = unitElement.Element("Symbol").Value;
                    var quantityName = unitElement.Element("QuantityName").Value;
                    var baseUnit = new BaseUnit(name, symbol, quantityName);
                    ReadConversions(baseUnit, unitElement.Element("Conversions"));
                    units.Add(baseUnit);
                }
                return units;
            }
        }

        public static IReadOnlyList<DerivedUnit> DerivedUnits
        {
            get
            {
                var xDocument = XDocument.Parse(Properties.Resources.GeneratorSettings);
                var derivedUnitsElement = xDocument.Root.Element("DerivedUnits");
                var units = new List<DerivedUnit>();
                foreach (var unitElement in derivedUnitsElement.Elements())
                {
                    var name = unitElement.Element("ClassName").Value;
                    var symbol = unitElement.Element("Symbol").Value;
                    var quantityName = unitElement.Element("QuantityName").Value;
                    units.Add(new DerivedUnit { Name = name, Symbol = symbol, QuantityName = quantityName });
                }

                return units;
            }
        }

        [Test]
        public void DumpPrefixes()
        {
            var settings = new Settings();
            settings.Prefixes.InvokeAddRange(Prefixes);
            var json = JsonConvert.SerializeObject(settings, CreateSettings());
            Console.Write(json);
        }

        [Test]
        public void DumpSiUnits()
        {
            var settings = new Settings();
            settings.SiUnits.InvokeAddRange(BaseUnits);
            var json = JsonConvert.SerializeObject(settings, CreateSettings());
            Console.Write(json);
        }

        [Test]
        public void DumpDerivedUnits()
        {
            var baseUnits = BaseUnits;
            var derivedUnits = DerivedUnits;
            var allUnits = baseUnits.Concat(derivedUnits).ToList();
            var settings = new Settings();
            settings.DerivedUnits.InvokeAddRange(derivedUnits);
            //foreach (var derivedUnit in derivedUnits)
            //{
            //    derivedUnit.Parts = new UnitParts(derivedUnit,);
            //}
            var json = JsonConvert.SerializeObject(settings, CreateSettings());
            Console.Write(json);
        }

        private static UnitAndPower[] ReadParts(XElement partsElement, IReadOnlyList<BaseUnit> allUnits)
        {
            var unitAndPowers = new List<UnitAndPower>();
            foreach (var e in partsElement.Elements("UnitAndPower"))
            {
                var name = e.Element("UnitName").Value;
                var power = int.Parse(e.Element("Power").Value);
                unitAndPowers.Add(new UnitAndPower(allUnits.Single(x => x.Name == name), power));
            }
            return unitAndPowers.ToArray();
        }

        private static void ReadConversions(BaseUnit unit, XElement conversionsElement)
        {
            foreach (var conversionElement in conversionsElement.Elements("Conversion"))
            {
                var name = conversionElement.Element("ClassName").Value;
                var symbol = conversionElement.Element("Symbol").Value;
                var formulaElement = conversionElement.Element("Formula");
                var factor = double.Parse(formulaElement.Element("ConversionFactor").Value, CultureInfo.InvariantCulture);
                var offset = double.Parse(formulaElement.Element("Offset").Value, CultureInfo.InvariantCulture);
                var prefix = Prefixes.SingleOrDefault(x => name.StartsWith(x.Name));

                if (prefix != null)
                {
                    if (factor != Math.Pow(10, prefix.Power))
                    {
                        var match = unit.FactorConversions.SingleOrDefault(x => name == prefix.Name + x.Name.ToFirstCharLower());
                        if(match != null)
                        {
                            if (factor == match.Factor*Math.Pow(10, prefix.Power))
                            {
                                match.PrefixConversions.Add(new PrefixConversion(name, symbol,prefix));
                                continue;
                            }
                        }
                        throw new InvalidOperationException();
                    }

                    if (offset != 0)
                    {
                        throw new InvalidOperationException();
                    }
                    if (name == prefix.Name + unit.Name.ToFirstCharLower())
                    {
                        unit.PrefixConversions.Add(new PrefixConversion(name, symbol, prefix));
                        continue;
                    }
                    throw new InvalidOperationException();
                }
                if (factor != 0 &&
                    offset == 0)
                {
                    unit.FactorConversions.Add(new FactorConversion(name, symbol, factor));
                    continue;
                }

                if (offset != 0)
                {
                    unit.OffsetConversions.Add(new OffsetConversion(name, symbol, factor, offset));
                    continue;
                }
                throw new NotImplementedException();
            }
        }

        public JsonSerializerSettings CreateSettings()
        {
            return new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = ExcludeCalculatedResolver.Default
            };
        }
    }
}
