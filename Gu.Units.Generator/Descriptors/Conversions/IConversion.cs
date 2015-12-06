namespace Gu.Units.Generator
{
    public interface IConversion : INameAndSymbol
    {
        double Factor { get; }

        double Offset { get; }

        bool IsOffset { get; }
    }
}