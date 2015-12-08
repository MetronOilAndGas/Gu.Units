namespace Gu.Units.Generator
{
    using System;

    public class PartConversion : IConversion
    {
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

        public bool CanRoundtrip => this.CanRoundtrip();

        public static PartConversion Create(PowerPart c1, PowerPart c2)
        {
            var name = c1.Conversion.Name + c2.Conversion.Name;
            var symbolAndPowers = new[] {c1.AsSymbolAndPower(), c2.AsSymbolAndPower()};
            var symbol = symbolAndPowers.AsSymbol();
            var factor = c1.Factor * c2.Factor;
            return new PartConversion(name, symbol, factor);
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

            public double Factor => Math.Pow(Conversion.Factor, Power);

            internal SymbolAndPower AsSymbolAndPower()
            {
                return new SymbolAndPower(Conversion.Symbol, Power);
            }
        }

        public class IdentityConversion : IConversion
        {
            private readonly Unit unit;

            public IdentityConversion(Unit unit)
            {
                this.unit = unit;
            }

            public string Name => this.unit.Name;

            public string ParameterName => Name.ToFirstCharLower();

            public string Symbol => this.unit.Symbol;

            public double Factor => 1;

            public double Offset => 0;

            public bool IsOffset => false;

            public string ToSi => this.GetToSi();

            public string FromSi => this.GetFromSi();

            public string SymbolConversion => this.GetSymbolConversion();

            public bool CanRoundtrip => true;
        }
    }
}