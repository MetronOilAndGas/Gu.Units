namespace Gu.Units.Generator
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using ChangeTracking;
    using JetBrains.Annotations;
    using Reactive;

    public abstract class UnitViewModel<TUnit> : INotifyPropertyChanged
        where TUnit : Unit
    {
        protected const string UnknownName = "Unknown";
        protected const string UnknownSymbol = "??";

        public UnitViewModel(TUnit unit)
        {
            Unit = unit;
            var settings = ChangeTrackerSettings.Default;
            settings.AddImmutableType<UnitParts>();
            settings.AddImmutableType<Quantity>();
            settings.AddImmutableType<OperatorOverload>();
            settings.AddImmutableType<InverseOverload>();
            settings.AddExplicitType<IEnumerable<IConversion>>();
            settings.AddExplicitProperty<IConversion>(x => x.Unit);

            ChangeTracker.Track(unit, settings)
                .ObservePropertyChangedSlim()
                .Subscribe(_ =>
                {
                    OnPropertyChanged(nameof(IsUnknown));
                    OnPropertyChanged(nameof(IsOk));
                });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public TUnit Unit { get; }

        public bool IsUnknown => Unit.Name == UnknownName ||
                                 Unit.QuantityName == UnknownName ||
                                 Unit.Symbol == UnknownSymbol ||
                                 Unit.Parts.Count == 0;

        public bool IsOk => IsEverythingOk();

        public ObservableCollection<string> Errors { get; } = new ObservableCollection<string>();

        protected bool IsEverythingOk()
        {
            Errors.Clear();
            if (!Unit.AllConversions.All(c => c.CanRoundtrip))
            {
                foreach (var conversion in Unit.AllConversions.Where(x => !x.CanRoundtrip))
                {
                    Errors.Add($"{conversion.Name} cannot roundtrip");
                }
                return false;
            }

            if (IsUnknown)
            {
                Errors.Add("IsUnknown");
                return false;
            }

            return true;
        }

        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}