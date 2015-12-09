namespace Gu.Units.Generator
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;
    using Newtonsoft.Json;
    using Reactive;

    public class Settings : INotifyPropertyChanged
    {
        public static Settings Instance;

        public static Settings FromResource => JsonConvert.DeserializeObject<Settings>(Properties.Resources.Units);

        private Settings()
        {
        }

        public Settings(ObservableCollection<Prefix> prefixes, ObservableCollection<BaseUnit> baseUnits, ObservableCollection<DerivedUnit> derivedUnits)
        {
            if (Instance != null)
            {
                throw new InvalidOperationException("This is nasty design but there can only be one read from file. Reason is resolving units and prefixes by key.");
            }

            Prefixes = prefixes;
            BaseUnits = baseUnits;
            DerivedUnits = derivedUnits;
            Instance = this;

            Observable.Merge(BaseUnits.ObserveCollectionChangedSlim(true),
                             DerivedUnits.ObserveCollectionChangedSlim(true))
                .Subscribe(_ =>
                {
                    OverloadFinder.Find(AllUnits);
                    OnPropertyChanged(nameof(AllUnits));
                    OnPropertyChanged(nameof(Quantities));
                });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static string Namespace => "Gu.Units";

        public static string ProjectName => "Gu.Units";

        /// <summary>
        /// The extension for the generated files, set to txt if it does not build so you can´inspect the reult
        /// cs when everything works
        /// </summary>
        public static string Extension => "cs";

        public ObservableCollection<Prefix> Prefixes { get; }

        public ObservableCollection<BaseUnit> BaseUnits { get; }

        public ObservableCollection<DerivedUnit> DerivedUnits { get; }

        public IReadOnlyList<Unit> AllUnits => BaseUnits.Concat<Unit>(DerivedUnits).ToList();

        public IReadOnlyList<Quantity> Quantities => AllUnits.Select(x => x.Quantity).ToList();

        public static Settings CreateEmpty()
        {
            return new Settings(new ObservableCollection<Prefix>(), new ObservableCollection<BaseUnit>(), new ObservableCollection<DerivedUnit>());
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
