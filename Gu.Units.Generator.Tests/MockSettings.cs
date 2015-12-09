namespace Gu.Units.Generator.Tests
{
    using System.Collections.ObjectModel;

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

        public readonly DerivedUnit MetresPerSecondSquared;
        public readonly Quantity Acceleration;

        public readonly DerivedUnit SquareMetres;
        public readonly Quantity Area;

        public readonly DerivedUnit CubicMetres;
        public readonly Quantity Volume;

        public readonly DerivedUnit KilogramsPerCubicMetre;
        public readonly Quantity Density;

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


        private MockSettings()
            : base(new ObservableCollection<Prefix>(), new ObservableCollection<BaseUnit>(), new ObservableCollection<DerivedUnit>())
        {
            Prefixes.Add(this.Milli);
            Prefixes.Add(this.Kilo);
            Metres = new BaseUnit("Metres", "m", "Length");
            BaseUnits.Add(Metres);
            Length = Metres.Quantity;

            Kelvins = new BaseUnit("Kelvin", "K", "Temperature");
            BaseUnits.Add(this.Kelvins);
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
              new[]{ UnitAndPower.Create(Metres, 1),
               UnitAndPower.Create(Seconds, -1)});
            DerivedUnits.Add(MetresPerSecond);
            Speed = MetresPerSecond.Quantity;

            MetresPerSecondSquared = new DerivedUnit(
                "MetresPerSecondSquared",
                "m/s^2",
                "Acceleration",
                new[]
                {
                    UnitAndPower.Create(Metres, 1),
                    UnitAndPower.Create(Seconds, -2)
                });
            DerivedUnits.Add(MetresPerSecondSquared);
            Acceleration = MetresPerSecondSquared.Quantity;

            Newtons = new DerivedUnit(
                "Newtons",
                "N",
                "Force",
              new[]{   UnitAndPower.Create(Kilograms, 1),
               UnitAndPower.Create(Metres, 1),
               UnitAndPower.Create(Seconds, -2)});
            DerivedUnits.Add(Newtons);
            Force = Newtons.Quantity;

            Joules = new DerivedUnit(
                "Joules",
                "J",
                "Energy",
              new[]{   UnitAndPower.Create(Newtons, 1),
               UnitAndPower.Create(Metres, 1)});
            DerivedUnits.Add(Joules);
            Energy = Joules.Quantity;

            Watts = new DerivedUnit(
                "Watts",
                "W",
                "Power",
             new[]{    UnitAndPower.Create(Joules, 1),
               UnitAndPower.Create(Seconds, -1)});
            DerivedUnits.Add(Watts);
            Power = Watts.Quantity;

            Volts = new DerivedUnit(
                "Volts",
                "V",
                "Voltage",
             new[]{    UnitAndPower.Create(Watts, 1),
               UnitAndPower.Create(Amperes, -1)});
            DerivedUnits.Add(Volts);
            Voltage = Volts.Quantity;

            Coloumbs = new DerivedUnit(
                "Coloumbs",
                "C",
                "ElectricCharge",
              new[]{   UnitAndPower.Create(Seconds, 1),
               UnitAndPower.Create(Amperes, 1)});
            DerivedUnits.Add(Coloumbs);
            ElectricCharge = Coloumbs.Quantity;

            SquareMetres = new DerivedUnit("SquareMetres", "m^2", "Area", new[] { UnitAndPower.Create(Metres, 2) });
            DerivedUnits.Add(SquareMetres);
            Area = SquareMetres.Quantity;

            CubicMetres = new DerivedUnit("CubicMetres", "m^3", "Volume", new[] { UnitAndPower.Create(Metres, 3) });
            DerivedUnits.Add(CubicMetres);
            Volume = CubicMetres.Quantity;

            KilogramsPerCubicMetre = new DerivedUnit("KilogramsPerCubicMetre", "kg/m^3", "Density", new[] {UnitAndPower.Create(this.Kilograms,1), UnitAndPower.Create(Metres, -3) });
            DerivedUnits.Add(KilogramsPerCubicMetre);
            Density = CubicMetres.Quantity;

            Hertz = new DerivedUnit("Hertz", "1/s", "Frequency", new[] { UnitAndPower.Create(Seconds, -1) });

            DerivedUnits.Add(Hertz);
            Frequency = Hertz.Quantity;
        }

        public static MockSettings Create()
        {
            Instance = null;
            return new MockSettings();
        }
    }
}
