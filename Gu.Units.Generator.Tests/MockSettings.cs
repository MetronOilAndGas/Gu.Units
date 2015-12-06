namespace Gu.Units.Generator.Tests
{

    public class MockSettings : Settings
    {
        public readonly BaseUnit Metres;
        public readonly Quantity Length;

        public readonly BaseUnit Kilograms;
        public FactorConversion Grams;
        public readonly Quantity Mass;

        public readonly BaseUnit Kelvins;
        public readonly Quantity Temperature;

        public readonly BaseUnit Seconds;
        public readonly Quantity Time;

        public readonly DerivedUnit MetresPerSecond;
        public readonly Quantity Speed;

        public readonly DerivedUnit SquareMetres;
        public readonly Quantity Area;

        public readonly DerivedUnit CubicMetres;
        public readonly Quantity Volume;

        public readonly BaseUnit Amperes;
        public readonly Quantity Current;

        public readonly DerivedUnit Newtons;
        public readonly Quantity Force;

        public readonly DerivedUnit Joules;
        public readonly Quantity Energy;

        public readonly DerivedUnit Watts;
        public readonly Quantity Power;

        public readonly DerivedUnit Volts;
        public readonly Quantity Voltage;

        public readonly DerivedUnit Coloumbs;
        public readonly Quantity ElectricCharge;

        public readonly DerivedUnit Hertz;
        public readonly Quantity Frequency;
        public Prefix Milli = new Prefix("Milli", "m", -3);
        public Prefix Kilo = new Prefix("Kilo", "k", 3);

        public MockSettings()
        {
            Prefixes.Add(this.Milli);
            Prefixes.Add(this.Kilo);
            Metres = new BaseUnit("Metres", "m", "Length");
            BaseUnits.Add(Metres);
            Length = Metres.Quantity;

            Kelvins = new BaseUnit("Kelvin", "K", "Temperature");
            Temperature = this.Kelvins.Quantity;

            Seconds = new BaseUnit("Seconds", "s", "Time");
            BaseUnits.Add(Seconds);
            Time = Seconds.Quantity;

            Kilograms = new BaseUnit("Kilograms", "kg", "Mass");
            this.Grams = new FactorConversion("Grams", "g", 0.001);
            this.Kilograms.FactorConversions.Add(Grams);
            BaseUnits.Add(Kilograms);
            Mass = Kilograms.Quantity;

            Amperes = new BaseUnit("Amperes", "A", "ElectricalCurrent");
            BaseUnits.Add(Amperes);
            Current = Amperes.Quantity;

            MetresPerSecond = new DerivedUnit(
                "MetresPerSecond",
                "m/s",
                "Speed",
                new UnitAndPower(Metres, 1),
                new UnitAndPower(Seconds, -1));
            DerivedUnits.Add(MetresPerSecond);
            Speed = MetresPerSecond.Quantity;

            Newtons = new DerivedUnit(
                "Newtons",
                "N",
                "Force",
                new UnitAndPower(Kilograms, 1),
                new UnitAndPower(Metres, 1),
                new UnitAndPower(Seconds, -2));
            DerivedUnits.Add(Newtons);
            Force = Newtons.Quantity;

            Joules = new DerivedUnit(
                "Joules",
                "J",
                "Energy",
                new UnitAndPower(Newtons, 1),
                new UnitAndPower(Metres, 1));
            DerivedUnits.Add(Joules);
            Energy = Joules.Quantity;

            Watts = new DerivedUnit(
                "Watts",
                "W",
                "Power",
                new UnitAndPower(Joules, 1),
                new UnitAndPower(Seconds, -1));
            DerivedUnits.Add(Watts);
            Power = Watts.Quantity;

            Volts = new DerivedUnit(
                "Volts",
                "V",
                "Voltage",
                new UnitAndPower(Watts, 1),
                new UnitAndPower(Amperes, -1));
            DerivedUnits.Add(Volts);
            Voltage = Volts.Quantity;

            Coloumbs = new DerivedUnit(
                "Coloumbs",
                "C",
                "ElectricCharge",
                new UnitAndPower(Seconds, 1),
                new UnitAndPower(Amperes, 1));
            DerivedUnits.Add(Coloumbs);
            ElectricCharge = Coloumbs.Quantity;

            SquareMetres = new DerivedUnit("SquareMetres", "m^2", "Area", new UnitAndPower(Metres, 2));
            DerivedUnits.Add(SquareMetres);
            Area = SquareMetres.Quantity;

            CubicMetres = new DerivedUnit("CubicMetres", "m^3", "Volume", new UnitAndPower(Metres, 3));
            DerivedUnits.Add(CubicMetres);
            Volume = CubicMetres.Quantity;

            Hertz = new DerivedUnit("Hertz", "1/s", "Frequency", new UnitAndPower(Seconds, -1));

            DerivedUnits.Add(Hertz);
            Frequency = Hertz.Quantity;
        }
    }
}
