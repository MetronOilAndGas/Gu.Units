namespace Gu.Units.Tests.Internals.Helpers
{
    using NUnit.Framework;

    public class MapTests
    {
        [Test]
        public void AddThenGetWithIndexer()
        {
            var map = new Map<int, string>();
            map.Add(1, "1");
            map.Add(2, "2");
            Assert.AreEqual("1", map[1]);
            Assert.AreEqual(1, map["1"]);

            string actualString;
            Assert.True(map.TryGetValue(1, out actualString));
        }

        [Test]
        public void TryGetSuccess()
        {
            var map = new Map<int, string>();
            map.Add(1, "1");
            map.Add(2, "2");

            string actualString;
            Assert.True(map.TryGetValue(1, out actualString));
            Assert.AreEqual("1", actualString);

            int actualInt;
            Assert.True(map.TryGetValue("1", out actualInt));
            Assert.AreEqual(1, actualInt);
        }
    }
}
