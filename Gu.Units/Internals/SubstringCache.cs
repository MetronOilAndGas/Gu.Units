namespace Gu.Units
{
    using System;
    using System.Collections.Generic;

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

            var tempCache = this.cache;
            var index = BinaryFindSubstring(this.cache, text, pos);
            if (index < 0)
            {
                result = default(CachedItem);
                return false;
            }

            result = tempCache[index];
            for (int i = index + 1; i < tempCache.Length; i++)
            {
                // searching linearly for longest match after finding one
                var temp = tempCache[i];
                if (Compare(temp.Key, text, pos) == 0)
                {
                    result = temp;
                }
                else
                {
                    return true;
                }
            }

            return true;
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

        private static int BinaryFindSubstring(CachedItem[] cache, string key, int pos)
        {
            int lo = 0;
            int hi = cache.Length - 1;
            while (lo <= hi)
            {
                var i = (lo + hi) / 2;
                var cached = cache[i];
                var c = Compare(cached.Key, key, pos);
                if (c == 0)
                {
                    return i;
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

            return ~1;
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