namespace Gu.Units.Wpf.Demo
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Annotations;

    public class ViewModel : INotifyPropertyChanged
    {
        private Length _length = Length.FromMillimetres(1234.567);

        public event PropertyChangedEventHandler PropertyChanged;

        public Length Length
        {
            get { return this._length; }
            set
            {
                if (value.Equals(this._length))
                    return;
                this._length = value;
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