namespace Gu.Units.Generator
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    public class PartConversionVm : INotifyPropertyChanged
    {
        private readonly IList<PartConversion> conversions;
        private readonly PartConversion conversion;

        public PartConversionVm(IList<PartConversion> conversions, PartConversion conversion)
        {
            this.conversions = conversions;
            this.conversion = conversion;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public PartConversion Conversion { get; }

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

        public override string ToString()
        {
            return Conversion.Symbol;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool IsMatch(PartConversion x)
        {
            if (Conversion.Factor != x.Factor)
            {
                return false;
            }

            return true;
        }
    }
}
