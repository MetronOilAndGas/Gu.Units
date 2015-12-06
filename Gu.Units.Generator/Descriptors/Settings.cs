namespace Gu.Units.Generator
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class Settings
    {
        public Settings()
        {
        }

        public static string Namespace => "Gu.Units";

        public static string ProjectName => "Gu.Units";

        /// <summary>
        /// The extension for the generated files, set to txt if it does not build so you can´inspect the reult
        /// cs when everything works
        /// </summary>
        public static string Extension => "cs";

        public ObservableCollection<Prefix> Prefixes { get; } = new ObservableCollection<Prefix>();

        public ObservableCollection<BaseUnit> BaseUnits { get; } = new ObservableCollection<BaseUnit>();

        public ObservableCollection<DerivedUnit> DerivedUnits { get; } = new ObservableCollection<DerivedUnit>();

        public IReadOnlyList<BaseUnit> AllUnits => BaseUnits.Concat(DerivedUnits).ToList();

        public IReadOnlyList<Quantity> Quantities => AllUnits.Select(x => x.Quantity).ToList();
    }
}
