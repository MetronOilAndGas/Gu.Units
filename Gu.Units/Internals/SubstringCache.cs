namespace Gu.Units
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    internal class SubstringCache<TItem>
    {
        private readonly object gate = new object();
        private List<CachedItem> cache = new List<CachedItem>();

        internal bool TryFind(string text, int pos, out CachedItem result)
        {
            if (text == null)
            {
                result = default(CachedItem);
                return false;
            }

            var temp = this.cache;
            for (int i = 0; i < temp.Count; i++)
            {
                var symbolAndUnit = temp[i];
                if (IsSubstringEqual(symbolAndUnit.Key, text, pos))
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
                for (int i = 0; i < this.cache.Count; i++)
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
                var updated = new List<CachedItem>(this.cache.Count + 1);
                updated.AddRange(this.cache);
                updated.Add(item);
                updated.Sort();
                this.cache = updated;
            }
        }

        private static bool IsSubstringEqual(string cached, string key, int pos)
        {
            if (cached.Length > key.Length - pos)
            {
                return false;
            }

            for (int i = 0; i < cached.Length; i++)
            {
                if (cached[i] != key[i + pos])
                {
                    return false;
                }
            }

            return true;
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
                if (this.Key.Length != other.Key.Length)
                {
                    return other.Key.Length.CompareTo(this.Key.Length);
                }

                return string.CompareOrdinal(this.Key, other.Key);
            }

            public override string ToString() => this.Key;
        }
    }
}