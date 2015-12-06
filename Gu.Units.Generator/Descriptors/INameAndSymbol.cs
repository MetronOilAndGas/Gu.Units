namespace Gu.Units.Generator
{
    using System.ComponentModel;

    public interface INameAndSymbol : INotifyPropertyChanged
    {
        string Name { get; set; }

        string Symbol { get; set; }
    }
}