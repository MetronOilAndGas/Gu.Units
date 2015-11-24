namespace Gu.Units.Tests.Internals.Parsing
{
    public class ErrorData<T> : SuccessData<T>, IErrorData
    {
        public ErrorData(string text,
            int start,
            T expected,
            int expectedEnd,
            string expectedMessage) 
            : base(text, start, expected, expectedEnd)
        {
            ExpectedMessage = expectedMessage;
        }

        public string ExpectedMessage { get; }
    }
}