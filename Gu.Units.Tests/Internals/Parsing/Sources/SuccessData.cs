namespace Gu.Units.Tests.Internals.Parsing
{
    public static class SuccessData
    {
        public static SuccessData<T> Create<T>(string text,
            int start,
            T expected,
            int expectedEnd)
        {
            return new SuccessData<T>(text, start, expected, expectedEnd);
        }
    }
}