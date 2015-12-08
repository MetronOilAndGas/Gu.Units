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
        private BaseUnit baseUnit;

        public ConversionsVm(Settings settings)
        {
            this.settings = settings;
            PrefixConversions = new PrefixConversionsVm(settings);
            PartConversions = new PartConversionsVm(settings);
            BaseUnit = settings.AllUnits.FirstOrDefault(x => x.QuantityName == "Speed"); // for designtime
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public BaseUnit BaseUnit
        {
            get { return this.baseUnit; }
            set
            {
                if (Equals(value, this.baseUnit))
                {
                    return;
                }
                this.baseUnit = value;
                this.allConversions.SetSource(this.baseUnit?.AllConversions);
                PrefixConversions.SetBaseUnit(value);
                PartConversions.SetBaseUnit(value);
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
