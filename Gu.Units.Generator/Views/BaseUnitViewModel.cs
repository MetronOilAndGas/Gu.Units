namespace Gu.Units.Generator
{
    using System;
    using System.Linq;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using Reactive;

    public class BaseUnitViewModel : UnitViewModel<BaseUnit>
    {
        private readonly SerialDisposable subscription = new SerialDisposable();

        public BaseUnitViewModel()
            : this(new BaseUnit(UnknownName, UnknownSymbol, UnknownName))
        {
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
            : base(unit)
        {
            UpdateSubscriptions();
        }

        public bool IsOk => IsEverythingOk();

        private void UpdateSubscriptions()
        {
            OnPropertyChanged(nameof(IsOk));
            var observable = Observable.Merge(
                Unit.FactorConversions.ObservePropertyChangedSlim(),
                Unit.FactorConversions.Select(x => x.PrefixConversions.ObservePropertyChangedSlim()).Merge(),
                Unit.OffsetConversions.ObservePropertyChangedSlim(),
                Unit.PrefixConversions.ObservePropertyChangedSlim(),
                Unit.PartConversions.ObservePropertyChangedSlim(),
                Unit.ObservePropertyChangedSlim());
            this.subscription.Disposable = observable.Subscribe(_ => UpdateSubscriptions());
        }
    }
}
