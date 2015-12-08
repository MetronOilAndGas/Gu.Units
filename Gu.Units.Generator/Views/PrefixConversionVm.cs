namespace Gu.Units.Generator
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    public class PrefixConversionVm : INotifyPropertyChanged
    {
        private readonly IList<PrefixConversion> conversions;
        private readonly IConversion baseConversion;

        private PrefixConversionVm(IList<PrefixConversion> conversions, IConversion baseConversion, PrefixConversion prefixConversion)
        {
            this.conversions = conversions;
            this.baseConversion = baseConversion;
            Conversion = prefixConversion;
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

        public object Formula => $"1 {Conversion.Symbol} = 1E{Conversion.Prefix.Power} {this.baseConversion.Symbol}";

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

        public static PrefixConversionVm Create(Unit unit, Prefix prefix)
        {
            var identityConversion = new PartConversion.IdentityConversion(unit);
            var prefixConversion = PrefixConversion.Create(unit, prefix);
            return new PrefixConversionVm(unit.PrefixConversions, identityConversion, prefixConversion);
        }

        public static PrefixConversionVm Create(FactorConversion factorConversion, Prefix prefix)
        {
            var prefixConversion = PrefixConversion.Create(factorConversion, prefix);
            return new PrefixConversionVm(factorConversion.PrefixConversions, factorConversion, prefixConversion);
        }
    }
}