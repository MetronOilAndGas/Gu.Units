﻿namespace Gu.Units.Generator
{
    using System;
    using System.ComponentModel;
    using System.Linq;

    public class UnitAndPower : INotifyPropertyChanged
    {
        public UnitAndPower(string unitName, int power)
        {
            UnitName = unitName;
            Power = power;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string UnitName { get; }

        public BaseUnit Unit => Settings.Instance.AllUnits.Single(x => x.Name == UnitName);

        public int Power { get; }

        public static bool operator ==(UnitAndPower left, UnitAndPower right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UnitAndPower left, UnitAndPower right)
        {
            return !Equals(left, right);
        }

        public static UnitAndPower Create(BaseUnit unit)
        {
            return new UnitAndPower(unit.Name, 1);
        }

        public static UnitAndPower Create(BaseUnit unit, int power)
        {
            if (power == 0)
            {
                throw new ArgumentException("power == 0", nameof(power));
            }

            return new UnitAndPower(unit.Name, power);
        }

        public override string ToString()
        {
            if (Power == 1)
            {
                return this.Unit.Symbol;
            }

            return $"{Unit.Symbol}^{Power}";
        }

        protected bool Equals(UnitAndPower other)
        {
            return string.Equals(UnitName, other.UnitName) && Power == other.Power;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((UnitAndPower) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (UnitName.GetHashCode()*397) ^ Power;
            }
        }
    }
}
