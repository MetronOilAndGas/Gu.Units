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
        private Unit unit;

        public PrefixConversionsVm(Settings settings)
        {
            this.settings = settings;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<PrefixConversionVm[]> Prefixes => this.prefixes;

        public Unit Unit
        {
            get { return this.unit; }
            set
            {
                if (Equals(value, this.unit))
                {
                    return;
                }

                this.unit = value;
                OnPropertyChanged();
            }
        }

        public void SetBaseUnit(Unit value)
        {
            this.Unit = value;
            this.prefixes.Clear();
            if (this.unit != null)
            {
                if (IsValidPrefixUnit(this.unit))
                {
                    this.prefixes.Add(this.settings.Prefixes.Select(x => PrefixConversionVm.Create(this.unit, x)).ToArray());
                }

                foreach (var conversion in this.unit.FactorConversions)
                {
                    this.prefixes.Add(this.settings.Prefixes.Select(x => PrefixConversionVm.Create(conversion, x)).ToArray());
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
