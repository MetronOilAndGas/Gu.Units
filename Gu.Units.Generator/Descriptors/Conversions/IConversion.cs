namespace Gu.Units.Generator
{
    public interface IConversion : INameAndSymbol
    {
        double Factor { get; }

        double Offset { get; }

        bool IsOffset { get; }

        string ToSi { get; }

        string FromSi { get; }

        string SymbolConversion { get; }

        Unit Unit { get; }

        bool CanRoundtrip { get; }
    }
}