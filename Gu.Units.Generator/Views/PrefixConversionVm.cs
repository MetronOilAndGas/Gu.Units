namespace Gu.Units.Generator
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Reactive;
    using System.Runtime.CompilerServices;
    using Annotations;
    using Reactive;
    using WpfStuff;

    public class PrefixConversionVm : INotifyPropertyChanged
    {
        public PrefixConversionVm(Prefix prefix, IUnit unit)
            : this(new Conversion(unit, prefix))
        {
        }

        public PrefixConversionVm(Conversion conversion)
        {
            Conversion = conversion;
            Conversion.ObservePropertyChanged(x => x.Prefix.Power).Subscribe(_ => Conversion.Update());
            Conversion.ObservePropertyChanged(x => x.Formula.ConversionFactor).Subscribe(_ => Conversion.Update());
            Conversion.ObservePropertyChanged(x => x.Formula.Offset).Subscribe(_ => Conversion.Update());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Conversion Conversion { get; }

        public bool IsUsed
        {
            get
            {
                var conversion = Conversion;
                if (conversion == null)
                {
                    return false;
                }
                conversion.Update();
                return Conversion.Formula.RooUnit.AllConversions.Any(IsMatch);
            }
            set
            {
                if (value.Equals(IsUsed))
                {
                    return;
                }
                if (value)
                {
                    Conversion.BaseUnit.Conversions.Add(Conversion);
                }
                else
                {
                    Conversion.BaseUnit.Conversions.InvokeRemove(IsMatch);
                }
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool IsMatch(Conversion x)
        {
            if (Conversion.Formula.Offset != x.Formula.Offset)
            {
                return false;
            }

            if (Conversion.Formula.RootFactor != x.Formula.RootFactor)
            {
                return false;
            }

            return string.Equals(Conversion.ClassName, x.ClassName, StringComparison.OrdinalIgnoreCase);
        }
    }
}