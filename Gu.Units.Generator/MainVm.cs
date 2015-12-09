namespace Gu.Units.Generator
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;
    using JetBrains.Annotations;
    using Wpf.Reactive;

    public class MainVm : INotifyPropertyChanged
    {
        public static readonly MainVm Instance = new MainVm();
        private readonly Settings settings;
        private readonly ConversionsVm conversions;
        private string nameSpace;

        private MainVm()
        {
            this.settings = Settings.FromResource;
            NameSpace = Settings.ProjectName;
            this.conversions = new ConversionsVm(this.settings);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Prefix> Prefixes => this.settings.Prefixes;

        public ObservableCollection<BaseUnit> SiUnits => this.settings.BaseUnits;

        public ObservableCollection<DerivedUnit> DerivedUnits => this.settings.DerivedUnits;

        public ConversionsVm Conversions => this.conversions;

        public string NameSpace
        {
            get
            {
                return this.nameSpace;
            }
            set
            {
                if (value == this.nameSpace)
                {
                    return;
                }
                this.nameSpace = value;
                this.OnPropertyChanged();
            }
        }

        public ICommand AddBaseUnit { get; }

        public ICommand AddDerviedUnit { get; }

        public void Save()
        {
            Persister.Save(Persister.SettingsFileName);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
