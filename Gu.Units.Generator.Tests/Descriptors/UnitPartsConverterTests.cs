﻿namespace Gu.Units.Generator.Tests.Descriptors
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using WpfStuff;

    public class UnitPartsConverterTests
    {
        [TestCaseSource(typeof(UnitPartsConverterSource))]
        public void ConvertFrom(UnitPartsConverterSource.Data data)
        {
            var converter = new UnitPartsConverter();
            Assert.IsTrue(converter.CanConvertFrom(null, typeof(string)));
            var parts = (UnitParts)converter.ConvertFrom(null, null, data.Value);
            Assert.IsTrue(data.Units.SequenceEqual(parts, UnitAndPower.Comparer));
            var convertTo = converter.ConvertTo(null, null, parts, typeof(string));
            Assert.AreEqual(data.Formatted, convertTo);
            Assert.AreEqual(data.Formatted, parts.BaseUnitExpression);
        }
    }

    public class UnitPartsConverterSource : IEnumerable
    {
        public readonly BaseUnit Metres;
        public readonly BaseUnit Kilograms;
        public readonly BaseUnit Seconds;

        private readonly List<Data> _datas;

        public UnitPartsConverterSource()
        {
            var settings = new MockSettings();
            this.Metres = settings.Metres;
            this.Kilograms = settings.Kilograms;
            this.Seconds = settings.Seconds;
            this._datas = new List<Data>
                                        {
                                            new Data("m^2","m²", UnitAndPower.Create(this.Metres, 2)),
                                            new Data("m²","m²", UnitAndPower.Create(this.Metres, 2)),
                                            new Data("m³","m³", UnitAndPower.Create(this.Metres, 3)),
                                            new Data("kg⋅m/s²","kg⋅m⋅s⁻²",UnitAndPower.Create(this.Kilograms, 1),UnitAndPower.Create(this.Metres, 1),UnitAndPower.Create(this.Seconds, -2)),
                                            new Data("kg⋅m⋅s⁻²","kg⋅m⋅s⁻²",UnitAndPower.Create(this.Kilograms, 1),UnitAndPower.Create(this.Metres, 1),UnitAndPower.Create(this.Seconds, -2)),
                                            new Data("kg*m/s²","kg⋅m⋅s⁻²",UnitAndPower.Create(this.Kilograms, 1),UnitAndPower.Create(this.Metres, 1),UnitAndPower.Create(this.Seconds, -2)),
                                            new Data("m/s","m⋅s⁻¹", UnitAndPower.Create(this.Metres,1), UnitAndPower.Create(this.Seconds,-1)),
                                            new Data("m/s","m⋅s⁻¹", UnitAndPower.Create(this.Metres,1), UnitAndPower.Create(this.Seconds,-1)),
                                            new Data("m¹⋅s^-1","m⋅s⁻¹", UnitAndPower.Create(this.Metres,1), UnitAndPower.Create(this.Seconds,-1)),
                                            new Data("m^1⋅s^-1","m⋅s⁻¹", UnitAndPower.Create(this.Metres,1), UnitAndPower.Create(this.Seconds,-1)),
                                            new Data("m⋅s⁻¹","m⋅s⁻¹", UnitAndPower.Create(this.Metres,1), UnitAndPower.Create(this.Seconds,-1)),
                                            new Data("1/s","s⁻¹", UnitAndPower.Create(this.Seconds,-1)),
                                            new Data("s^-1","s⁻¹", UnitAndPower.Create(this.Seconds,-1))
                                            //new Data("J/s",UnitAndPower.Create(Joules, 1),UnitAndPower.Create(Seconds, -1)),
                                        };

        }
        public IEnumerator GetEnumerator()
        {
            return this._datas.GetEnumerator();
        }

        public class Data
        {
            public Data(string value, string formatted, params UnitAndPower[] units)
            {
                this.Value = value;
                this.Formatted = formatted;
                this.Units = units;
            }

            public string Value { get; private set; }

            public string Formatted { get; private set; }

            public IEnumerable<UnitAndPower> Units { get; private set; }

            public override string ToString()
            {
                var units = string.Join(", ", this.Units.Select(x => $"{x.Unit.Symbol}^{x.Power}"));
                return $"{Value} Formatted: {Formatted} Units: {{{units}}}";
            }
        }
    }
}
