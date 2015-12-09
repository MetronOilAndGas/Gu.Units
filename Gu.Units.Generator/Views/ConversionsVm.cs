namespace Gu.Units.Generator
{
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;
    using JetBrains.Annotations;
    using Reactive;
    using Wpf.Reactive;

    public class ConversionsVm : INotifyPropertyChanged
    {
        private readonly SerialDisposable subscription = new SerialDisposable();
        private readonly Settings settings;
        private readonly ReadOnlySerialView<IConversion> allConversions = new ReadOnlySerialView<IConversion>();
        private Unit unit;
        private IConversion selectedConversion;

        public ConversionsVm(Settings settings)
        {
            this.settings = settings;
            PrefixConversions = new PrefixConversionsVm(settings);
            PartConversions = new PartConversionsVm(settings);
            Unit = settings.AllUnits.FirstOrDefault(x => x.QuantityName == "Speed"); // for designtime
            DeleteSelectedCommand = new RelayCommand(DeleteSelected);
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
                SelectedConversion = null;
                this.unit = value;
                UpdateAllConversionsSubscription();

                PrefixConversions.SetBaseUnit(value);
                PartConversions.SetUnit(value);
                OnPropertyChanged();
            }
        }

        public PrefixConversionsVm PrefixConversions { get; }

        public PartConversionsVm PartConversions { get; }

        public IConversion SelectedConversion
        {
            get { return this.selectedConversion; }
            set
            {
                if (Equals(value, this.selectedConversion))
                    return;
                this.selectedConversion = value;
                OnPropertyChanged();
            }
        }

        public IReadOnlyObservableCollection<IConversion> AllConversions => this.allConversions;
        public ICommand DeleteSelectedCommand { get; }

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

        private void DeleteSelected()
        {
            if (this.selectedConversion == null || this.unit == null)
            {
                return;
            }

            TryRemove(this.unit.FactorConversions, this.selectedConversion);
            foreach (var factorConversion in this.unit.FactorConversions)
            {
                TryRemove(factorConversion.PrefixConversions, this.selectedConversion);
            }

            TryRemove(this.unit.OffsetConversions, this.selectedConversion);
            TryRemove(this.unit.PrefixConversions, this.selectedConversion);
            TryRemove(this.unit.PartConversions, this.selectedConversion);
        }

        private static void TryRemove<T>(ObservableCollection<T> collection, IConversion item) 
            where T : IConversion
        {
            ((IList)collection).Remove(item);
        }
    }
}
