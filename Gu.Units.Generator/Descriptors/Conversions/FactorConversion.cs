namespace Gu.Units.Generator
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    public class FactorConversion : IConversion
    {
        private string name;
        private string symbol;
        private double factor;

        public FactorConversion(string name, string symbol, double factor)
        {
            this.name = name;
            this.symbol = symbol;
            this.factor = factor;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name
        {
            get { return this.name; }
            set
            {
                if (value == this.name)
                {
                    return;
                }

                this.name = value;
                OnPropertyChanged();
            }
        }

        public string Symbol
        {
            get { return this.symbol; }
            set
            {
                if (value == this.symbol)
                {
                    return;
                }

                this.symbol = value;
                OnPropertyChanged();
            }
        }

        public double Factor
        {
            get { return this.factor; }
            set
            {
                if (value.Equals(this.factor))
                    return;
                this.factor = value;
                OnPropertyChanged();
            }
        }

        public double Offset => 0;

        public bool IsOffset => false;

        public ObservableCollection<PrefixConversion> PrefixConversions { get; } = new ObservableCollection<PrefixConversion>();

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}