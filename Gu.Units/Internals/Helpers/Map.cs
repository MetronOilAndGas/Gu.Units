namespace Gu.Units
{
    using System;
    using System.Collections.Generic;

    internal class Map<T1, T2>
        where T1 : IEquatable<T1>
        where T2 : IEquatable<T2>
    {
        private readonly object gate = new object();
        private Dictionary<T1, T2> dictT1T2 = new Dictionary<T1, T2>();
        private Dictionary<T2, T1> dictT2T1 = new Dictionary<T2, T1>();

        internal T2 this[T1 key] => this.dictT1T2[key];

        internal T1 this[T2 key] => this.dictT2T1[key];

        internal void Add(T1 key, T2 value)
        {
            lock (this.gate)
            {
                var newT1T2 = new Dictionary<T1, T2>(this.dictT1T2) { { key, value } };
                var newT2T1 = new Dictionary<T2, T1>(this.dictT2T1) { { value, key } };
                this.dictT1T2 = newT1T2;
                this.dictT2T1 = newT2T1;
            }
        }

        internal bool TryGetValue(T1 key, out T2 result)
        {
            return this.dictT1T2.TryGetValue(key, out result);
        }

        internal bool TryGetValue(T2 key, out T1 result)
        {
            return this.dictT2T1.TryGetValue(key, out result);
        }
    }
}
