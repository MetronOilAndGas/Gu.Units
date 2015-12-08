namespace Gu.Units.Generator
{
    public interface IConversion : INameAndSymbol
    {
        double Factor { get; }

        double Offset { get; }

        bool IsOffset { get; }

        string ToSi { get; }

        string FromSi { get; }

        bool CanRoundtrip { get; }
    }
}