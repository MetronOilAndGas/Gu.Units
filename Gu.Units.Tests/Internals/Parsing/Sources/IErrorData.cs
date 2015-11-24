namespace Gu.Units.Tests.Internals.Parsing
{
    public interface IErrorData
    {
        string ExpectedMessage { get; }
        string Text { get; }
        int Start { get; }
        int ExpectedEnd { get; }
    }
}