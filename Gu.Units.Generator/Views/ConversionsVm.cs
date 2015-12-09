namespace Gu.Units.Generator
{
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;
    using Reactive;

    public class ConversionsVm : INotifyPropertyChanged
    {
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
                this.allConversions.SetSource(this.unit?.AllConversions);
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
    }
}
