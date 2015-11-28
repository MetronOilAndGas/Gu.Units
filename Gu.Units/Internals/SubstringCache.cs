namespace Gu.Units
{
    using System;

    internal class SubstringCache<TItem>
    {
        private static readonly CachedItem[] Empty = new CachedItem[0];
        private readonly object gate = new object();
        private CachedItem[] cache = Empty;

        internal bool TryFindSubString(string text, int pos, out CachedItem result)
        {
            if (text == null)
            {
                result = default(CachedItem);
                return false;
            }

            CachedItem? tempResult = null;
            var temp = this.cache;
            int lo = 0;
            int hi = temp.Length - 1;
            int i = 0;
            while (lo <= hi)
            {
                if (tempResult == null)
                {
                    i = GetMedian(lo, hi);
                }
                else
                {
                    i = i + 1; // searching linearly for longest match after finding one
                    if (i == temp.Length)
                    {
                        result = tempResult.Value;
                        return true;
                    }
                }

                var symbolAndUnit = temp[i];
                var c = Compare(symbolAndUnit.Key, text, pos);
                if (c == 0)
                {
                    tempResult = symbolAndUnit;
                    continue;
                }
                else if (tempResult != null)
                {
                    result = tempResult.Value;
                    return true;
                }

                if (c < 0)
                {
                    lo = i + 1;
                }
                else
                {
                    hi = i - 1;
                }
            }

            result = default(CachedItem);
            return false;
        }

        internal void Add(CachedItem item)
        {
            Ensure.NotNull(item.Key, $"{nameof(item)}.{item.Key}");
            lock (this.gate) // this was five times faster than ReaderWriterLockSlim in benchmarks.
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
            return CompareChars(cached, key, pos);
        }

        private static int CompareChars(string cached, string key, int pos)
        {
            for (int i = 0; i < cached.Length; i++)
            {
                var j = i + pos;
                if (key.Length == j)
                {
                    return 1;
                }

                var compare = cached[i] - key[j];
                if (compare != 0)
                {
                    return compare;
                }
            }

            return 0;
        }

        private static int GetMedian(int low, int high)
        {
            Ensure.LessThanOrEqual(low, high, nameof(low));
            return low + ((high - low) >> 1);
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
                return CompareChars(this.Key, other.Key, 0);
            }

            public override string ToString() => this.Key;
        }
    }
}