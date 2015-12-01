namespace Gu.Units.Tests.Internals.Helpers
{
    using System;
    using System.Reflection;
    using NUnit.Framework;

    public class SubstringCacheTests
    {
        [Test]
        public void AddSameTwice()
        {
            var cache = new SubstringCache<string>();
            cache.Add("abc", "d");
            cache.Add("abc", "d");
            var cachedItems = GetInnerCache(cache);
            Assert.AreEqual(1, cachedItems.Length);
        }

        [Test]
        public void AddDifferentWithSameKeyTwice()
        {
            var cache = new SubstringCache<string>();
            cache.Add("abc", "d");
            Assert.Throws<InvalidOperationException>(() => cache.Add("abc", "e"));
        }

        [Test]
        public void Sorts()
        {
            var cache = new SubstringCache<string>();
            var item1 = cache.Add("abcde", "1");
            var item2 = cache.Add("abc", "2");
            var item3 = cache.Add("abcd", "3");
            var item4 = cache.Add("bar", "4");
            var actual = GetInnerCache(cache);
            var expected = new[] { item2, item3, item1, item4 };
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestCase("abc", 0, "abc", "2")]
        [TestCase("abcdef", 0, "abcde", "1")]
        [TestCase(" abc", 1, "abc", "2")]
        [TestCase(" abcd", 1, "abcd", "3")]
        public void TryFindSubStringSuccess(string key, int pos, string expectedKey, string expectedValue)
        {
            var cache = new SubstringCache<string>();
            cache.Add("abcde", "1");
            cache.Add("abc", "2");
            cache.Add("abcd", "3");
            cache.Add("bar", "4");
            SubstringCache<string>.CachedItem actual;
            var success = cache.TryGetBySubString(key, pos, out actual);
            Assert.AreEqual(true, success);
            Assert.AreEqual(expectedKey, actual.Key);
            Assert.AreEqual(expectedValue, actual.Value);
        }

        [TestCase("abc",  "2")]
        [TestCase("abcde", "1")]
        [TestCase("abcd",  "3")]
        public void TryGetSuccess(string key, string expectedValue)
        {
            var cache = new SubstringCache<string>();
            cache.Add("abcde", "1");
            cache.Add("abc", "2");
            cache.Add("abcd", "3");
            cache.Add("bar", "4");
            SubstringCache<string>.CachedItem actual;
            var success = cache.TryGet(key, out actual);
            Assert.AreEqual(true, success);
            Assert.AreEqual(key, actual.Key);
            Assert.AreEqual(expectedValue, actual.Value);
        }

        [TestCase(null, 0)]
        [TestCase("", 0)]
        [TestCase(" ggg", 1)]
        [TestCase("ggg", 1)]
        [TestCase("g", 1)]
        public void AddThenGetFail(string key, int pos)
        {
            var cache = new SubstringCache<string>();
            cache.Add("abc", "d");
            cache.Add("foo", "e");
            cache.Add("bar", "f");
            SubstringCache<string>.CachedItem actual;
            var success = cache.TryGetBySubString(key, pos, out actual);
            Assert.AreEqual(false, success);
            Assert.AreEqual(null, actual.Key);
            Assert.AreEqual(null, actual.Value);
        }

        private static SubstringCache<string>.CachedItem[] GetInnerCache(SubstringCache<string> cache)
        {
            var fieldInfo = typeof(SubstringCache<string>).GetField("cache", BindingFlags.NonPublic | BindingFlags.Instance);
            return (SubstringCache<string>.CachedItem[])fieldInfo.GetValue(cache);
        }
    }
}
