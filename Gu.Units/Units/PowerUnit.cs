namespace Gu.Units.Tests
{
    using System;
    using System.Collections.Generic;
    [Serializable]
    public struct PowerUnit<T> : IPowerUnit<T>, IEquatable<PowerUnit<T>> where T : IUnit
    {
        private readonly T _unit;
        private readonly int _power;
        public PowerUnit(T unit, int power)
        {
            if (power == 0)
            {
                throw new ArgumentException("Power == 0", "power");
            }
            _unit = unit;
            _power = power;
        }
        public T Unit
        {
            get { return _unit; }
        }
        public int Power
        {
            get { return _power; }
        }

        public bool Equals(PowerUnit<T> other)
        {
            return EqualityComparer<T>.Default.Equals(_unit, other._unit) && _power == other._power;
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            return obj is PowerUnit<T> && Equals((PowerUnit<T>) obj);
        }
        public override int GetHashCode()
        {
            unchecked
            {
                return (EqualityComparer<T>.Default.GetHashCode(_unit)*397) ^ _power;
            }
        }
        public static bool operator ==(PowerUnit<T> left, PowerUnit<T> right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(PowerUnit<T> left, PowerUnit<T> right)
        {
            return !left.Equals(right);
        }

        public string Symbol
        {
            get
            {
                throw new NotImplementedException("message");
            }
        }

        public double ToSiUnit(double value)
        {
            throw new NotImplementedException();
        }
    }
}