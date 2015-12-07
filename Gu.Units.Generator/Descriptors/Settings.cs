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
        public Settings()
        {
            Prefixes = new ObservableCollection<Prefix>();
            BaseUnits = new ObservableCollection<BaseUnit>();
            DerivedUnits = new ObservableCollection<DerivedUnit>();

            //Observable.Merge(BaseUnits.ObserveCollectionChangedSlim(true),
            //                 DerivedUnits.ObserveCollectionChangedSlim(true))
            //          .Subscribe(_ =>
            //          {
            //              OverloadFinder.Find(AllUnits);
            //              OnPropertyChanged(nameof(AllUnits));
            //              OnPropertyChanged(nameof(Quantities));
            //          });
        }

        [JsonConstructor]
        public Settings(ObservableCollection<Prefix> prefixes, ObservableCollection<BaseUnit> baseUnits, ObservableCollection<DerivedUnit> derivedUnits)
        {
            Prefixes = prefixes;
            BaseUnits = baseUnits;
            DerivedUnits = derivedUnits;

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

        public IReadOnlyList<BaseUnit> AllUnits => BaseUnits.Concat(DerivedUnits).ToList();

        public IReadOnlyList<Quantity> Quantities => AllUnits.Select(x => x.Quantity).ToList();

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
