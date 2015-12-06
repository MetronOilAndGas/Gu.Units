namespace Gu.Units.Generator
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    public class PrefixConversion : IConversion
    {
        private string name;
        private string symbol;

        public PrefixConversion(string name, string symbol, Prefix prefix)
        {
            Name = name;
            Symbol = symbol;
            Prefix = prefix;
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

        public Prefix Prefix { get; }

        public double Factor => Math.Pow(10, Prefix.Power);

        public double Offset => 0;

        public bool IsOffset => false;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
