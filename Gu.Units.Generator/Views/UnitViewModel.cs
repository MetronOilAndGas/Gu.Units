namespace Gu.Units.Generator
{
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    public abstract class UnitViewModel<TUnit> : INotifyPropertyChanged
        where TUnit : Unit
    {
        protected const string UnknownName = "Unknown";
        protected const string UnknownSymbol = "??";

        public UnitViewModel(TUnit unit)
        {
            Unit = unit;
        }

        public virtual event PropertyChangedEventHandler PropertyChanged;

        public TUnit Unit { get; }

        public bool IsUnknown => Unit.Name == UnknownName ||
                                 Unit.QuantityName == UnknownName ||
                                 Unit.Symbol == UnknownSymbol;

        protected bool IsEverythingOk()
        {
            if (!Unit.AllConversions.All(c => c.CanRoundtrip))
            {
                return false;
            }

            if (IsUnknown)
            {
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