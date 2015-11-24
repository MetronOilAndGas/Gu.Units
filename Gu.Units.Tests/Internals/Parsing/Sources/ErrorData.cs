namespace Gu.Units.Tests.Internals.Parsing
{
    public static class ErrorData
    {
        public static ErrorData<T> Create<T>(string text,
            int start,
            T expected,
            string expectedMessage)
        {
            return new ErrorData<T>(text, start, expected, start, expectedMessage);
        }

        public static ErrorData<T> Create<T>(string text,
            int start,
            T expected)
        {
            return new ErrorData<T>(text, start, expected, start, null);
        }
    }
}