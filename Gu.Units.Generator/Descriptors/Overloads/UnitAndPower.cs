namespace Gu.Units.Generator
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    public class UnitAndPower : INotifyPropertyChanged
    {
        public static readonly IEqualityComparer<UnitAndPower> Comparer = new UnitNamePowerEqualityComparer();


        private UnitAndPower()
        {
        }

        public UnitAndPower(BaseUnit unit)
        {
            Unit = unit;
            Power = 1;
        }

        public UnitAndPower(BaseUnit unit, int power)
        {
            if (power == 0)
            {
                throw new ArgumentException("power == 0", nameof(power));
            }
            Unit = unit;
            Power = power;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public BaseUnit Unit { get; }

        public int Power { get; }

        public static UnitAndPower operator ^(UnitAndPower up, int i)
        {
            return new UnitAndPower(up.Unit, up.Power * i);
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
