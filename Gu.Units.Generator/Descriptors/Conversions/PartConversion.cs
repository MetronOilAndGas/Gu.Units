﻿namespace Gu.Units.Generator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PartConversion : IConversion
    {
        private Unit unit;

        public PartConversion(string name, string symbol, double factor)
        {
            Name = name;
            Symbol = symbol;
            Factor = factor;
        }

        public string Name { get; }

        public string ParameterName => Name.ToFirstCharLower();

        public string Symbol { get; }

        public double Factor { get; }

        public double Offset => 0;

        public bool IsOffset => false;

        public string ToSi => this.GetToSi();

        public string FromSi => this.GetFromSi();

        public string SymbolConversion => this.GetSymbolConversion();

        public Unit Unit => this.unit ?? (this.unit = this.GetUnit());

        public bool CanRoundtrip => this.CanRoundtrip();

        public static PartConversion Create(Unit unit, PowerPart c1)
        {
            var name = c1.Name;
            var symbol = c1.Symbol;
            var factor = c1.Factor;
            return new PartConversion(name, symbol, factor) { unit = unit }; // hacking unit like this for simpler serialization
        }

        public static PartConversion Create(Unit unit, PowerPart c1, PowerPart c2)
        {
            string name;
            if (c1.Power > 0 && c2.Power > 0)
            {
                name = $"{ c1.Name}{c2.Name}";
            }
            else if (c1.Power > 0 && c2.Power < 0)
            {
                name = $"{ c1.Name}Per{c2.Name.TrimEnd('s')}";
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }

            var symbolAndPowers = c1.AsSymbolAndPowers().Concat(c2.AsSymbolAndPowers());
            var symbol = symbolAndPowers.AsSymbol();
            var factor = c1.Factor * c2.Factor;
            return new PartConversion(name, symbol, factor) { unit = unit }; // hacking unit like this for simpler serialization
        }

        public static PowerPart CreatePart(int power, IConversion conversion)
        {
            return new PowerPart(power, conversion);
        }

        public static PowerPart CreatePart(int power, Unit unit)
        {
            return new PowerPart(power, new IdentityConversion(unit));
        }

        public class PowerPart
        {
            public PowerPart(int power, IConversion conversion)
            {
                Power = power;
                Conversion = conversion;
            }

            public int Power { get; }

            public IConversion Conversion { get; }

            public string Name
            {
                get
                {
                    var power = Math.Abs(Power);
                    switch (power)
                    {
                        case 1:
                            return Conversion.Name;
                        case 2:
                            return $"Square{Conversion.Name}";
                        case 3:
                            return $"Cubic{Conversion.Name}";
                        default:
                            throw new ArgumentOutOfRangeException(nameof(power));
                    }
                }
            }

            public string Symbol
            {
                get
                {
                    IReadOnlyList<SymbolAndPower> symbolAndPowers;
                    if (SymbolAndPowerReader.TryRead(Conversion.Symbol, out symbolAndPowers))
                    {
                        return symbolAndPowers.Select(x => new SymbolAndPower(x.Symbol, x.Power * Power)).AsSymbol();
                    }

                    return "Error";
                }
            }

            public double Factor => Math.Pow(Conversion.Factor, Power);

            internal IReadOnlyList<SymbolAndPower> AsSymbolAndPowers()
            {
                IReadOnlyList<SymbolAndPower> symbolAndPowers;
                if (SymbolAndPowerReader.TryRead(Conversion.Symbol, out symbolAndPowers))
                {
                    return symbolAndPowers.Select(x => new SymbolAndPower(x.Symbol, x.Power * Power)).ToList();
                }

                throw new InvalidOperationException();
            }
        }

        public class IdentityConversion : IConversion
        {

            public IdentityConversion(Unit unit)
            {
                Unit = unit;
            }

            public string Name => Unit.Name;

            public string ParameterName => Name.ToFirstCharLower();

            public string Symbol => Unit.Symbol;

            public double Factor => 1;

            public double Offset => 0;

            public bool IsOffset => false;

            public string ToSi => this.GetToSi();

            public string FromSi => this.GetFromSi();

            public Unit Unit { get; }

            public string SymbolConversion => this.GetSymbolConversion();

            public bool CanRoundtrip => true;
        }
    }
}