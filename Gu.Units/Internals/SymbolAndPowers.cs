namespace Gu.Units
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    internal class SymbolAndPowers : IReadOnlyList<SymbolAndPower>, IEquatable<SymbolAndPowers>
    {
        private static readonly IReadOnlyList<SymbolAndPower> Empty = new SymbolAndPower[0];
        internal readonly IReadOnlyList<SymbolAndPower> Positives;
        internal readonly IReadOnlyList<SymbolAndPower> Negatives;

        public SymbolAndPowers(IReadOnlyList<SymbolAndPower> positives, IReadOnlyList<SymbolAndPower> negatives)
        {
            this.Positives = positives;
            this.Negatives = negatives;
        }

        public int Count => this.Positives.Count + this.Negatives.Count;

        public SymbolAndPower this[int index]
        {
            get
            {
                if (index < this.Positives.Count)
                {
                    return this.Positives[index];
                }

                return this.Negatives[index - this.Positives.Count - 1];
            }
        }

        public static bool operator ==(SymbolAndPowers left, SymbolAndPowers right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SymbolAndPowers left, SymbolAndPowers right)
        {
            return !Equals(left, right);
        }

        public IEnumerator<SymbolAndPower> GetEnumerator()
        {
            return this.Positives.Concat(this.Negatives).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool Equals(SymbolAndPowers other)
        {
            // compare sorted
            throw new System.NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((SymbolAndPowers)obj);
        }

        public override int GetHashCode()
        {
            // compare sorted
            throw new System.NotImplementedException();
            if (Count == 0)
            {
                return 0;
            }
            unchecked
            {
                int hash = 17;

                foreach (var item in this)
                {
                    hash = (hash * 397) ^ item.GetHashCode();
                }

                return hash;
            }
        }
    }
}