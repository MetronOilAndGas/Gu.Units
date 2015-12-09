namespace Gu.Units.Generator
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;
    using Reactive;

    public class BaseUnitViewModel : INotifyPropertyChanged
    {
        private const string UnknownName = "Unknown";
        private const string UnknownSymbol = "??";
        private readonly SerialDisposable subscription = new SerialDisposable();

        public BaseUnitViewModel()
        {
            Unit = new BaseUnit(UnknownName, UnknownSymbol, UnknownName);
            Unit.ObservePropertyChangedSlim().Subscribe(_ =>
            {
                if (Settings.Instance.BaseUnits.Contains(Unit))
                {
                    return;
                }

                if (!IsUnknown)
                {
                    Settings.Instance.BaseUnits.Add(Unit);
                }
            });
        }

        public BaseUnitViewModel(BaseUnit unit)
        {
            Unit = unit;
        }

        public bool IsUnknown => Unit.Name == UnknownName ||
                                  Unit.QuantityName == UnknownName ||
                                  Unit.Symbol == UnknownSymbol;

        public bool IsOk => IsEverythingOk();

        public BaseUnit Unit { get; }

        private bool IsEverythingOk()
        {
            if (!Unit.AllConversions.All(c => c.CanRoundtrip))
            {
                return false;
            }

            if (IsUnknown)
            {
                return false;
            }
            return true;
        }

        private void UpdateSubscriptions()
        {
            OnPropertyChanged(nameof(IsOk));
            var observable = Observable.Merge(
                Unit.FactorConversions.ObservePropertyChangedSlim(),
                Unit.FactorConversions.Select(x => x.PrefixConversions.ObservePropertyChangedSlim()).Merge(),
                Unit.OffsetConversions.ObservePropertyChangedSlim(),
                Unit.PrefixConversions.ObservePropertyChangedSlim(),
                Unit.PartConversions.ObservePropertyChangedSlim());
            this.subscription.Disposable = observable.Subscribe(_ => UpdateSubscriptions());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
