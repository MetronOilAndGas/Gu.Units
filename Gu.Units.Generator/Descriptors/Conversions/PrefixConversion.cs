namespace Gu.Units.Generator
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    public class PrefixConversion : IConversion
    {
        private string name;
        private string symbol;
        [EditorBrowsable(EditorBrowsableState.Never)]
        private Unit unit;

        public PrefixConversion(string name, string symbol, string prefixName)
        {
            this.name = name;
            this.symbol = symbol;
            PrefixName = prefixName;
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

        public string PrefixName { get; }

        public Prefix Prefix => Settings.Instance.Prefixes.Single(x => x.Name == PrefixName);

        public double Factor
        {
            get
            {
                var factorConversion = Unit.FactorConversions.SingleOrDefault(x => x.PrefixConversions.Any(pc => pc == this));
                if (factorConversion != null)
                {
                    return factorConversion.Factor * Math.Pow(10, Prefix.Power);
                }

                return Math.Pow(10, Prefix.Power);
            }
        }

        public double Offset => 0;

        public bool IsOffset => false;

        public string ToSi => this.GetToSi();

        public string FromSi => this.GetFromSi();

        public string SymbolConversion => this.GetSymbolConversion();

        public Unit Unit => this.unit ?? (this.unit = this.GetUnit());

        public bool CanRoundtrip => this.CanRoundtrip();

        public static PrefixConversion Create(Unit unit, Prefix prefix)
        {
            var prefixConversion = Create((INameAndSymbol)unit, prefix);
            prefixConversion.unit = unit;
            return prefixConversion;
        }

        public static PrefixConversion Create(FactorConversion factorConversion, Prefix prefix)
        {
            var prefixConversion = Create((INameAndSymbol)factorConversion, prefix);
            prefixConversion.unit = factorConversion.GetUnit();
            return prefixConversion;
        }

        private static PrefixConversion Create(INameAndSymbol nas, Prefix prefix)
        {
            return new PrefixConversion(prefix.Name + nas.ParameterName, prefix.Symbol + nas.Symbol, prefix.Name);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
