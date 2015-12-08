namespace Gu.Units.Generator
{
    using System.ComponentModel;

    public interface INameAndSymbol : INotifyPropertyChanged
    {
        string Name { get; set; }

        string ParameterName { get; }

        string Symbol { get; set; }
    }
}