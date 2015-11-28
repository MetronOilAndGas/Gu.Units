namespace Gu.Units
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    internal class SubstringCache<TItem>
    {
        private static readonly CachedItem[] Empty = new CachedItem[0];
        private readonly object gate = new object();
        private CachedItem[] cache = Empty;

        internal bool TryFind(string text, int pos, out CachedItem result)
        {
            if (text == null)
            {
                result = default(CachedItem);
                return false;
            }

            var temp = this.cache;
            for (int i = 0; i < temp.Length; i++)
            {
                var symbolAndUnit = temp[i];
                if (Compare(symbolAndUnit.Key, text, pos) == 0)
                {
                    result = symbolAndUnit;
                    return true;
                }
            }

            result = default(CachedItem);
            return false;
        }

        internal void Add(CachedItem item)
        {
            Ensure.NotNull(item.Key, $"{nameof(item)}.{item.Key}");
            lock (this.gate)
            {
                for (int i = 0; i < this.cache.Length; i++)
                {
                    var cachedItem = this.cache[i];
                    if (cachedItem.Key == item.Key)
                    {
                        if (Equals(cachedItem.Value, item.Value))
                        {
                            return;
                        }

                        throw new InvalidOperationException("Cannot add same key with different values.\r\n" +
                                                            $"The key is {item.Key} and the values are {{{item.Value}, {cachedItem.Value}}}");
                    }
                }
                var updated = new CachedItem[this.cache.Length + 1];
                Array.Copy(this.cache, 0, updated, 0, this.cache.Length);
                updated[this.cache.Length] = item;
                Array.Sort(updated);
                this.cache = updated;
            }
        }

        private static int Compare(string cached, string key, int pos)
        {
            var compare = (key.Length - pos) - cached.Length;
            if (compare < 0)
            {
                return compare;
            }

            return CompareChars(cached, key, pos);
        }

        private static int CompareChars(string cached, string key, int pos)
        {
            for (int i = 0; i < cached.Length; i++)
            {
                var compare = cached[i] - key[i + pos];
                if (compare != 0)
                {
                    return compare;
                }
            }

            return 0;
        }

        internal struct CachedItem : IComparable<CachedItem>
        {
            internal readonly string Key;
            internal readonly TItem Value;

            public CachedItem(string key, TItem value)
            {
                this.Key = key;
                this.Value = value;
            }

            public int CompareTo(CachedItem other)
            {
                return string.CompareOrdinal(this.Key, other.Key);
            }

            public override string ToString() => this.Key;
        }
    }
}