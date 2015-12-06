namespace Gu.Units.Generator
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    public class PrefixConversionsVm : INotifyPropertyChanged
    {
        private static readonly IReadOnlyList<string> Illegals = new[] { "cubic", "square", "per" };
        private readonly Settings settings;
        private readonly ObservableCollection<PrefixConversionVm[]> prefixes = new ObservableCollection<PrefixConversionVm[]>();
        private BaseUnit baseUnit;

        public PrefixConversionsVm(Settings settings)
        {
            this.settings = settings;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<PrefixConversionVm[]> Prefixes => this.prefixes;

        public BaseUnit BaseUnit
        {
            get { return this.baseUnit; }
            set
            {
                if (Equals(value, this.baseUnit))
                {
                    return;
                }

                this.baseUnit = value;
                OnPropertyChanged();
            }
        }

        public void SetBaseUnit(BaseUnit value)
        {
            this.BaseUnit = value;
            this.prefixes.Clear();
            if (this.baseUnit != null)
            {
                if (IsValidPrefixUnit(this.baseUnit))
                {
                    this.prefixes.Add(this.settings.Prefixes.Select(x => new PrefixConversionVm(this.baseUnit.PrefixConversions, this.baseUnit, x)).ToArray());
                }

                foreach (var conversion in this.baseUnit.FactorConversions)
                {
                    this.prefixes.Add(this.settings.Prefixes.Select(x => new PrefixConversionVm(conversion.PrefixConversions, conversion, x)).ToArray());
                }
            }
        }

        private bool IsValidPrefixUnit(INameAndSymbol item)
        {
            if (this.settings.Prefixes.Any(p => item.Name.StartsWith(p.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }

            if (Illegals.Any(x => item.Name.IndexOf(x, StringComparison.OrdinalIgnoreCase) >= 0))
            {
                return false;
            }

            var conversion = item as BaseUnit;
            if (conversion != null)
            {
                return true;
            }

            var factorConversion = item as FactorConversion;
            if (factorConversion != null)
            {
                return true;
            }

            return false;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
