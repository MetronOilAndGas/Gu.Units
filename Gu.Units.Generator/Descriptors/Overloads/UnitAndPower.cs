namespace Gu.Units.Generator
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    public class UnitAndPower : INotifyPropertyChanged
    {
        public static readonly IEqualityComparer<UnitAndPower> Comparer = new UnitNamePowerEqualityComparer();

        public UnitAndPower(string unitName, int power)
        {
            UnitName = unitName;
            Power = power;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string UnitName { get; }

        public BaseUnit Unit => Persister.GetSettings().AllUnits.Single(x => x.Name == UnitName);

        public int Power { get; }

        public static UnitAndPower operator ^(UnitAndPower up, int i)
        {
            return UnitAndPower.Create(up.Unit, up.Power * i);
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
                if (Unit == null)
                {
                    return $"(({Unit.Name})null)^1";
                }
                return this.Unit.Symbol;
            }
            return $"({(this.Unit == null ? "null" : "")}){Unit.Name}^{Power}";
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool Equals(UnitAndPower other)
        {
            return Equals(Unit, other.Unit) && Power == other.Power;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return Equals((UnitAndPower)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Unit?.GetHashCode() ?? 0) * 397) ^ Power;
            }
        }

        private sealed class UnitNamePowerEqualityComparer : IEqualityComparer<UnitAndPower>
        {
            public bool Equals(UnitAndPower x, UnitAndPower y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }
                if (ReferenceEquals(x, null))
                {
                    return false;
                }
                if (ReferenceEquals(y, null))
                {
                    return false;
                }
                if (x.GetType() != y.GetType())
                {
                    return false;
                }
                return string.Equals(x.Unit, y.Unit) && x.Power == y.Power;
            }

            public int GetHashCode(UnitAndPower obj)
            {
                unchecked
                {
                    return ((obj.Unit?.GetHashCode() ?? 0) * 397) ^ obj.Power;
                }
            }
        }
    }
}
