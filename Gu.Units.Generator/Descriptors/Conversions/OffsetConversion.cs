namespace Gu.Units.Generator
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    public class OffsetConversion : IConversion
    {
        private string name;
        private string symbol;
        private double factor;
        private double offset;

        public OffsetConversion(string name, string symbol, double factor, double offset)
        {
            this.name = name;
            this.symbol = symbol;
            this.factor = factor;
            this.offset = offset;
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

        public string ParameterName => Name.ToFirstCharLower();

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

        public double Offset
        {
            get { return this.offset; }
            set
            {
                if (value.Equals(this.offset))
                    return;
                this.offset = value;
                OnPropertyChanged();
            }
        }

        public bool IsOffset => true;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}