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
                foreach (var unit in Settings.Instance.AllUnits)
                {
                    if (unit.PrefixConversions.Any(pc => pc == this))
                    {
                        return Math.Pow(10, Prefix.Power);
                    }

                    var factorConversion = unit.FactorConversions.SingleOrDefault(x => x.PrefixConversions.Any(pc => pc == this));
                    if (factorConversion != null)
                    {
                        return factorConversion.Factor * Math.Pow(10, Prefix.Power);
                    }
                }

                throw new ArgumentOutOfRangeException();
            }
        }

        public double Offset => 0;

        public bool IsOffset => false;

        public string ToSi => this.GetToSi();

        public string FromSi => this.GetFromSi();

        public bool CanRoundtrip => this.CanRoundtrip();

        public static PrefixConversion Create(Unit unit, Prefix prefix)
        {
            return Create((INameAndSymbol)unit, prefix);
        }

        public static PrefixConversion Create(FactorConversion factorConversion, Prefix prefix)
        {
            return Create((INameAndSymbol)factorConversion, prefix);
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
