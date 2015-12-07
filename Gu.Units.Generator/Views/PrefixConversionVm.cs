namespace Gu.Units.Generator
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    public class PrefixConversionVm : INotifyPropertyChanged
    {
        private readonly IList<PrefixConversion> conversions;
        private readonly INameAndSymbol nameAndSymbol;

        public PrefixConversionVm(IList<PrefixConversion> conversions, INameAndSymbol nameAndSymbol, Prefix prefix)
        {
            this.conversions = conversions;
            this.nameAndSymbol = nameAndSymbol;
            Conversion = PrefixConversion.Create(prefix.Name + nameAndSymbol.Name.ToFirstCharLower(), prefix.Symbol + nameAndSymbol.Symbol, prefix);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public PrefixConversion Conversion { get; }

        public bool IsUsed
        {
            get
            {
                return conversions.Any(IsMatch);
            }
            set
            {
                if (value.Equals(IsUsed))
                {
                    return;
                }
                if (value)
                {
                    conversions.Add(Conversion);
                }
                else
                {
                    var match = this.conversions.FirstOrDefault(IsMatch);
                    if (match != null)
                    {
                        conversions.Remove(match);
                    }
                }
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool IsMatch(PrefixConversion x)
        {
            if (Conversion.Prefix.Power != x.Prefix.Power)
            {
                return false;
            }

            return true;
        }
    }
}