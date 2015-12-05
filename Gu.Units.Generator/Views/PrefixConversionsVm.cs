namespace Gu.Units.Generator
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Annotations;

    public class PrefixConversionsVm : INotifyPropertyChanged
    {
        private static readonly IReadOnlyList<string> Illegals = new[] { "cubic", "square", "per" };
        private readonly Settings settings;
        private readonly ObservableCollection<PrefixConversionVm[]> prefixes = new ObservableCollection<PrefixConversionVm[]>();
        private IUnit baseUnit;

        public PrefixConversionsVm(Settings settings)
        {
            this.settings = settings;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<PrefixConversionVm[]> Prefixes => this.prefixes;

        public IUnit BaseUnit
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

        public void SetBaseUnit(IUnit value)
        {
            this.BaseUnit = value;
            this.prefixes.Clear();
            if (this.baseUnit != null)
            {
                var units = new List<IUnit>(this.baseUnit.Conversions.Count + 1) {this.baseUnit};
                units.AddRange(this.baseUnit.AllConversions);
                var conversions = units.Where(IsValidPrefixUnit);
                foreach (var conversion in conversions)
                {
                    this.prefixes.Add(this.settings.Prefixes.Select(x => new PrefixConversionVm(x, conversion)).ToArray());
                }
            }
        }

        private bool IsValidPrefixUnit(IUnit unit)
        {
            if (this.settings.Prefixes.Any(p => unit.ClassName.StartsWith(p.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }

            if (Illegals.Any(x => unit.ClassName.IndexOf(x, StringComparison.OrdinalIgnoreCase) >= 0))
            {
                return false;
            }

            var conversion = unit as Conversion;
            if (conversion != null &&
                conversion.Formula.Offset != 0)
            {
                return false;
            }

            return true;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
