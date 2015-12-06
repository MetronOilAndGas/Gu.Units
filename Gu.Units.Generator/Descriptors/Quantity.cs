namespace Gu.Units.Generator
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    public class Quantity : MarshalByRefObject, INotifyPropertyChanged
    {
        private InverseOverload inverse;

        public Quantity(BaseUnit unit)
        {
            Unit = unit;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public BaseUnit Unit { get; }

        public string Name => Unit.QuantityName;

        public ObservableCollection<OperatorOverload> OperatorOverloads { get; } = new ObservableCollection<OperatorOverload>();

        public InverseOverload Inverse
        {
            get { return this.inverse; }
            internal set
            {
                if (Equals(value, this.inverse))
                    return;
                this.inverse = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
