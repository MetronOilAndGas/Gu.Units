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

    public class ConversionsVm : INotifyPropertyChanged
    {
        private readonly SerialDisposable subscription = new SerialDisposable();
        private readonly Settings settings;
        private readonly ReadOnlySerialView<IConversion> allConversions = new ReadOnlySerialView<IConversion>();
        private Unit unit;

        public ConversionsVm(Settings settings)
        {
            this.settings = settings;
            PrefixConversions = new PrefixConversionsVm(settings);
            PartConversions = new PartConversionsVm(settings);
            Unit = settings.AllUnits.FirstOrDefault(x => x.QuantityName == "Speed"); // for designtime
        }

        public event PropertyChangedEventHandler PropertyChanged;

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
                UpdateAllConversionsSubscription();

                PrefixConversions.SetBaseUnit(value);
                PartConversions.SetUnit(value);
                OnPropertyChanged();
            }
        }

        public PrefixConversionsVm PrefixConversions { get; }

        public PartConversionsVm PartConversions { get; }

        public IReadOnlyObservableCollection<IConversion> AllConversions => this.allConversions;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateAllConversionsSubscription()
        {
            if (this.unit == null)
            {
                this.subscription.Disposable.Dispose();
                this.allConversions.SetSource(null);
            }
            else
            {
                var observable = Observable.Merge(this.unit.FactorConversions.ObservePropertyChangedSlim(),
                                                  this.unit.FactorConversions.Select(x => x.PrefixConversions.ObservePropertyChangedSlim()).Merge(),
                                                  this.unit.OffsetConversions.ObservePropertyChangedSlim(),
                                                  this.unit.PrefixConversions.ObservePropertyChangedSlim(),
                                                  this.unit.PartConversions.ObservePropertyChangedSlim());
                this.subscription.Disposable = observable.Subscribe(_ => UpdateAllConversionsSubscription());
                this.allConversions.SetSource(this.unit.AllConversions);
            }
        }
    }
}
